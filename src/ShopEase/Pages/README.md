# Pages/

Every routable page in the app, plus one component that technically isn't one.

- [`Home.razor`](Home.razor) — the landing page at `/`. Rebuilt in the
  storefront-bridge run from the untouched scaffold template into a page
  that introduces ShopEase and routes into the storefront. Originally also
  carried a small sale-campaign preview section; that section was removed
  in the v2.2 run as dead weight ahead of Activity 3 (see
  [`../../../handoff/v2.2/README.md`](../../../handoff/v2.2/README.md)), so
  Home is now just the hero and its call-to-action. It does not inject
  `Cart` and does not show cart contents — Home presents and links to the
  cart, it isn't the cart.
- [`NotFound.razor`](NotFound.razor) — the scaffold's default 404 fallback,
  kept unchanged.
- [`CartTest.razor`](CartTest.razor) — Activity 1's required test program,
  rendered at `/cart-test` since this app has no real console. Kept
  unchanged, route and all, by explicit run instruction — the storefront
  bridge only removed its entry from `NavMenu.razor`, so it's no longer
  something a shopper clicks into while browsing, but it's still directly
  reachable at `/cart-test`. See [`../README.md`](../README.md) for how to
  run it in Codespaces or VS Code.
- [`ProductCard.razor`](ProductCard.razor) — the reusable product card
  component. Activity 2 gave it a `Product` parameter and an `OnAddToCart`
  event callback; the storefront-bridge run rebuilt its markup into a retail
  card (image, price, category, stock, a quantity stepper, a remove
  control) and added four new parameters (`Quantity`, `OnIncrement`,
  `OnDecrement`, `OnRemove`) alongside the original two, which are
  untouched. See below for why it lives in this folder.
- [`Products.razor`](Products.razor) — the storefront page at `/products`: a
  card grid, category filter, price sort, progressive "Show more" paging,
  and an on-page cart summary, all wired to the shared `Cart`. Originally
  Activity 2's product listing page; rebuilt in the storefront-bridge run
  with the Activity 2 assignment narration removed, then given show-more
  paging in the v2.2 run — see
  [`../../../docs/activity-3-decisions.md`](../../../docs/activity-3-decisions.md)
  for why paging beats pagination here.

## Why ProductCard.razor Is Here, Not in a Components Folder

`ProductCard` isn't a routable page — it has no `@page` directive, and nothing
ever navigates to it directly. In most Blazor projects a reusable building
block like this would live in a separate components folder instead of
alongside actual pages. It's kept in `Pages/` here because the capstone brief
names this exact folder and file by name ("Create a new file inside the Pages
folder named ProductCard.razor"), and following the brief as written took
priority over the more common convention.
