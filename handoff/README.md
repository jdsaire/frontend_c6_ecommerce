# handoff/

Archived plans and completion reports, one subfolder per run against this
repository.

- [`v1/`](v1/README.md) — this repository's first build: the capstone scaffold
  plus Activity 1 (business logic) and Activity 2 (Blazor components).
- [`v2/`](v2/README.md) — the storefront-bridge run: a retail shell (catalog,
  imagery, quantity controls, filtering, persistent cart summary, landing
  page) built between Activity 2 and Activity 3, on top of the same graded
  Activity 1/2 contract v1 established.
- [`v2.2/`](v2.2/README.md) — three small cleanup items closing out the
  storefront-bridge run immediately ahead of Activity 3: removing a dead
  sale section, fixing a header-alignment CSS bug, and adding progressive
  paging to the storefront grid.
- [`v3/`](v3/README.md) — Activity 3: a full styling and responsive-design
  pass, with a dedicated `site.css`, mobile/tablet/desktop breakpoints, and
  an accessibility audit. Its completion report also covers `v2.2` in full,
  since both were built from one two-part plan with a human merge gate in
  between.
- [`v4/`](v4/README.md) — Activity 4: input validation and sanitization,
  simulated authentication on Blazor's real `AuthenticationStateProvider`/
  `AuthorizeView` abstraction, cart gating on sign-in, and a security test
  pass, combined with a patch adding a checkout screen and retroactive
  Activity-3-equivalent styling/accessibility coverage for both new screens.
- [`v4.1/`](v4.1/README.md) — three surgical fixes found by using the app
  after `v4` merged: a critical navigation regression (base-relative hrefs
  restored), a search field rebuilt with a clear control and catalog
  autocomplete, and a header-spacing fix.
- [`v5/`](v5/README.md) — Activity 5, the capstone's last graded activity:
  browser-persisted cart state via `localStorage` (surviving both a refresh
  and closing the tab), wired entirely from `MainLayout.razor` so `Cart.cs`
  needed zero changes, plus a final cross-app regression and optimization
  pass.

Each run's plan and completion report live together in its own `v{N}/`
subfolder, so the reasoning behind a given set of commits stays attached to
that run rather than scattered across commit messages alone.
