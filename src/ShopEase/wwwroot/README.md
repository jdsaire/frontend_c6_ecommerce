# wwwroot/

Static files served to the browser exactly as-is — no C# processing touches
anything here.

- `index.html` — the single HTML page every route in this app resolves to.
  Keeps `<base href="/" />` in tracked source; the GitHub Pages CI workflow
  rewrites that only in its own build output, never here.
- `css/app.css` — the scaffold's default stylesheet, unmodified.
- `favicon.png`, `icon-192.png` — the scaffold's default icons, unmodified.
- `lib/bootstrap/` — the Bootstrap CSS/JS toolkit the scaffold ships with,
  third-party code used as-is, not written for this app.
