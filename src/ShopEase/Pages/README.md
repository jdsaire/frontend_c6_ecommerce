# Pages/

Every routable page in the app, plus one component that technically isn't one.

- [`Home.razor`](Home.razor) — the default landing page, kept from the
  scaffold template unchanged.
- [`NotFound.razor`](NotFound.razor) — the scaffold's default 404 fallback,
  kept unchanged.
- [`CartTest.razor`](CartTest.razor) — Activity 1's required test program,
  rendered at `/cart-test` since this app has no real console. See
  [`../README.md`](../README.md) for how to run it in Codespaces or VS Code.
- [`ProductCard.razor`](ProductCard.razor) — Activity 2's reusable product
  card component. See below for why it lives in this folder.
- [`Products.razor`](Products.razor) — the product listing page at
  `/products`, rendering multiple `ProductCard`s wired to the shared cart.

## Why ProductCard.razor Is Here, Not in a Components Folder

`ProductCard` isn't a routable page — it has no `@page` directive, and nothing
ever navigates to it directly. In most Blazor projects a reusable building
block like this would live in a separate components folder instead of
alongside actual pages. It's kept in `Pages/` here because the capstone brief
names this exact folder and file by name ("Create a new file inside the Pages
folder named ProductCard.razor"), and following the brief as written took
priority over the more common convention.
