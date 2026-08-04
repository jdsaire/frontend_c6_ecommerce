# wwwroot/js/

Browser-side JavaScript this app calls via Blazor's `IJSRuntime` interop —
code that has to run in the browser itself rather than as compiled C#.

- [`cartStorage.js`](cartStorage.js) — a thin wrapper over
  `window.localStorage`, exposing `getItem`/`setItem`/`removeItem` under
  `window.cartStorage`. Called from
  [`CartStorageService.cs`](../../Services/CartStorageService.cs), which owns
  the actual storage key. Referenced by a `<script>` tag in
  [`index.html`](../index.html), after the Blazor framework's own boot
  script.
