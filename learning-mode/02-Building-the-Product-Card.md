# 02 — Building the Product Card

## Picking Up From 01

`01-Business-Logic-Foundations.md` covered `Product` and `Cart` as plain C# — no
Blazor, no screen, nothing a browser was involved in yet. This file is where that
logic first meets an actual interface: `ProductCard.razor`, the button on it, and
the page that turns a list of mock products into something clickable.

## What a Blazor Component Actually Is

A **component** is a self-contained, reusable chunk of a Blazor app that bundles
its own markup (what shows up) and its own C# (what it does) into one `.razor`
file. `ProductCard` is this project's example: instead of writing near-identical
markup for a laptop, a mouse, and a mug separately, this app writes `ProductCard`
once and reuses it for every product in the catalog.

## Where ProductCard Lives, and Why That's Slightly Unusual

[`ProductCard.razor`](../src/ShopEase/Pages/ProductCard.razor) sits inside `Pages/`,
even though it isn't routable — it has no `@page` directive, and nobody ever
navigates to it directly. In most Blazor projects, a reusable building block like
this would live in a separate components folder instead. It's kept in `Pages/`
here because the capstone brief names that exact location by name, and following
the brief as written matters more than the more common convention. `Pages/README.md`
says the same thing at the point someone would actually go looking for it.

## Component Parameters: How ProductCard Receives a Product

A **component parameter** is a property a component receives from whatever is
using it, marked with `[Parameter]` in its C#. `ProductCard` declares one:

```csharp
[Parameter]
public Product Product { get; set; } = default!;
```

This is what makes one `ProductCard` definition work for every product instead of
just one. [`Products.razor`](../src/ShopEase/Pages/Products.razor) hands it a
different `Product` object each time it renders one:

```razor
<ProductCard Product="product"
             Quantity="Cart.GetQuantity(product.ProductID)"
             OnAddToCart="HandleAddToCart"
             OnIncrement="HandleIncrement"
             OnDecrement="HandleDecrement"
             OnRemove="HandleRemove" />
```

Change what `Products.razor` loops over, and every card updates automatically —
nothing about `ProductCard` itself has to change.

The four parameters beyond `Product` and `OnAddToCart` — `Quantity`,
`OnIncrement`, `OnDecrement`, `OnRemove` — are a storefront-bridge addition,
not part of Activity 2's original brief. They're new parameters layered on
top, the same pattern as `Product`/`OnAddToCart` below, so the card can show
a quantity stepper and a remove control once an item is already in the cart
without touching the two parameters Activity 2 defined.

## Event-Driven Development: Getting the Click Back Out

**Event-driven development** is a programming approach where a user action —
here, a button click — triggers code to run in response. Blazor's version of "tell
whoever's using me that something happened" is an **event callback**, typed as
`EventCallback<T>`. `ProductCard` declares one alongside its `Product` parameter:

```csharp
[Parameter]
public EventCallback<Product> OnAddToCart { get; set; }

private async Task HandleAddToCartClick()
{
    await OnAddToCart.InvokeAsync(Product);
}
```

The button's `@onclick` calls `HandleAddToCartClick`, which invokes whatever
handler the parent page supplied, passing this card's own `Product` along with it.
Notice what `ProductCard` deliberately does *not* do: it never references `Cart`,
never calls `AddProduct`, and has no idea the cart even exists. It only reports
"this was clicked, and here's which product" — what happens next is entirely the
parent page's decision. That separation is what makes `ProductCard` reusable
anywhere a product needs to be shown, cart or no cart.

## Products.razor: Where the Callback Actually Does Something

[`Products.razor`](../src/ShopEase/Pages/Products.razor) is the parent that gives
`OnAddToCart` meaning. It loops over the seed catalog from `MockProductData`,
renders one `ProductCard` per product, and supplies `HandleAddToCart` as the
callback:

```csharp
private void HandleAddToCart(Product product)
{
    Cart.AddProduct(product);
    Cart.NotifyChange();
}
```

`Cart.AddProduct(product)` is that same bridge between "a card got clicked" and
"the cart changed" — the exact same `Cart` method `CartTest.razor` used in
Activity 1, untouched. `Cart.NotifyChange()` beside it is a storefront-bridge
addition: it tells the header's persistent cart summary (a separate component,
not a child of this page) to refresh, since it has no other way to know the
cart just changed. Below the cards, the same page renders a quantity-aware
summary built from `Cart.GetGroupedItems()` and `Cart.CalculateTotal()` — the
same `Cart` state, still the same singleton shared across pages by dependency
injection, exactly as described in
[01, "Sharing One Cart Across Pages."](01-Business-Logic-Foundations.md#sharing-one-cart-across-pages-dependency-injection)
Add a product on `/products`, then check `/cart-test`'s own display logic against
the same underlying `Cart.Items`, and it's the same list both times.

## What's Next

Between Activity 2 and Activity 3, a storefront-bridge run took what this file
describes and gave it a real retail shell: a twelve-product catalog, a
quantity stepper and remove control on each card, category and price
filtering, a persistent header cart summary, and a landing page — without
changing anything this file or `01` describes about `Product`, `Cart`, or
`ProductCard`'s original two parameters. That same run left a couple of loose
ends (a dead sale-preview section on Home, a header-alignment bug, no paging
on the grid) that were cleaned up in a small v2.2 run just before Activity 3.
Activity 3 (responsive styling and accessibility) is covered next, in
[`03-Responsive-UI-and-Accessibility.md`](03-Responsive-UI-and-Accessibility.md).
Activities 4 and 5 (secure coding practices and persisted state) are still
separate, later deliveries against this same repository.
