# DEPLOY-C6-SecureCoding-v4_0 + Checkout/A11y/Timing Patch — Execution Plan

Combines the core prompt (`DEPLOY-C6-SecureCoding-v4_0.xml`, Activity 4: input
validation, simulated authentication, security testing) with a patch delivered after
authoring (`PATCH-DEPLOY-C6-SecureCoding-v4_0-checkout-a11y-and-deploy-timing.xml`)
that inserts a checkout screen, retroactive Activity-3-equivalent styling/accessibility
coverage for the two new screens, and a standing deploy-timing measurement. The patch's
edits were applied as insertions into the core prompt's existing task sequence, not a
rewrite — approved together as one plan before any branch existed.

## Structural problem this run resolves up front

The capstone brief's Activity 4 text doesn't map onto this codebase as written: it says
"modify your Blazor form" against an app with no form anywhere, names ASP.NET Identity
against a static WASM host with no server, and asks to "prevent SQL injection" against a
simulated database that builds no SQL at all. All three are resolved by building the
real, correct client-side thing and labeling honestly what is simulated — see
`docs/security-decisions.md` for the durable record of each resolution.

## Verified before writing this plan

- HEAD was `91e67df` on `main`, exactly as the prompt's `verified_state` claimed.
- Grepping `<input`, `<EditForm`, `<form`, `InputText`, `MarkupString` across every
  `.razor` file returned nothing — no form, no text input, no XSS sink anywhere.
- `ShopDatabase` executes no SQL — an in-memory `List<Product>` whose comments already
  model parameterized (`@productId`) style.
- `dotnet build` on `main` was clean (0/0) before any change.
- A scratch restore of `Microsoft.AspNetCore.Components.Authorization` 10.0.5 succeeded
  — decision (2)'s primary path applied, not its fallback.
- No browser-automation or headless-rendering tool was available this session — every
  UI/keyboard/contrast claim in the completion report is CSS-reasoned, not run.
- This worktree's branch was already sitting exactly at `main`'s HEAD with a clean tree,
  so `deploy/v4-secure-coding` was created with `git checkout -b` directly here.

## Interpretations flagged and how they were resolved

1. **Checkout contact field** — the patch explicitly left this open ("pick one, not
   both"). Asked directly; **Email** was chosen, built with DataAnnotations'
   `[EmailAddress]`.
2. **Commit count (16 landed, not the originally-planned 15)** — the core+patch tasks
   specify commit messages that sometimes bundle two genuinely separate items (auth
   provider class+store vs. its DI/cascading wiring; login form vs. header sign-in
   surfacing; checkout form vs. its cart-summary entry point). Splitting those three
   pairs, honoring "ONE COMMIT PER ITEM" more granularly, was planned to produce 15. A
   16th commit was added during the mandated final-verify grep (task 12's own
   instruction to "fix any hit") after two headings/table cells in the docs were found
   to say XSS was "prevented" — technically defensible for Blazor's own default
   encoding, but stricter than the honesty invariant's blanket wording calls for. See
   the completion report's Authorized Deviations section.
3. **"Unit-testable in isolation"** — no test project or framework was added, since that
   would be a second new NuGet package (an explicit stop condition). The validation
   service is a plain static class with zero Blazor/DI/UI coupling instead.
4. **Search input UX** — `EditForm`/`DataAnnotationsValidator` wraps the search box, but
   reactivity matches the existing category/sort selects: `@bind-Value:after` triggers
   `ApplyFilters` on every keystroke (no separate submit button), since `InputText`
   already updates its bound value on every keystroke by default.

## Design decisions

- **Validation service** (`Services/InputValidationService.cs`): an allow-list regex
  covering letters/digits/spaces plus the punctuation actually present in the catalog,
  addresses, and usernames (`.,&'-/#_`); an injection-metacharacter detector; a shared
  length bound; a non-mutating `Sanitize` (trim/whitespace-collapse only — rejection
  happens via a DataAnnotations attribute, `SafeText`, so the user sees why, not a
  silent rewrite). Passwords are validated for presence/length only, never sanitized —
  a password must compare byte-exact.
- **Demo accounts** (`Services/DemoAccountStore.cs`): `demo_shopper1` /
  `Demo#2026Test1` and `demo_shopper2` / `Demo#2026Test2`, shown openly on the login
  page.
- **Authentication**: `Services/DemoAuthenticationStateProvider.cs` extends Blazor's real
  `AuthenticationStateProvider`; `AddAuthorizationCore()` + `AddScoped<AuthenticationStateProvider,
  DemoAuthenticationStateProvider>()` in `Program.cs`; `<CascadingAuthenticationState>`
  wraps the router in `App.razor`.
- **Cart gating**: one additive, non-frozen `[Parameter] public bool CanModifyCart` on
  `ProductCard` (default `true`); `Products.razor` wraps each card in `<AuthorizeView>`,
  passing `true`/`false` per branch. `Cart.AddProduct` itself is untouched.
- **Checkout**: `Pages/Checkout.razor`, five non-financial fields, gated in the page
  itself on sign-in AND non-empty cart (three-way branch: not-authorized / empty-cart /
  form), an additive `Cart.ClearCart()` appended after `CalculateTotal()`, an in-memory
  `DEMO-{timestamp}` confirmation.
- **Styling/a11y retrofit**: reuse existing `site.css` tokens; one token addition,
  `--shopease-danger`, naming the hex the remove control already used inline (not a new
  color); the app's own 641px/1025px breakpoints; real `<label for=...>` on every input;
  `aria-live="polite"` on every validation message and the checkout confirmation; the
  existing focus-visible rule extended to every new interactive element.

## Ordered commit sequence (as planned)

1. `feat(security): add input validation and sanitization service`
2. `feat(ui): add validated product search to the storefront`
   — **Gate 1**, STOP for approval —
3. `feat(auth): add simulated authentication state provider`
4. `feat(auth): wire cascading authentication state into the app`
5. `feat(auth): add login form with validation and sign-out`
6. `feat(auth): surface sign-in state in the header`
7. `feat(auth): require sign-in to add products to the cart`
8. `feat(checkout): add checkout screen with validated shipping form`
9. `feat(checkout): add proceed-to-checkout entry point to the cart summary`
10. `feat(checkout): gate checkout on sign-in and cart contents, wire order confirmation`
11. `style: apply responsive styling and accessibility parity to login and checkout`
    — **Gate 2**, STOP for approval —
12. `feat(security): add security test page and documented test cases`
13. `docs: record security and authentication decisions`
14. `docs: update READMEs and learning-mode for the security pass`
15. `docs: archive v4 plan and completion report`

(An unplanned 16th commit, `docs: tighten XSS phrasing to match the client-side honesty
invariant`, was added between 14 and 15 during the mandated verify-pass grep — see the
completion report.)

## Security-test probe set

1. `<script>alert('test')</script>` — expect rejected.
2. `' OR '1'='1` — expect rejected.
3. `Robert'); DROP TABLE Products;--` — expect rejected.
4. `27-Inch Monitor` (real catalog name) — expect **accepted**, proving the allow-list
   doesn't over-reject legitimate hyphenated names.
5. A 500-character string — expect rejected on the length bound.
