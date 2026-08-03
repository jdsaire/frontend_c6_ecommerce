# ShopEase

ShopEase is the e-commerce web application built for the Microsoft Frontend
Developer Specialization capstone project. It started with Activity 1
(business logic) and Activity 2 (Blazor components for product listings) of
the capstone's five activities, had a storefront-bridge pass that gave it a
real retail shell — expanded catalog, imagery, quantity controls, filtering,
a persistent cart summary — and has now completed Activity 3: a full styling
and responsive-design pass with mobile/tablet/desktop breakpoints and an
accessibility audit. None of this changed Activity 1 or 2's graded code.

## See It Live

**https://jdsaire.github.io/frontend_c6_ecommerce/**

Click "Products" to browse the storefront: filter by category, sort by
price, reveal more with "Show more", and add items to your cart. Activity 1's original test page still
exists at `/cart-test` — it's just no longer linked in the sidebar, since
it was built for grading evidence rather than as something a shopper
browses into. See [`docs/how-to-run.md`](docs/how-to-run.md) for every way
to reach it.

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
- **Storefront bridge** (between Activities 2 and 3): a twelve-product
  catalog with stock and placeholder imagery, a retail
  [`ProductCard`](src/ShopEase/Pages/ProductCard.razor) with a quantity
  stepper and remove control, category/price filtering on
  [`Products.razor`](src/ShopEase/Pages/Products.razor), a persistent
  [`CartSummary`](src/ShopEase/Layout/CartSummary.razor) in the header, and a
  real [`Home.razor`](src/ShopEase/Pages/Home.razor) landing page. Built as
  additions on top of Activity 1/2's classes and component parameters, not
  replacements for them — see [`docs/storefront-decisions.md`](docs/storefront-decisions.md)
  for the deferred and resolved design decisions behind it.
- **v2.2 — Last changes**: removed Home's sale-campaign preview section,
  fixed the header cart summary's alignment below desktop, and added
  progressive "Show more" paging to the storefront grid. See
  [`handoff/v2.2/`](handoff/v2.2/README.md).
- **Activity 3 — Responsive UI/UX**: a dedicated
  [`site.css`](src/ShopEase/wwwroot/css/site.css) holding this project's own
  styling, a strengthened visual hierarchy on
  [`ProductCard`](src/ShopEase/Pages/ProductCard.razor) and the storefront
  grid, mobile/tablet/desktop breakpoints, and an accessibility pass (WCAG
  AA contrast, keyboard operability, visible focus indicators). See
  [`docs/activity-3-decisions.md`](docs/activity-3-decisions.md) and
  [`handoff/v3/`](handoff/v3/README.md).

## Documentation

- [`docs/`](docs/README.md) — how to run this project, setup notes, and the
  capstone's grading criteria (kept for reference, not answered here).
- [`learning-mode/`](learning-mode/README.md) — a plain-language walkthrough of
  how this app was built and why, plus a glossary.
- [`handoff/`](handoff/README.md) — the plan each run was built from and its
  completion report, one subfolder per run.

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

## Out of Scope (So Far)

Activities 1, 2, and 3 are graded and complete; the storefront bridge and
Activity 3 extended the UI without altering that graded code. Two activities
are still separate, later deliveries against this same repository:

- **Activity 4** — secure coding practices and authentication.
- **Activity 5** — persisted state management. The cart's state does not
  survive a page refresh, by design — that's this activity's job, not an
  oversight here.

Also deferred, recorded in [`docs/storefront-decisions.md`](docs/storefront-decisions.md):
out-of-stock and insufficient-stock enforcement. `Stock` is displayed on
every card but not yet checked against cart quantity.

## Attribution

Built by [jdsaire](https://github.com/jdsaire).
