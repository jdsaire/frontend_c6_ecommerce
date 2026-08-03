# docs/

Operational documentation for running, setting up, and grading this project.

- [`how-to-run.md`](how-to-run.md) — every way to open the app: the live
  GitHub Pages URL, GitHub Codespaces, or VS Code.
- [`setup-guide.md`](setup-guide.md) — local prerequisites and first-time setup.
- [`grading-criteria.md`](grading-criteria.md) — the capstone's 18-point
  peer-review breakdown and submission questions, kept here for reference —
  not answered in this repository.
- [`storefront-decisions.md`](storefront-decisions.md) — why stock is
  displayed but not enforced, and why cart-line removal is a separate
  explicit control rather than decrementing quantity to zero.
- [`activity-3-decisions.md`](activity-3-decisions.md) — why `site.css`
  exists alongside `app.css`, the chosen responsive breakpoints, the
  show-more-over-pagination tradeoff, and the `/cart-test` scope boundary.
- [`security-decisions.md`](security-decisions.md) — Activity 4: the
  client-side honesty invariant, why simulated authentication uses Blazor's
  real `AuthenticationStateProvider`/`AuthorizeView` abstraction instead of
  ASP.NET Identity, the no-SQL/parameterized-query position, the
  no-`MarkupString` XSS position, the no-payment-fields checkout decision,
  and the retroactive styling/accessibility coverage for the login and
  checkout screens.
- [`security-testing.md`](security-testing.md) — the five benign canonical
  probe strings `/security-test` runs, expected behavior per case, and what
  a client-side PASS does and does not prove.

Looking for a plain-language explanation of *how* the app works, rather than
how to run it? See [`../learning-mode/`](../learning-mode/README.md) instead.
