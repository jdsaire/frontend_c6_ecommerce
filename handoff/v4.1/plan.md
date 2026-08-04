# handoff/v4.1 — Retrospective Diagnosis (executed in Auto Mode, documented after the fact)

This run (PR #5) was executed in Auto Mode rather than Plan Mode, at the user's request,
immediately after the principal reported three specific bugs found by using the app.
There was no forward-looking plan approved before work began — this file instead records
the root-cause diagnosis and fix approach actually used, in the same "verified against
actual source" register `handoff/v2.2/plan.md` uses, so the reasoning stays attached to
the commits rather than living only in commit messages.

## Context

Three reports came in against the `main` produced by merging PR #4 (Activity 4: input
validation, simulated authentication, checkout, a11y retrofit):

1. **Critical** — clicking "Sign In" from any entry point (header, either product-card
   prompt, the cart-summary link) served the host's 404 page, cutting off login, cart
   mutation, and checkout entirely.
2. **Major** — the product search field accepted text but offered no way to clear it
   short of backspacing, and no autocomplete help finding a product by partial name.
3. **Minor** — the header's "Sign In" status sat flush against the cart summary next to
   it, with no visual separation, at every breakpoint.

## Root-cause diagnosis (bug 1)

Confirmed directly, not assumed:

- `src/ShopEase/wwwroot/index.html` tracks `<base href="/" />`.
- `.github/workflows/deploy-pages.yml` rewrites that to
  `<base href="/frontend_c6_ecommerce/" />` in the CI-produced publish output only — the
  tracked source is never touched, by design, so local `dotnet run` and Codespaces keep
  working against `/`.
- Every internal link the original Activity 4 pass added used a **leading-slash** href:
  `href="/login"` (`AuthStatus.razor`, `ProductCard.razor` x2, `Products.razor`),
  `href="/checkout"` (`Products.razor`), `href="/products"` (`Checkout.razor` x2),
  `href="/login"` (`Checkout.razor`), and `Navigation.NavigateTo("/products")`
  (`Login.razor`). A leading slash is **origin-relative** — it resolves against the
  page's origin (`jdsaire.github.io`), not against `<base href>`. It bypasses the base
  rewrite entirely.
- Result on the live deploy: every one of those nine targets resolved to
  `jdsaire.github.io/login` (etc.), outside the `/frontend_c6_ecommerce/` path the app
  actually lives under.
- The SPA fallback (`404.html` → decode-redirect script) that normally rescues deep
  links on this static host only exists **under** `/frontend_c6_ecommerce/` in the
  publish output — a request to `jdsaire.github.io/login` never reaches it, so GitHub's
  own generic 404 was served instead.
- **This was a regression the original Activity 4 pass introduced, not a pre-existing
  gap**: `Layout/NavMenu.razor` (`href="products"`) and `Pages/Home.razor`
  (`href="products"`) already used the correct base-relative form, and continued
  working throughout. The bug was in the *new* auth/checkout links only.

Fix: switch all nine targets (and the one `NavigateTo` call, which resolves its argument
identically) to base-relative form, matching the pre-existing convention exactly.

## Search field (bug 2)

Verified in `Pages/Products.razor`: the field was a real `EditForm`/`DataAnnotationsValidator`/
`InputText` combination (correct per decision (1) from `handoff/v4/plan.md`), but purely
reactive-filter — typed text drove `ApplyFilters()` directly, with no independent way to
clear the field, and no suggestion mechanism.

Approach: rebuild as an ARIA 1.2 combobox — `role="combobox"` with `aria-expanded`,
`aria-controls`, `aria-autocomplete`, `aria-activedescendant`; a `role="listbox"` of
`role="option"` items; full keyboard support (arrows to navigate, Enter to select,
Escape to dismiss-then-clear); a real `<button>` clear control that returns focus to the
field. Suggestions are keywords extracted from the catalog's own product names
(`MockProductData.GetSeedProducts()`), so a partial word matches mid-name — verified by
running the extraction algorithm against the real catalog data before committing:
`"la"` → `Lamp`, `Laptop`.

Required switching the input from `<InputText>` to a plain `<input>`, since a combobox
needs per-keystroke value control plus its own keydown handling, and `<InputText>`
cannot combine `@bind-Value:event="oninput"` with `@bind-Value:after`. Validation
(`SearchModel`, `EditContext`, `DataAnnotationsValidator`, `ValidationMessage`) is
unchanged.

## Header spacing (bug 3)

Verified in `Layout/MainLayout.razor.css`: `.top-row` sets no `gap` of its own; its only
spacing rule, `.top-row ::deep a { margin-left: 1.5rem; }`, applies to anchors and
therefore separates the "Sign In" link from whatever precedes it, but does nothing for
the element *after* it — `AuthStatus`, sitting directly against `CartSummary`.

Fix: a matching `1.5rem` right margin on `.auth-status`, added to `site.css` (this
project's own component styling) rather than the layout's scoped `MainLayout.razor.css`
(template-owned), per the file-ownership boundary `v3` established. Confirmed no
`@media` override touches it — the only breakpoint rule affecting `.cart-summary`
changes internal `gap`/`padding`, not layout — so the gutter holds at every width.

## Commit sequence (as executed)

1. `fix(nav): resolve sign-in and checkout links against the app base path`
2. `feat(ui): add clearable search with accessible catalog autocomplete`
3. `style(layout): separate the header sign-in status from the cart summary`

Branch `fix/v4-navigation-search-and-header-spacing`, opened as PR #5 against
`deploy/v4-secure-coding` (the branch these bugs were found on, before PR #4 had merged).
