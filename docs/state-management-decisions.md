# State Management Decisions

This document records the durable reasoning behind Activity 5's persistence pass —
why local storage over session storage, what gets persisted and why, and where the
new wiring lives.

## Local Storage Over Session Storage

The brief's own Step 1 ("ensure the cart persists even after the user refreshes the
page") is satisfied by either mechanism — session storage survives a refresh too. But
the course's own explanation of the distinction is explicit: local storage persists
"even after closing the browser," while session storage "clears once the browser or
tab is closed," and the shopping-cart example it gives is framed specifically as local
storage — "when a user adds items to an online shopping cart and returns later to find
them still there, that's local storage in action."

**Resolution**: `localStorage`, not `sessionStorage`. It's the stronger read of the
brief that's still fully compliant with the weaker one — a cart that survives closing
the tab also, trivially, survives a same-tab refresh.

## What Gets Persisted: IDs, Not Products

The stored value is a flat JSON array of `ProductID` integers, with repeats — one
entry per unit, exactly mirroring how `Cart.Items` itself already represents
quantity (a repeated `Product` entry per unit, not a separate count field). It is
**not** a serialized array of full `Product` objects.

**Resolution**: the catalog (`MockProductData.GetSeedProducts()`) stays the single
source of truth for name, price, and category. If a later run ever changed a price,
a browser holding a stale cached `Product` blob from before that change would show
the old price forever — storing only the ID and re-resolving against the live catalog
on every load closes that off entirely, at the cost of one lookup per restored item.

## Where the Wiring Lives: MainLayout, Not Cart.cs

Activity 1 froze `Cart.cs`'s four original methods
(`AddProduct`/`RemoveProduct`/`DisplayCartItems`/`CalculateTotal`); this run adds zero
lines to that file. Persistence is implemented as two new, separate pieces instead:

- [`wwwroot/js/cartStorage.js`](../src/ShopEase/wwwroot/js/cartStorage.js) — a thin
  `window.cartStorage` wrapper over `window.localStorage`, called through Blazor's
  `IJSRuntime` interop.
- [`Services/CartStorageService.cs`](../src/ShopEase/Services/CartStorageService.cs) —
  owns the storage key (`shopease.cart.v1`) and the `System.Text.Json`
  serialize/deserialize round-trip; `SaveAsync`/`LoadAsync` never throw, treating a
  missing, empty, or corrupt stored value as an empty cart.

Both are orchestrated from
[`Layout/MainLayout.razor`](../src/ShopEase/Layout/MainLayout.razor), the one layout
every routed page renders inside — not from any single page, and not from `Cart.cs`
itself. This follows the same calling-layer pattern this repo already established for
checkout gating (`CanModifyCart` computed and passed down from the calling page,
`Cart.cs` unaware of authentication) and for the header's live cart count
(`CartSummary.razor` subscribing to `Cart.OnChange` from outside `Cart`). `MainLayout`
subscribes a save handler to `Cart.OnChange` in `OnInitialized` (unsubscribed in
`Dispose`), and restores a previously-saved cart in `OnAfterRenderAsync(firstRender)`
— after the circuit has actually rendered, since `IJSRuntime` calls need one — by
resolving each stored `ProductID` against the live catalog and replaying it through
the existing, unmodified `Cart.AddProduct`. A stored ID no longer in the catalog is
skipped and logged to the console rather than treated as an error.

**Result**: `Services/Cart.cs`, `Models/Product.cs`, and
`Pages/ProductCard.razor` have zero diff against `main` from this run.

## What's Deliberately Out of Scope

- **Authentication persistence.** `DemoAuthenticationStateProvider`'s sign-in state
  living in memory only, lost on refresh, is Activity 4's stated, deliberate design
  (see [`security-decisions.md`](security-decisions.md)) — this run does not change
  that.
- **Checkout-form draft saving via `sessionStorage`.** A natural companion idea from
  the course material, but the graded brief's Step 1 names cart data specifically, not
  in-progress form fields — left for a possible future run, not built here.

## Summary Table

| Question | Answer |
|---|---|
| Storage mechanism | `localStorage` (survives closing the tab, not just a refresh) |
| What's stored | `ProductID` integers, with repeats — quantity by repetition, matching `Cart.Items` |
| Serialization | `System.Text.Json` (BCL, zero new package) |
| Browser access | `wwwroot/js/cartStorage.js` via `IJSRuntime` |
| New service | `CartStorageService` (`SaveAsync`/`LoadAsync`), singleton DI |
| Orchestration | `MainLayout.razor` — `Cart.OnChange` → save; `OnAfterRenderAsync` → restore |
| `Cart.cs` changes | None — zero diff against `main` |
