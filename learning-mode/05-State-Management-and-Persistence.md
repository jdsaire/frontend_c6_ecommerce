# 05 — State Management and Persistence

## Picking Up From 04

`04-Input-Validation-and-Authentication.md` closed with the cart and the signed-in
session both resetting the moment the browser tab refreshed. This file covers Activity
5, the capstone's last graded activity: making the cart survive that refresh — and,
this project's stronger-but-still-compliant reading of the brief, survive closing the
tab entirely — while leaving the signed-in session exactly as transient as Activity 4
left it on purpose.

## State, and Why a Refresh Used to Erase It

**State** is the data an app is currently holding onto as a user interacts with it —
what's in the cart, who's signed in, what a form currently contains. Normally state
lives only in the browser tab's memory: `Cart` is a DI singleton, so every page shares
one instance for as long as the tab stays open, but the moment that tab is closed or
reloaded, that memory is gone and a fresh `Cart` starts empty. Nothing before this run
wrote cart data anywhere more durable than that memory.

## Local Storage vs. Session Storage

Browsers give a page two built-in places to write data that outlives a single page
load: **local storage**, which keeps whatever's written until something explicitly
clears it — including across closing and reopening the browser entirely — and
**session storage**, which survives a refresh but is wiped the moment the tab or
browser closes. The shopping-cart example is the textbook case for local storage: a
user adds items, closes the tab, comes back later, and the cart is still there. Session
storage's textbook case is different — protecting an in-progress form from being lost
to an accidental refresh, without meaning to keep it forever. This app uses local
storage for the cart, and does not use session storage for anything — see
[`docs/state-management-decisions.md`](../docs/state-management-decisions.md) for the
full reasoning.

## JS Interop: How C# Talks to `localStorage`

Blazor WebAssembly compiles C# to run in the browser, but `localStorage` is a
JavaScript API with no C# equivalent of its own — reaching it means calling out to a
small piece of actual JavaScript. **JS interop** is Blazor's mechanism for that:
`IJSRuntime.InvokeAsync`/`InvokeVoidAsync` call a named JavaScript function and
(optionally) get a value back, awaited like any other async C# call.
[`wwwroot/js/cartStorage.js`](../src/ShopEase/wwwroot/js/cartStorage.js) is three
one-line functions — `getItem`/`setItem`/`removeItem` — wrapping
`window.localStorage` directly, loaded via a `<script>` tag in `index.html` after
Blazor's own boot script. It's deliberately dumb: it doesn't know what a cart is or
what key to use, it just does what it's told.

## `CartStorageService`: Where the Actual Logic Lives

[`CartStorageService`](../src/ShopEase/Services/CartStorageService.cs) is the C# side
of the JS interop call: it owns the one storage key this app writes under
(`shopease.cart.v1`), and turns a list of numbers into JSON text and back using
`System.Text.Json` — already part of .NET, so this run adds zero new NuGet packages.
Its two methods, `SaveAsync` and `LoadAsync`, are careful never to throw: a missing
key, an empty string, or a corrupted value all just come back as an empty list rather
than crashing the app. That matters because `localStorage` is outside this app's
control entirely — a visitor (or a browser extension) could edit it by hand at any
time.

What actually gets saved is not full product details — just a list of `ProductID`
numbers, with one entry repeated per unit in the cart, the same way `Cart.Items`
itself already represents quantity. On load, each ID is looked up again in
`MockProductData.GetSeedProducts()`, the single source of truth for prices and names,
rather than trusting whatever was cached in the browser. If the catalog ever changes,
a stale local-storage entry can never show a stale price.

## Why `Cart.cs` Needed Zero Changes

Every earlier activity in this project added new capability by writing to `Cart`
directly or extending it — this one didn't, on purpose. The four methods Activity 1
graded (`AddProduct`, `RemoveProduct`, `DisplayCartItems`, `CalculateTotal`) are
untouched, and so is everything added to `Cart` since. Persistence is wired entirely
from [`Layout/MainLayout.razor`](../src/ShopEase/Layout/MainLayout.razor) instead —
the one layout every routed page renders inside, so it sees every cart change no
matter which page caused it. This is the same shape this project already used for the
header's live cart count: `CartSummary.razor` subscribes to `Cart.OnChange` from
outside `Cart` itself, rather than `Cart` knowing anything about the header. `05`'s
version of that same idea:

- `OnInitialized` subscribes a save handler to `Cart.OnChange` — every time any page
  mutates the cart, that handler fires and calls `CartStorageService.SaveAsync` with
  the cart's current `ProductID`s. `Dispose` unsubscribes it, matching
  `CartSummary`'s existing pattern exactly.
- `OnAfterRenderAsync(firstRender)` — not `OnInitializedAsync`, because `IJSRuntime`
  calls need a circuit that has actually rendered — loads whatever was previously
  saved, resolves each ID against the live catalog (skipping and logging to the
  console any ID the catalog no longer has), and replays each one through the
  existing, unmodified `Cart.AddProduct`. One `Cart.NotifyChange()` and one
  `StateHasChanged()` happen after the whole batch, not once per item, so the restored
  cart appears in a single paint instead of flickering item by item.

Because the save handler never calls `StateHasChanged()` itself, subscribing it adds
no extra re-renders beyond what `CartSummary`'s own subscription already causes on
every mutation — the one exception is the single deliberate repaint right after the
initial restore, so the storefront reflects the recovered cart immediately.

## What's Next

There is no Activity 6. This is the capstone's last graded delivery — from here, the
remaining work is the final regression pass this same run also performed (products
still display correctly, the cart still updates and persists, and Activity 4's
authentication is unchanged), and preparing the peer-review submission itself, which
lives outside this repository.
