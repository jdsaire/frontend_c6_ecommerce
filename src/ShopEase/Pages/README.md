# Pages/

Every routable page in the app, plus one component that technically isn't one.

- [`Home.razor`](Home.razor) — the landing page at `/`. Rebuilt in the
  storefront-bridge run from the untouched scaffold template into a page
  that introduces ShopEase and routes into the storefront, with a small
  sale-campaign preview. It does not inject `Cart` and does not show cart
  contents — Home presents and links to the cart, it isn't the cart.
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
  card grid, category filter, price sort, and an on-page cart summary, all
  wired to the shared `Cart`. Originally Activity 2's product listing page;
  rebuilt in the storefront-bridge run, with the Activity 2 assignment
  narration removed.

## Why ProductCard.razor Is Here, Not in a Components Folder

`ProductCard` isn't a routable page — it has no `@page` directive, and nothing
ever navigates to it directly. In most Blazor projects a reusable building
block like this would live in a separate components folder instead of
alongside actual pages. It's kept in `Pages/` here because the capstone brief
names this exact folder and file by name ("Create a new file inside the Pages
folder named ProductCard.razor"), and following the brief as written took
priority over the more common convention.
