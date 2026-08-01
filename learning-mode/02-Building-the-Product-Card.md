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
<ProductCard Product="product" OnAddToCart="HandleAddToCart" />
```

Change what `Products.razor` loops over, and every card updates automatically —
nothing about `ProductCard` itself has to change.

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
}
```

That one line is the entire bridge between "a card got clicked" and "the cart
changed." Below the cards, the same page renders `Cart.DisplayCartItems()` and
`Cart.CalculateTotal()` — the exact same `Cart` methods `CartTest.razor` used in
Activity 1. Nothing about `Cart` changed to make this work; it's the same
singleton, shared across two different pages by dependency injection, exactly as
described in [01, "Sharing One Cart Across Pages."](01-Business-Logic-Foundations.md#sharing-one-cart-across-pages-dependency-injection)
Add a product on `/products`, then check `/cart-test`'s own display logic against
the same underlying `Cart.Items`, and it's the same list both times.

## What's Next

This run stops here — Activities 3 through 5 (responsive styling, secure coding
practices, and persisted state) are separate, later deliveries against this same
repository, not part of what's covered in these two walkthrough files. When they
land, this folder gains a `03-...md` continuing exactly where this file leaves off.
