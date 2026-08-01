# src/ShopEase/

The Blazor WebAssembly project itself — everything under this folder is what
actually builds and runs as the app GitHub Pages serves.

## Structure

- [`Models/`](Models/README.md) — `Product` and the mock seed catalog.
- [`Services/`](Services/README.md) — `Cart` and the simulated database.
- [`Pages/`](Pages/README.md) — routable pages and the `ProductCard` component.
- [`Layout/`](Layout/README.md) — the app shell and navigation menu.
- [`Properties/`](Properties/README.md) — local launch configuration.
- [`wwwroot/`](wwwroot/README.md) — static assets served as-is.

## Running This Project (Activity 1's Test Program, Included)

Activity 1 asks for a "test program" — since this app has no real console,
that's [`CartTest.razor`](Pages/CartTest.razor) at the `/cart-test` route,
viewed in a browser like every other page here. Either of these gets it running:

### GitHub Codespaces

1. Open this repository in a Codespace (the green "Code" button → Codespaces).
2. In the integrated terminal, confirm a .NET 10 SDK is available:

   ```bash
   dotnet --version
   ```

   If it isn't already installed, install one with Microsoft's install script
   before continuing:

   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
   ```

3. From the repo root, run:

   ```bash
   dotnet run --project src/ShopEase
   ```

4. Open the forwarded port Codespaces offers (it usually prompts
   automatically), then navigate to `/cart-test` or `/products`.

### VS Code (Local)

1. Clone the repository and open the folder in VS Code.
2. Open the integrated terminal and run the same command:

   ```bash
   dotnet run --project src/ShopEase
   ```

3. Open the printed `http://localhost:...` URL in a browser, then navigate to
   `/cart-test` or `/products`.

Both paths run against the tracked `wwwroot/index.html`, which keeps
`<base href="/" />` — the `/frontend_c6_ecommerce/` subpath rewrite only
happens inside the GitHub Pages CI workflow, never in this source.
