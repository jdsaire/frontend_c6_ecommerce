# DEPLOY-C6-StorefrontBridge-v2_1 — Execution Plan

## Context

`jdsaire/frontend_c6_ecommerce` currently reads as an assignment demo: three
pages (Home is the untouched Blazor scaffold, Products opens with "Activity
2: Product Listing" narration, Cart Test exists only as Activity 1 evidence),
four seed products, one-unit-only add-to-cart, no imagery, no filters, no
persistent cart indicator. A visual inspection (`P-2.1-c6-ecommerce.txt`)
flagged this as not reading like a real electronics retailer, and asked for a
bridge run — between Activity 2 and Activity 3 — that converts the app into a
credible storefront without doing Activity 3's responsive/accessibility work
and without touching the graded Activity 1/2 contract underneath it.

This plan implements that bridge: expanded catalog with stock and imagery, a
retail `ProductCard` with a quantity stepper (1–10) and explicit removal,
category/price filtering, a persistent header cart summary, a real landing
page, and Cart Test removed from the app's visible navigation. One deviation
from the source prompt is authorized for this run (see below): Cart Test is
unlinked from navigation rather than deleted, per explicit instruction, since
the file itself is graded Activity 1 evidence worth keeping live in the repo
rather than only in git history.

## Preflight — confirmed

- `jdsaire/frontend_c6_ecommerce`, HEAD `4733746` on `main` — matches, no drift.
- gh CLI authenticated as `jdsaire` (keyring), repo access confirmed.
- `dotnet build` on current `main`: **0 errors, 0 warnings.**
- `P-2.1-c6-ecommerce.txt` and the capstone KEY read in full.
- Frozen-contract files read directly: `Models/Product.cs`, `Services/Cart.cs`,
  `Services/ShopDatabase.cs`, `Pages/ProductCard.razor`, `Pages/Products.razor`,
  `Pages/Home.razor`, `Pages/CartTest.razor`, `Layout/MainLayout.razor`,
  `Layout/NavMenu.razor` — all match the prompt's `verified_state` exactly.
- `docs/`, `handoff/v1/`, `learning-mode/` all exist as described; v1's link
  count (87/87) and its verification method (a script resolving every
  relative markdown link, external `http(s)://` links excluded) are in
  `handoff/v1/completion-report.md` — this run's `docs-sync`/`verify` steps
  reuse the same method.
- Current repo is a git **worktree** on a pre-existing local branch,
  identical to `main` (clean, no unique commits). Branch setup: create and
  checkout `deploy/v2-storefront-bridge` from `main` in this worktree
  (`git checkout -b deploy/v2-storefront-bridge main`) before the first commit
  — this does not require checking out `main` itself.

## Two gated decisions — resolved

1. **Product imagery** → locally-authored SVG category placeholders. No
   external source, no license question, renders offline and on Pages. This
   is the prompt's own designated fallback for exactly the situation here (no
   reliable way to positively confirm a real photo's redistribution license
   from this environment).
2. **Cart Test visibility** → unlink only. Remove the "Cart Test" entry from
   `NavMenu.razor` and stop promoting `/cart-test` in `README.md` /
   `docs/how-to-run.md`. Keep the `@page "/cart-test"` directive intact, so
   the page still builds, still renders, and is still directly reachable by
   URL — exactly as `docs/how-to-run.md` already documents deep-linking on
   this static host. Nothing in the app's browsable interface (nav, Home,
   storefront) surfaces it; the file and its route are otherwise untouched.
   **This overrides the source prompt's task 13 ("Delete Pages/CartTest.razor")
   and its architecture-table "DELETED" entry — recorded as an authorized
   deviation in the Completion Report.**

## Frozen Activity 1/2 contract — trace

| Frozen member | Plan | Survives? |
|---|---|---|
| `Product.ProductID/Name/Price/Category` | Untouched; only new members (`Stock`, image ref) added | Yes |
| `Product.GetDetails()` exact format | Untouched | Yes, byte-identical |
| `Cart.AddProduct(Product)` | Untouched; reused verbatim as the increment primitive | Yes |
| `Cart.RemoveProduct(int)` | Untouched; reused verbatim as the explicit line-removal primitive | Yes |
| `Cart.DisplayCartItems()` | Untouched | Yes |
| `Cart.CalculateTotal()` | Untouched | Yes |
| `ProductCard.razor` `[Parameter] Product Product` | Untouched | Yes |
| `ProductCard.razor` `[Parameter] EventCallback<Product> OnAddToCart` | Untouched; still the "0→1" add path | Yes |
| `ProductCard.razor` location (`Pages/`) | Untouched | Yes |

