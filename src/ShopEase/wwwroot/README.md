# wwwroot/

Static files served to the browser exactly as-is — no C# processing touches
anything here.

- `index.html` — the single HTML page every route in this app resolves to.
  Keeps `<base href="/" />` in tracked source; the GitHub Pages CI workflow
  rewrites that only in its own build output, never here.
- `css/app.css` — the scaffold's own base styles (loading spinner, error
  boundary, form-floating placeholders, and similar template chrome). No
  longer unmodified as of the storefront-bridge run, but still limited to
  styles the scaffold itself owns — this project's own styling lives in
  `css/site.css` instead.
- `css/site.css` — this project's own storefront, landing, product-card, and
  cart-summary styling, added in Activity 3. See
  [`../../../docs/activity-3-decisions.md`](../../../docs/activity-3-decisions.md)
  for why it's a separate file from `app.css`.
- `favicon.png`, `icon-192.png` — the scaffold's default icons, unmodified.
- `lib/bootstrap/` — the Bootstrap CSS/JS toolkit the scaffold ships with,
  third-party code used as-is, not written for this app.
