# ShopEase

ShopEase is the e-commerce web application built for the Microsoft Frontend
Developer Specialization capstone project. This run covers Activity 1
(business logic) and Activity 2 (Blazor components for product listings) of
the capstone's five activities.

## See It Live

**https://jdsaire.github.io/frontend_c6_ecommerce/**

Click "Products" to add items to the cart, or "Cart Test" to see Activity 1's
required add/remove/display/total sequence rendered on screen.

Want to run it yourself instead? See [`docs/how-to-run.md`](docs/how-to-run.md)
for GitHub Codespaces and VS Code instructions.

## Tech Stack

- **Blazor WebAssembly** (.NET 10) — the entire app is C# compiled to
  WebAssembly and run client-side in the browser; there is no server-side
  project.
- **Bootstrap** — the CSS toolkit the scaffold ships with, used as-is.
- **GitHub Pages** — static hosting, deployed by
  [`.github/workflows/deploy-pages.yml`](.github/workflows/deploy-pages.yml)
  on every push to `main`.

## What's Built So Far

- **Activity 1 — Business Logic**: a [`Product`](src/ShopEase/Models/Product.cs)
  class and a [`Cart`](src/ShopEase/Services/Cart.cs) class, backed by a
  simulated database (see below), proven out by a test page —
  [`CartTest.razor`](src/ShopEase/Pages/CartTest.razor) — that adds, removes,
  displays, and totals cart items.
- **Activity 2 — Components**: [`ProductCard.razor`](src/ShopEase/Pages/ProductCard.razor)
  (a reusable component with a `Product` parameter and an "Add to Cart" event)
  and [`Products.razor`](src/ShopEase/Pages/Products.razor) (a listing page
  rendering multiple cards, all wired to the same shared cart).

## Documentation

- [`docs/`](docs/README.md) — how to run this project, setup notes, and the
  capstone's grading criteria (kept for reference, not answered here).
- [`learning-mode/`](learning-mode/README.md) — a plain-language walkthrough of
  how this app was built and why, plus a glossary.
- [`handoff/`](handoff/README.md) — the plan this run was built from and its
  completion report.

Every folder under `src/ShopEase/` also has its own README explaining what
lives there.

## About the Simulated Database

Activity 1's brief asks for a local MySQL database. This app is a Blazor
WebAssembly client on static GitHub Pages hosting — there is no server process
here, so it cannot open a real MySQL connection or run any server-side
authentication. [`ShopDatabase.cs`](src/ShopEase/Services/ShopDatabase.cs) is a
clearly-labeled in-memory stand-in that mirrors the shape of that requirement
instead. No file in this repository claims a real database connection, real
authentication, or any server-side component exists.

## Out of Scope (This Run)

This run covers Activities 1 and 2 only. The remaining three activities are
separate, later deliveries against this same repository:

- **Activity 3** — UI/UX styling and responsive design.
- **Activity 4** — secure coding practices and authentication.
- **Activity 5** — persisted state management.

## Attribution

Built by [jdsaire](https://github.com/jdsaire).