No planned change requires touching any of the above — no STOP condition hit.

## Annotation → commit mapping

| Annotation (`P-2.1-c6-ecommerce.txt`) | Commit(s) |
|---|---|
| look-and-feel: retail feel over assignment framing, BestBuy-inspired | storefront rebuild, baseline styling |
| look-and-feel: placeholder images from a catalog | models (image field), imagery assets |
| look-and-feel: image+name+price+category+stock in one card | ProductCard rebuild |
| look-and-feel: category filter + price sort, dropdowns/icons | filters commit |
| dashboard <1>: Home → landing page, sale campaign, links to cart | Home rebuild |
| dashboard <2>: Products → storefront, grid not vertical list, cart summary top-right, About gone | storefront rebuild; cart summary + About removal |
| dashboard <3>: Cart Test disappears | Cart Test unlink (revised per user override above) |
| logic: 4 → 10–12 products | catalog expansion |
| logic: no per-unit +/− control, min 1 max 10 | cart quantity helpers; ProductCard stepper |
| logic: no remove control; resolve qty-zero-vs-explicit | resolved: floor 1, explicit trash-icon removal only, decrement at 1 is a no-op — recorded in docs-decisions, implemented in cart helpers + ProductCard |
| logic: out-of-stock/insufficient-stock not handled, log the deferral | docs-decisions commit |

Every annotation maps to at least one commit. None dropped.

## Proposed catalog (12 products, 4 categories)

