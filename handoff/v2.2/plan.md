# DEPLOY-C6-LastChangesAndActivity3-v2_2 — Part 1 Execution Plan

This is the Part 1 half of a two-part plan. Part 2's half lives in
[`../v3/plan.md`](../v3/plan.md). Both halves were approved together, before
either branch existed, per the source deploy prompt's "plan first, once, for
both parts" rule.

## Context

`jdsaire/frontend_c6_ecommerce` had just merged PR #1 (the storefront-bridge
run). Three small things were left before Activity 3 could start: a dead sale
section on Home, a header-alignment CSS bug, and no paging on a 12-product
grid. This part closes all three out in exactly three commits, opens PR #2,
and stops — Part 2 does not branch until a human merges PR #2 and it's
verified on `main`.

## Preflight — confirmed

- GitHub access: `gh auth status` → logged in as `jdsaire`; repo confirmed as
  `origin`.
- `origin/main` HEAD: `c17ecde2b7928a9169104b0abf43acae7a6f95b7` — matched the
  prompt's stated baseline exactly, no drift.
- `dotnet build` on `main` at that HEAD: 0 warnings, 0 errors.
- No browser-rendering tool available this session (checked directly, not
  assumed) — noted up front since it also governs Part 2's verification
  approach.

## Verified against actual source (not just the prompt's summary)

- `Pages/Home.razor` — exactly two sections: `landing-hero` (kept) and
  `landing-sale` (to remove), with dead `SaleProductIds`, `_saleProducts`,
  `OnInitialized` in `@code`.
- `Pages/Products.razor` — renders `_displayedProducts` in one
  `storefront-grid` with no paging.
- **Cart-summary bug, root cause** — confirmed directly in
  `Layout/MainLayout.razor.css`: base `.top-row { justify-content: flex-end; }`
  overridden inside `@media (max-width: 640.98px) { .top-row { justify-content: space-between; } }`.
  `Layout/CartSummary.razor` renders a single `<div class="cart-summary">` —
  no anchor or `.btn-link` — so the adjacent `.top-row ::deep a, .top-row ::deep .btn-link { margin-left: 0; }`
  rule inside that same mobile media block is dead too.

## Part 1 — branch `deploy/v2.2-last-changes`, PR #2 (exactly 3 commits)

1. **`refactor(pages): remove the sale campaign section from Home`** — delete
   the `landing-sale` section, its dead `@code` members, and its CSS rules.
   Hero and CTA untouched.
2. **`fix(layout): keep the header cart summary right-aligned at all breakpoints`**
   — delete the `space-between` override and the dead `::deep a` rule from
   the mobile media query in `MainLayout.razor.css`, so base `flex-end` holds
   unconditionally. No competing rule added; `CartSummary.razor` untouched.
3. **`feat(ui): add progressive show-more paging to the storefront grid`** —
   6 products initially, +6 per press, a real `<button>`, a live
   `aria-live="polite"` "Showing X of Y products" count, control hidden once
   exhausted, offset reset on any filter/sort change. Applies at all
   breakpoints.

Then: confirm build clean and exactly three commits, push, open PR #2
against `main` titled exactly `Deploy c6 storefrontbridge v2 2`, leave
unmerged, **stop**.

## Hard merge gate

After PR #2 is opened: no Part 2 branch, no Activity 3 code, nothing staged,
until a human merges PR #2 and `origin/main` is fetched and verified to
contain all three commits above as ancestors.
