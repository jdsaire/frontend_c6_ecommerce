# Layout/

The app's shell: `MainLayout.razor` wraps every page with the sidebar and
content area, `NavMenu.razor` is that sidebar's navigation links, and
`CartSummary.razor` is the persistent cart indicator in the top-right header.

- [`MainLayout.razor`](MainLayout.razor) — originally the unmodified
  `dotnet new blazorwasm` scaffold output, apart from pointing its nav at
  this app's own pages instead of the template's sample Counter/Weather
  pages. The storefront-bridge run replaced its hardcoded "About" link
  (pointing at `learn.microsoft.com`) with `<CartSummary />` in the same
  `top-row` element.
- [`NavMenu.razor`](NavMenu.razor) — links to Home and Products. Originally
  also linked to Cart Test; the storefront-bridge run removed that entry
  (see [`../Pages/README.md`](../Pages/README.md) — the page itself and its
  route are untouched, just no longer linked here).
- [`CartSummary.razor`](CartSummary.razor) — new in the storefront-bridge
  run. Shows the cart's item count and running total, refreshing on its own
  by subscribing to `Cart.OnChange` — necessary because it lives outside any
  page's render tree, so it has no other way to know when the cart changes.

This folder isn't part of the file tree originally assumed for the Activity
1/2 run — `dotnet new blazorwasm` emits it by default, and it was kept as-is
rather than restructured, since the scaffold's actual output takes priority
over any prior assumption about the tree.
