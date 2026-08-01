# 01 — Business Logic Foundations

## What's Actually in This File

Nothing here is a Blazor concept yet. Before any component or button exists, this
app needed a way to represent "one product" and "a cart full of products" as plain
C# — code that a grader could read and understand without knowing anything about
front-end frameworks. That's what Activity 1 built, and it's what this file walks
through: `Product`, `Cart`, the simulated database underneath it, and the test page
that proves all four of `Cart`'s methods actually work.

## Classes, Properties, and Methods, Before Any of the Code

A **class** is a blueprint — it describes what a thing *has* (its properties) and
what it can *do* (its methods), without being any particular instance of that thing
yet. An **object** is one real instance built from that blueprint: not "the idea of
a product," but one specific product, with its own name and price. This app creates
plenty of objects — every seed product is one — all built from the same two
blueprints: `Product` and `Cart`.

A **property** is a named piece of data a class carries — `Name`, `Price`,
`Category`. A **method** is a named action a class can perform — `GetDetails()`,
`AddProduct()`. Grouping related properties and methods into one class instead of
passing loose variables around is the core idea behind **object-oriented
programming** (OOP): it keeps "the data" and "what you can do with the data"
together in one place, so a change to how a product is structured only has to
happen in one file.

## Meet the Building Block: the Product Class

[`Product.cs`](../src/ShopEase/Models/Product.cs) is the smaller of the two classes,
and everything else builds on it. It has exactly four properties, fixed by the
capstone brief: `ProductID`, `Name`, `Price`, `Category`. `ProductID` deserves a
second look — it exists so that later code can say "remove *this specific* product"
without needing to compare every other property, the same way a **primary key**
lets a real database table find one exact row.

The one method on `Product`, `GetDetails()`, formats those four properties into a
single line:

```
Product: Laptop | Price: $999.99 | Category: Electronics
```

That exact layout is fixed by the brief, which is why the code uses `Price:F2` —
without it, a price like `999.9` would print as `999.9` instead of `999.90`, and the
column-style layout would drift depending on what a given product happens to cost.

## Where the Product Data Actually Comes From

None of the products in this app are real — there's no supplier feed, no admin
panel, no outside service. [`MockProductData.cs`](../src/ShopEase/Models/MockProductData.cs)
is a small static class with one method, `GetSeedProducts()`, that returns the same
four made-up products every time it's called: a laptop, a mouse, a mug, and a lamp.
This is **mock data** — invented for building and testing, standing in for whatever
a real product catalog would eventually supply.

## The Cart Class: Storing, Removing, Displaying, Totaling

[`Cart.cs`](../src/ShopEase/Services/Cart.cs) is where the brief's four required
behaviors live, each as its own method:

- `AddProduct(Product product)` — appends a product to the cart's internal list.
- `RemoveProduct(int productId)` — removes whatever product matches that ID.
- `DisplayCartItems()` — returns every item's formatted detail line.
- `CalculateTotal()` — adds up the price of everything currently in the cart.

Underneath, `Cart` holds its items in a `List<Product>` — a growable, ordered
collection, the natural fit for "however many products happen to be in the cart
right now." `AddProduct` and `RemoveProduct` aren't only touching that list, though
— each one also calls through to the simulated database described below, because
the brief specifically asks for a database component in both of those methods.

`DisplayCartItems()` is worth pausing on, because "printing" means something
slightly different here than the brief's original console-app framing implies. This
app has no real console — it's a browser app, and its only way to show anything is
by rendering it on a page. So `DisplayCartItems()` returns the formatted lines
(reusing `Product.GetDetails()` for each one) and leaves it to a Razor page —
`CartTest.razor` below, and later `Products.razor` — to actually put those lines on
screen. The method still does exactly what the brief describes; it just hands its
output to a browser page instead of a terminal.

## The Database Requirement, Honestly

The brief asks for a **local MySQL database** called `Shop`, with a `Products`
table, wired into `AddProduct` and `RemoveProduct`. This app cannot do that, for a
structural reason rather than a shortcut: it's a **Blazor WebAssembly** app, meaning
it's plain client-side code that runs entirely inside a visitor's browser tab, and
it's deployed to **GitHub Pages** — a static file host with no server process behind
it at all. There is nothing here that could open a network connection to a MySQL
server, because there's no always-on backend process to open one from.

[`ShopDatabase.cs`](../src/ShopEase/Services/ShopDatabase.cs) is the honest
substitute: a class that mirrors the *shape* of the brief's database — insert,
delete, and read of `Products` rows — using an ordinary `List<Product>` held in
browser memory instead of a real connection. Its method names deliberately borrow
SQL vocabulary (`InsertProduct`, `DeleteProduct`, `SelectAllProducts`), because
that's genuinely what each one is standing in for, but the class carries a comment
at its own definition stating plainly that it isn't one — no ADO.NET, no Entity
Framework, no real connection string, anywhere in this project. `Cart.AddProduct`
and `Cart.RemoveProduct` call into it precisely where the brief asks for the
database step, so the requirement is met in spirit and structure, just not with a
real server behind it.

## Sharing One Cart Across Pages: Dependency Injection

`Cart` and `ShopDatabase` are registered once, in
[`Program.cs`](../src/ShopEase/Program.cs), as **singletons** — meaning the whole
app gets exactly one instance of each, for as long as the browser tab stays open.
This is **dependency injection** (DI): instead of a page constructing its own
`Cart`, it declares that it needs one, and the framework hands it the one shared
instance everyone else is also using. That's the mechanism that will let a later
page add a product and have it show up on a different page's cart display, without
either page needing to know the other exists.

## Proving It Works: the CartTest Page

[`CartTest.razor`](../src/ShopEase/Pages/CartTest.razor) is Activity 1's required
test program. Since this app has no real console, it's a Blazor page at
`/cart-test` instead — the closest honest equivalent, run by opening it in a
browser rather than executing a console binary. On its first load, it seeds three
products, adds all three to the cart, removes one by ID, and then renders whatever
`DisplayCartItems()` and `CalculateTotal()` actually return — not hardcoded text,
the live output of the same `Cart` methods described above.

## What's Next

Activity 1 produced business logic a grader can read without knowing anything about
Blazor. Activity 2 is where that logic first meets an actual UI component —
`ProductCard.razor`, its `Product` parameter, and the click event that connects it
to this same `Cart`. File `02-Building-the-Product-Card.md` picks up exactly there.
