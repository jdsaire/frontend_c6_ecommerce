# wwwroot/css/

- `app.css` — the Blazor project template's own base styles (loading
  spinner, error boundary, form-floating placeholders, and similar
  scaffold chrome). Holds none of this project's own storefront styling.
- `site.css` — this project's own storefront, landing, product-card, and
  cart-summary styling, added in Activity 3 by moving it out of `app.css`.
  See [`../../../../docs/activity-3-decisions.md`](../../../../docs/activity-3-decisions.md)
  for why the split exists and why it's organized this way rather than one
  combined file.

Both are linked from [`../index.html`](../index.html), `app.css` before
`site.css`, alongside the third-party `lib/bootstrap/` stylesheet.
