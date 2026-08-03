# Security and Authentication Decisions

This document records the durable reasoning behind Activity 4's secure-coding pass —
why each simulated element is simulated, and the one rule that governs how every
security claim in this repository is worded.

## The Client-Side Honesty Invariant

Stated once, in full, because it governs everything else in this document:

> Every control built in this run runs in the browser, where a user can bypass it with
> developer tools. No code comment, README, doc, commit message, or PR body in this
> repository states or implies that this app is "secure against" SQL injection or XSS,
> that it "prevents" attacks, or that its authentication protects any data. The correct
> framing, used consistently throughout: these are the client-side halves of defenses
> whose enforcing half must live on a server this app does not have. Validation here
> improves data quality and demonstrates the technique; it is not a security boundary.

If a sentence anywhere in this repository would reassure a reader that this app is safe
to put real credentials or real payment data into, that sentence is wrong.

## Why ASP.NET Identity Isn't Here

Activity 4's brief names ASP.NET Identity, which is built to run against a server-side
ASP.NET Core host with a real backing database. This app is Blazor **WebAssembly**,
deployed as static files to GitHub Pages — there is no server process for Identity to
run on, and adding one would be a backend, which is outside this project's scope
ceiling.

**Resolution**: build against Blazor's *real* authentication abstraction —
`AuthenticationStateProvider`, `CascadingAuthenticationState`, `AuthorizeView` — with a
custom in-memory provider
([`DemoAuthenticationStateProvider`](../src/ShopEase/Services/DemoAuthenticationStateProvider.cs))
standing in for the Identity-backed store. The API surface, the cascading auth state,
and the component-level authorization are genuine Blazor authentication plumbing; only
the credential store
([`DemoAccountStore`](../src/ShopEase/Services/DemoAccountStore.cs)) is simulated, and
it says so directly in its own doc comment. Hand-rolling a `bool isLoggedIn` field would
have been both less correct and less instructive — it would have skipped the actual
`AuthorizeView`/cascading-state mechanics this activity is meant to demonstrate.

The `Microsoft.AspNetCore.Components.Authorization` package (version-matched to this
project's existing 10.0.5 references) was restored successfully in this run, so the
primary path above was used — no fallback was needed.

Sign-in state lives in `DemoAuthenticationStateProvider`'s memory only, for the lifetime
of the browser tab, and is **intentionally lost on refresh**. Persisting it is Activity
5's job; doing it here would breach this run's scope ceiling. This is a deliberate
boundary, not a defect.

## SQL Injection With No SQL

Activity 4's brief also asks to "prevent SQL injection." This app's simulated database,
[`ShopDatabase`](../src/ShopEase/Services/ShopDatabase.cs), builds no query strings at
all — it's an in-memory `List<Product>`, and its existing comments already model
parameterized (`@productId`) style rather than string concatenation. There is no
injectable query anywhere in this codebase to fix.

**Resolution**: implement input validation
([`InputValidationService`](../src/ShopEase/Services/InputValidationService.cs)) that
rejects the metacharacter patterns associated with SQL injection (`'`, `--`, `;`,
tautologies like `OR 1=1`), applied to every text input this app accepts. This
demonstrates the technique the brief is asking for. It does **not** mean this app
"prevents SQL injection" — there is no SQL for it to prevent injection into. In a real
implementation with a real MySQL backend, the actual defense would be parameterized
commands via ADO.NET or Entity Framework on the server — not input filtering on the
client, and not string concatenation. Input validation and parameterized queries are
different defenses for different layers; this app only has the client-side layer to
demonstrate.

## XSS: Preserving What Was Already True

Blazor's Razor rendering HTML-encodes interpolated values by default. Before this run,
grepping `MarkupString` across every `.razor` file in this project returned zero
matches — this app had no XSS sink to begin with, because nothing ever opted out of the
default encoding.

**Resolution**: preserve and demonstrate that property rather than bolt on a redundant
encoder. `Pages/SecurityTest.razor` renders a `<script>`-style probe string back to the
screen through normal `@` interpolation and shows the literal text, not an executed
script — proof the property still holds after this run's changes. No file in this
project uses `MarkupString`. If a future contributor is tempted to add one to render
user-supplied content, that would reopen exactly the sink this run confirms doesn't
exist — don't.

## No Payment Fields

The checkout screen this run adds ([`Pages/Checkout.razor`](../src/ShopEase/Pages/Checkout.razor))
collects Full Name, Shipping Address, City, Postal/ZIP Code, and Email — and nothing
resembling payment-card data: no card number, no CVV, no expiry, no billing address
distinct from shipping. This is not a scope-cutting shortcut. Collecting anything that
looks like real payment-card data in a public teaching repository is the wrong thing to
build, independent of whatever this project's scope ceiling happens to say — there is no
PCI-compliant handling here, no encryption at rest, nothing. The checkout page states
directly on itself that it's a demo submission and that no payment is collected or
processed. On a valid submission, the only output is an in-memory, non-persisted
confirmation with a demo reference (`DEMO-{timestamp}`) — clearly not a real
order-management identifier — after which `Cart.ClearCart()` empties the cart. Nothing
submitted is written to storage.

## Retroactive Coverage: Login and Checkout

Activity 3's styling, responsive-design, and accessibility pass (v3, PR #3) audited only
the screens that existed at that time — landing, storefront, and layout. It could not
have covered the login screen this run's core prompt introduces, or the checkout screen
this run's patch introduces, because neither screen existed yet.

**Resolution**: Activity 3 predates these two screens; this run closes that gap for
both, applying the identical standard, within this same run rather than deferring it to
a future pass. Concretely: both screens reuse `site.css`'s existing design tokens
(`--shopease-accent`, `--shopease-ink`, `--shopease-muted`, `--shopease-border`) rather
than a second palette; the one addition, `--shopease-danger`, isn't a new color — it
names the hex the remove control already used inline, already measured at ~6.54:1 on
white in the v3 report. Both screens use the app's established 641px/1025px mobile-first
breakpoints. Contrast was computed via the same relative-luminance method the v3 report
used, not asserted — see `handoff/v4/completion-report.md` for the measured ratios.
Every input has a real `<label for=...>`; validation errors and the checkout
confirmation are `aria-live="polite"`, matching the pattern already established by the
storefront's "Show more" count; every control is a native `<input>`/`<button>`/`<a>`
with no `tabindex` anywhere, matching v3's precedent. Along the way, this pass also fixed
a latent AA gap in the scaffold's own `app.css` — `.validation-message` was plain `red`
(~4.0:1, under the 4.5:1 body-text minimum) — by overriding it with `--shopease-danger`,
which benefits the pre-existing product-search validation message too, not only the two
new screens.

## Summary Table

| Brief asks for | This app has | Resolution |
|---|---|---|
| Modify your Blazor form | No form existed | Added a real `EditForm`-based search box and login form (Step 1), plus a checkout form (patch) |
| ASP.NET Identity | No server to run it on | Real `AuthenticationStateProvider`/`AuthorizeView` abstraction, simulated in-memory store |
| Prevent SQL injection | No SQL anywhere | Input validation against injection-associated patterns; documented that parameterized queries are the real server-side defense this app doesn't have |
| Prevent XSS | Already prevented by Blazor's default encoding | Preserved the no-`MarkupString` property, demonstrated it on `/security-test` |
