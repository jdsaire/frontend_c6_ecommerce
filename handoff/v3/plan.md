# DEPLOY-C6-LastChangesAndActivity3-v2_2 — Part 2 Execution Plan

This is the Part 2 half of a two-part plan. Part 1's half lives in
[`../v2.2/plan.md`](../v2.2/plan.md). Both halves were approved together,
before either branch existed. Part 2 does not begin until PR #2 (Part 1) is
merged and `main` is verified to contain it — see the hard gate at the end
of `../v2.2/plan.md`.

## Scope

The full scope of capstone Activity 3: CSS styling for readability and
hierarchy, responsive design via media queries across mobile/tablet/desktop,
and an accessibility pass — against the `main` produced once PR #2 merges.

## Verified before writing this plan

- `wwwroot/css/app.css`: 387 lines, zero `@media` rules, held every
  storefront/landing/product-card/cart-summary class. No
  `wwwroot/css/site.css` existed yet — the capstone brief's Activity 3 Step 1
  names that file explicitly.
- `index.html` linked `bootstrap.min.css` → `app.css` → `ShopEase.styles.css`.
- Existing breakpoint convention: the app already used 641px as its single
  sidebar/nav cutover in `Layout/MainLayout.razor.css` and
  `Layout/NavMenu.razor.css`, both scoped and both predating this run.
- Contrast was computed, not guessed, against the real hex values in
  `app.css`: accent-on-white and muted-on-white already passed WCAG AA
  (≈5.18:1 and ≈5.98:1); the border color `#e2e5ea` measured ≈1.26:1 against
  white — a real, pre-existing failure against the 3:1 non-text/UI-boundary
  threshold.
- Focus indicators: the existing `.btn:focus`/`.form-control:focus` rule in
  `app.css` did not cover the quantity stepper, the remove control, or the
  toolbar's plain `<select>` elements — not suppressed, just not extended to
  them yet.
- No browser-rendering tool was available in this session (confirmed via a
  deferred-tool search, not assumed) — governing how every responsive and
  accessibility claim in this part would need to be reported.

## Part 2 — branch `deploy/v3-activity3-responsive-ux`, PR #3

Branched only from a `main` verified (via `git merge-base --is-ancestor`) to
contain all three of Part 1's commits — never from Part 1's own branch.

1. **`style: introduce site.css and move storefront styles into it`** —
   create `wwwroot/css/site.css`; move (not rewrite) this project's own
   storefront/landing/product-card/cart-summary rules out of `app.css`,
   leaving the Blazor template's base styles behind; link `site.css` in
   `index.html` right after `app.css`. Verified as a lossless move by
   concatenating the two resulting files and diffing against the pre-split
   `app.css` — byte-identical.
2. **`style: improve readability, spacing, and visual hierarchy`** —
   strengthen price prominence, keep category/stock clearly secondary,
   more deliberate whitespace, consistent primary-button sizing across the
   hero CTA, add-to-cart button, and show-more button.
3. **`style: add responsive breakpoints for mobile, tablet, and desktop`** —
   mobile-first, reusing the app's existing 641px line and adding a new
   1025px line: storefront grid 1→2→3 columns, toolbar stacked→row, landing
   hero heading smaller on mobile, header cart summary slightly more
   compact on mobile.
4. **`a11y: meet contrast and keyboard navigation requirements`** — replace
   the failing border color with one that clears 3:1; extend the existing
   focus-ring style to the stepper, remove control, and toolbar selects;
   verify tab order and `alt`/`aria-hidden` coverage.
5. **`docs: record Activity 3 styling and CSS organization decisions`** —
   build/link/frozen-contract/AI-name/real-DB verification pass, plus
   `docs/activity-3-decisions.md` recording the `site.css` rationale, the
   breakpoint choices, the paging tradeoff, and the `/cart-test` scope
   boundary.
6. **`docs: archive v2.2 and v3 plans and completion reports`** — update
   affected READMEs and `learning-mode/`, add a numbered Activity 3
   learning-mode file, archive both `handoff/v2.2/` and `handoff/v3/`.

Then open PR #3 against `main`, describing scope, commits, the `/cart-test`
boundary, and which claims are browser-verified versus CSS-reasoned. Left
unmerged.

## Verification approach, stated up front

No browser-rendering tool was available this session. Every responsive and
visual-hierarchy claim is CSS-reasoned, not claimed as a rendered-browser
PASS. Contrast is measured via actual WCAG relative-luminance math on the
real hex values in the file. Keyboard operability and tab order are reasoned
from actual DOM order (no `tabindex` anywhere, confirmed by grep). The
frozen Activity 1/2 contract is verified by direct `git diff` against merged
`main`, not by re-reading and eyeballing.
