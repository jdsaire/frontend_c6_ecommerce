# 04 — Input Validation and Authentication

## Picking Up From 03

`03-Responsive-UI-and-Accessibility.md` covered making the storefront look right and
work without a mouse. Everything up to that point trusted whatever the user clicked —
there was no text input anywhere in the app to worry about, and no concept of "signed
in" at all. This file covers Activity 4: teaching the app to validate what a visitor
types, and to require signing in before the cart can be changed.

## What Input Validation and Sanitization Mean

**Input validation** is checking that user input follows expected rules before doing
anything with it — is it the right length, does it contain only expected characters.
**Sanitization** is cleaning up input that's basically fine but needs tidying (trimming
stray whitespace, for instance). This app puts both in one place,
[`InputValidationService`](../src/ShopEase/Services/InputValidationService.cs) — a plain
C# class with no Blazor or UI code in it at all, so the rules it enforces can be reasoned
about (and tested) completely separately from any page that uses them. Every text field
this app accepts — the product search box, the login form, the checkout form — runs
through the exact same service, via a shared `SafeText` rule rather than five separate
copies of similar logic.

**Important honesty note, worth stating plainly**: everything this service does runs in
the visitor's own browser. A visitor with developer tools can call the underlying `Cart`
or navigation methods directly and skip the validation layer entirely. This service
improves data quality and demonstrates the technique — it is not a security boundary by
itself. See [`docs/security-decisions.md`](../docs/security-decisions.md) for the full
position, which this file's code follows throughout.

## SQL Injection With No SQL

The capstone brief asks this activity to "prevent SQL injection." This app's simulated
database, [`ShopDatabase`](../src/ShopEase/Services/ShopDatabase.cs) (introduced back in
`01-Business-Logic-Foundations.md`), never builds a SQL query string at all — there is
nothing here for an attacker to inject *into*. What this activity actually does is reject
the metacharacter patterns associated with SQL injection (a stray `'`, a `;`, an `OR`
tautology) wherever text is typed, which demonstrates the validation technique the brief
is asking for without claiming a query got fixed that was never broken. In a real app
with a real MySQL backend, the actual fix for SQL injection is a parameterized query on
the server — not filtering on the client.

## Cross-Site Scripting and Blazor's Default Encoding

**Cross-site scripting (XSS)** is when an attacker gets their own script to run inside
someone else's page — classically, by typing `<script>...</script>` into a field that
gets echoed back to other visitors unescaped. Blazor's Razor syntax HTML-encodes any
value you interpolate with `@` by default, so `<script>` typed into a field and echoed
back with `@myField` renders as the literal text `<script>`, not a running script. This
app had zero uses of `MarkupString` (the one thing that would turn that protection back
off) before this activity, and still has zero after it —
[`Pages/SecurityTest.razor`](../src/ShopEase/Pages/SecurityTest.razor) proves it live by
rendering a script-tag probe string straight through `@` interpolation.

## The Search Box and Login Form: EditForm and DataAnnotations

Before this activity, this app had no `<form>`, no `<input>`, nothing to type into
anywhere — Activity 4's Step 1 ("modify your Blazor form") had no existing target. Two
real inputs were added instead of a contrived demo: a product **search box** on the
storefront (a real retail affordance) and a **login form** (which Step 2 needed anyway).

Both use Blazor's `EditForm` component paired with `DataAnnotations` — attributes like
`[Required]` and the custom `[SafeText]` attribute this activity adds, placed directly on
a small model class's properties. `EditForm` tracks an `EditContext` for that model,
`<DataAnnotationsValidator />` wires the attributes up to it, and `<ValidationMessage>`
shows the resulting error next to the field it belongs to. This is the idiomatic way
Blazor expects form validation to be built, which is what Step 1 is really asking to see
demonstrated — not just "some text got checked somehow."

## Authentication Without a Server

Activity 4's Step 2 names ASP.NET Identity, which needs a real server to run against.
This app is Blazor WebAssembly on static GitHub Pages hosting — there is no server here
at all. Rather than fake a `bool isLoggedIn` flag (which would skip the actual mechanism
this step is meant to teach), this activity builds against Blazor's *real* authentication
plumbing:

- **`AuthenticationStateProvider`** is the abstraction real ASP.NET Identity apps use to
  answer "who is signed in right now?"
  [`DemoAuthenticationStateProvider`](../src/ShopEase/Services/DemoAuthenticationStateProvider.cs)
  extends it, holding the current user's identity in memory only.
- **`CascadingAuthenticationState`**, wrapped around the app's router in `App.razor`,
  makes that authentication state available to every page and component below it,
  without each one having to ask for it individually.
- **`AuthorizeView`** is a component that renders one thing when the visitor is signed in
  (its `<Authorized>` section) and another when they aren't (`<NotAuthorized>`) — used on
  the login page itself, the header, the product cards, and the checkout entry point.

Only the credential store,
[`DemoAccountStore`](../src/ShopEase/Services/DemoAccountStore.cs), is simulated: a
short, fixed list of obviously-fake accounts, shown openly on the login page since
they're demo values, not secrets. Signing in doesn't survive a page refresh — that's
deliberate. Persisting session state across refreshes is Activity 5's job, and doing it
here would have gone beyond this activity's scope.

## Gating the Cart on Sign-In

Step 2 also requires that only signed-in visitors can add products to the cart. The
gate lives entirely in the **calling layer** — `Products.razor` wraps each product card
in `<AuthorizeView>` and passes a plain `CanModifyCart` boolean down to `ProductCard`,
which has no idea what "authentication" even means; it just shows a "Sign in to add to
cart" prompt instead of the Add button when told to. `Cart.AddProduct` itself was never
touched — the four methods Activity 1 fixed
(`AddProduct`, `RemoveProduct`, `DisplayCartItems`, `CalculateTotal`) are exactly the
same as they were in `01-Business-Logic-Foundations.md`. Putting the check there instead
of inside `Cart` keeps the business-logic class reusable and testable independent of
however a particular page decides to gate it.

## The Checkout Screen

A later patch to this activity's plan added a checkout screen, to give the security work
above a second, independent form to demonstrate against.
[`Pages/Checkout.razor`](../src/ShopEase/Pages/Checkout.razor) collects five fields —
Full Name, Shipping Address, City, Postal/ZIP Code, and Email — through the exact same
`SafeText`/`InputValidationService` rules as the login form, and deliberately collects
nothing resembling payment data (no card number, no CVV). It's reachable only from the
cart summary, only once the visitor is both signed in *and* the cart has something in it
— `<AuthorizeView>` again, plus a plain `Cart.Items.Any()` check, both in the page itself
so a visitor who lands there directly (not just by clicking the link) still sees a clear
explanation rather than a broken form. A valid submission shows an in-memory "order
confirmed" message with a demo reference like `DEMO-20260803181357` — clearly not a real
order-management identifier — and clears the cart through one new method,
`Cart.ClearCart()`, added after the four frozen methods rather than touching any of them.

## What's Next

Activity 5 (persisted state management) is the last remaining delivery against this
repository. Right now, both the cart and the signed-in session reset the moment the
browser tab refreshes — Activity 5's job is to make the cart (and only the cart; the
brief doesn't ask for persisted login) survive that refresh using the browser's storage
APIs.
