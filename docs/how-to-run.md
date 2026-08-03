# How to Run

Three ways to see this app, from least to most setup.

## 1. Live GitHub Pages URL (No Setup)

**https://jdsaire.github.io/frontend_c6_ecommerce/**

Click "Products" to browse the storefront and add items to your cart.
Activity 1's test page, `/cart-test`, still exists and still runs its
add/remove/display/total sequence automatically — it's just no longer
linked from the sidebar, so reach it directly: append `/cart-test` to the
URL above. Deep links like `/products` and `/cart-test` work directly either
way — a small SPA fallback in the Pages workflow handles that on a static
host.

## 2. GitHub Codespaces

1. On the repository's GitHub page, click the green **Code** button → **Codespaces**
   → **Create codespace on main**.
2. Once the Codespace finishes loading, open its integrated terminal and
   confirm a .NET 10 SDK is available:

   ```bash
   dotnet --version
   ```

   If it isn't already installed, install one with Microsoft's install script:

   ```bash
   curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
   export PATH="$HOME/.dotnet:$PATH"
   ```

3. From the repo root, run:

   ```bash
   dotnet run --project src/ShopEase
   ```

4. Codespaces will usually prompt to open the forwarded port automatically; if
   not, open the **Ports** tab and click the forwarded address. Navigate to
   `/cart-test` or `/products` from there.

## 3. VS Code (Local Machine)

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download) if you
   don't already have one that can target `net10.0`.
2. Clone the repository and open the folder in VS Code:

   ```bash
   git clone https://github.com/jdsaire/frontend_c6_ecommerce.git
   cd frontend_c6_ecommerce
   code .
   ```

3. Open the integrated terminal (`` Ctrl+` `` / `` Cmd+` ``) and run:

   ```bash
   dotnet run --project src/ShopEase
   ```

4. Open the printed `http://localhost:...` address in a browser, then
   navigate to `/cart-test` or `/products`.

## Why Both Codespaces and VS Code Use the Same Command

Both are just a terminal with the .NET SDK available — Codespaces is a
browser-hosted one, VS Code a local one. The `dotnet run` command and the
routes you visit afterward are identical either way, which is why this project
doesn't need a `.devcontainer` configuration file to work in Codespaces: the
default environment already has everything the command needs once the SDK
check above passes.

## More Detail

- [`setup-guide.md`](setup-guide.md) — prerequisites and first-time setup in
  more depth.
- [`../src/ShopEase/README.md`](../src/ShopEase/README.md) — the same run
  instructions, scoped to the project folder itself.
- [`../learning-mode/`](../learning-mode/README.md) — what the code you're now
  looking at actually does, explained in plain language.