Original four kept verbatim (so v1's documented arithmetic stays traceable):

| Product | Category | Price | Stock |
|---|---|---|---|
| Laptop | Electronics | $999.99 | 8 |
| Wireless Mouse | Electronics | $24.99 | 40 |
| Coffee Mug | Home Goods | $9.99 | 60 |
| Desk Lamp | Home Goods | $34.50 | 25 |

New eight, generic wholesale-electronics naming (no BestBuy SKUs/brands/trade dress):

| Product | Category | Price | Stock |
|---|---|---|---|
| External Hard Drive 1TB | Electronics | $64.99 | 22 |
| Mechanical Keyboard | Computer Accessories | $79.99 | 18 |
| 27-Inch Monitor | Computer Accessories | $229.99 | 10 |
| USB-C Hub | Computer Accessories | $39.99 | 25 |
| 1080p Webcam | Computer Accessories | $49.99 | 15 |
| Wireless Earbuds | Audio | $59.99 | 30 |
| Bluetooth Speaker | Audio | $44.99 | 20 |
| Over-Ear Headphones | Audio | $89.99 | 12 |

## Cart quantity-helper API (additive only)

All new members layer on top of the existing `List<Product> Items`; none
touch the frozen four. `ShopDatabase.cs` stays unmodified — helpers only call
its existing public `InsertProduct`/`DeleteProduct`.

- `GetQuantity(int productId) : int` — count of matching entries in `Items`.
- `GetGroupedItems() : IEnumerable<CartLine>` — new small `CartLine` record
  (Product, Quantity, LineTotal) for display; a read-only projection, not a
  restructuring of `Items`.
- `IncrementQuantity(int productId) : bool` — no-ops past ceiling 10;
  otherwise calls `AddProduct(product)` **directly, reusing the frozen
  method** rather than duplicating its logic.
- `DecrementQuantity(int productId) : bool` — no-ops at floor 1 (per the
  resolved decision: decrementing at 1 never removes); otherwise removes
  exactly one matching `Items` entry (`RemoveAt`, not `RemoveAll`) and
  resyncs `ShopDatabase` for that product ID via its existing
  `DeleteProduct`/`InsertProduct` calls, so simulated-DB row count keeps
  matching cart quantity without changing what those methods do.
- Explicit full-line removal reuses `RemoveProduct(int)` as-is — this is
  exactly its existing "remove all matching entries" behavior, which is
  correct for "remove this line entirely."

`ProductCard.razor` gains new parameters (not repurposing the frozen two):
`[Parameter] int Quantity`, `[Parameter] EventCallback<Product> OnIncrement`,
`[Parameter] EventCallback<Product> OnDecrement`, `[Parameter]
EventCallback<Product> OnRemove`. Quantity 0 → shows "Add to Cart" (existing
`OnAddToCart` path, unchanged). Quantity ≥1 → shows stepper + trash icon;
minus is disabled at quantity 1 (removal is trash-icon only, per the resolved
decision).

## Ordered commit sequence

Branch setup (no commit): `git checkout -b deploy/v2-storefront-bridge main`.

1. `feat(models): add stock and image fields to Product`
2. `feat(catalog): expand seed catalog to twelve products with stock levels`
3. `feat(cart): add quantity controls preserving the Activity 1 method contract`
4. `feat(ui): add product imagery assets` — authored SVG placeholders, one per category, `wwwroot/images/README.md` recording they are locally authored (no license question)

**— Gate 1 (STOP, explicit approval required): build clean, `GetDetails()`
byte-identical, all four frozen `Cart` methods unchanged in signature and
behavior. Report short SHAs + messages for commits 1–4. Wait before any UI
work begins.**

5. `feat(ui): rebuild ProductCard as a retail product card`
6. `refactor(pages): convert Products into the storefront page`
7. `feat(ui): add category filter and price sort to the storefront`
8. `feat(ui): add persistent cart summary to the header` (bundled with About-link removal — same `top-row` element, per the source prompt's own no-half-built-header rationale)
9. `feat(ui): rebuild Home as a landing page with sale campaign`
10. `chore(pages): remove Cart Test from the app navigation` — **revised scope**: unlink `NavMenu.razor` entry and de-promote `/cart-test` in README/docs; `CartTest.razor` and its `@page` route are kept, not deleted
11. `style: apply baseline retail storefront styling` (desktop only — no media queries; Activity 3's scope)

**— Gate 2 (STOP, explicit approval required): build clean; live-browser
walkthrough (landing → storefront routing, cards render with imagery/stock,
filters/sort behave, stepper honors 1–10, trash removes, header summary
tracks cart) — each check reported as browser-verified or code-traced,
never conflated. Report short SHAs + messages for commits 5–11. Wait before
docs.**

12. `docs: record stock deferral and quantity removal decisions`
13. `docs: update READMEs and learning-mode for the storefront changes` — includes updating `README.md`/`docs/how-to-run.md`'s Cart Test promotional copy per the revised commit 10, and `Pages/README.md`'s CartTest description
14. Open PR: `deploy/v2-storefront-bridge` → `main` via `gh`, **left unmerged**
15. `docs: archive v2 plan and completion report` → `handoff/v2/`, including this deviation in the Completion Report's "authorized deviations" section

Final verify pass (task 18, no separate commit unless a fix is needed):
build clean; markdown links re-counted and reported N/N against the 87
baseline; `git log` shows only `jdsaire` as author/committer, zero
attribution leakage; tracked `index.html` still has `<base href="/" />`;
no file claims a real database/auth; frozen `Cart` methods and
`GetDetails()` byte-identical to `main`.

## Critical files

- `src/ShopEase/Models/Product.cs`, `Models/MockProductData.cs`
- `src/ShopEase/Services/Cart.cs` (extend), `Services/ShopDatabase.cs` (read-only reference, no edits)
- `src/ShopEase/Pages/ProductCard.razor`, `Pages/Products.razor`, `Pages/Home.razor`, `Pages/CartTest.razor` (route/file untouched, only `NavMenu.razor` link removed)
- `src/ShopEase/Layout/MainLayout.razor`, new `Layout/CartSummary.razor`, `Layout/NavMenu.razor`
- `src/ShopEase/wwwroot/images/` (new, SVG placeholders + README)
- `README.md`, `docs/how-to-run.md`, `Pages/README.md`, `Layout/README.md`, `Models/README.md`, `learning-mode/01-Business-Logic-Foundations.md`
- `handoff/v2/plan.md`, `handoff/v2/completion-report.md`, `handoff/v2/README.md`, `handoff/README.md`

## Verification

- `dotnet build` from `src/ShopEase` after every commit — zero errors/warnings, checked individually not just at the end.
- Direct inspection (diff against `main`) of `Product.GetDetails()` output and all four `Cart` method bodies at Gate 1 and again at final verify.
- Live browser walkthrough at Gate 2: run `dotnet run --project src/ShopEase`, exercise landing → storefront navigation, add/increment/decrement/remove on multiple product lines, category filter, both sort directions, header summary updates, and confirm `/cart-test` still renders correctly by direct URL while absent from the nav.
- Markdown link re-count via the same script approach used in v1, reported N/N.
- `git log --format='%an %ae' deploy/v2-storefront-bridge` reviewed for zero non-`jdsaire` authors/committers and zero AI-attribution strings in any commit message.
