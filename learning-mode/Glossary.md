# Glossary

Not meant to be read start to finish — dip into it whenever a word in the
walkthroughs or the `/src` folder READMEs doesn't ring a bell. Each entry says what
the term means here, and where to see it in the project. Grows alongside the
walkthrough files — it currently covers everything introduced in
[`01-Business-Logic-Foundations.md`](01-Business-Logic-Foundations.md) and
[`02-Building-the-Product-Card.md`](02-Building-the-Product-Card.md).

## Blazor WebAssembly

Microsoft's framework for writing a browser app in C# instead of JavaScript, by
compiling that C# into a compact format the browser can run directly. This whole
app is one of these — including [`CartTest.razor`](../src/ShopEase/Pages/CartTest.razor),
which is a Blazor page even though the code it demonstrates (`Product`, `Cart`) is
plain C# with no Blazor of its own. Covered in [01, "Proving It Works."](01-Business-Logic-Foundations.md#proving-it-works-the-carttest-page)

## Class

A blueprint describing what a thing has (its properties) and what it can do (its
methods), without being any particular instance of that thing yet. `Product` and
`Cart` are this project's two business-logic classes. Covered in [01, "Classes, Properties, and Methods."](01-Business-Logic-Foundations.md#classes-properties-and-methods-before-any-of-the-code)

## Component

A self-contained, reusable chunk of a Blazor app that bundles its own markup (what
shows up) and its own C# (what it does) into one `.razor` file.
[`ProductCard.razor`](../src/ShopEase/Pages/ProductCard.razor) is this project's
example — written once, reused for every product in the catalog. Covered in [02, "What a Blazor Component Actually Is."](02-Building-the-Product-Card.md#what-a-blazor-component-actually-is)

## Component parameter

A property a component receives from whatever is using it, marked with
`[Parameter]` in its C#. `ProductCard`'s `Product` parameter is what lets one
component definition display a different product every time it's used. Covered in
[02, "Component Parameters."](02-Building-the-Product-Card.md#component-parameters-how-productcard-receives-a-product)

## Constructor

The method a class uses to set itself up when a new object is created from it.
`Cart`'s constructor takes a `ShopDatabase` as a parameter, which is how it gets a
reference to the shared simulated database without building one itself. Covered in
[01, "Sharing One Cart Across Pages."](01-Business-Logic-Foundations.md#sharing-one-cart-across-pages-dependency-injection)

## Dependency injection (DI)

The mechanism that lets a page or class ask for a shared service ("I need one of
these") without constructing it itself, and get back the one instance everyone else
is also sharing. `Cart` and `ShopDatabase` are both registered this way in
[`Program.cs`](../src/ShopEase/Program.cs). Covered in [01, "Sharing One Cart Across Pages."](01-Business-Logic-Foundations.md#sharing-one-cart-across-pages-dependency-injection)

## Event callback

A component's way of reporting "this happened" back out to whatever is using it,
typed as `EventCallback<T>`. `ProductCard`'s `OnAddToCart` is one — it hands the
clicked product back to `Products.razor` without `ProductCard` ever touching `Cart`
itself. Covered in [02, "Event-Driven Development."](02-Building-the-Product-Card.md#event-driven-development-getting-the-click-back-out)

## Event-driven development

A programming approach where a user action — a button click, here — triggers code
to run in response, rather than the program deciding on its own when to act.
`ProductCard`'s "Add to Cart" button is this project's example. Covered in [02, "Event-Driven Development."](02-Building-the-Product-Card.md#event-driven-development-getting-the-click-back-out)

## `List<T>`

.NET's growable, ordered collection type — the natural fit for "however many
products happen to be in the cart right now." Both `Cart.Items` and the simulated
database's internal storage are `List<Product>`. Covered in [01, "The Cart Class."](01-Business-Logic-Foundations.md#the-cart-class-storing-removing-displaying-totaling)

## Method

A named action a class can perform, written as a function that lives inside it.
`GetDetails()`, `AddProduct()`, and `CalculateTotal()` are all methods. Covered in
[01, "Classes, Properties, and Methods."](01-Business-Logic-Foundations.md#classes-properties-and-methods-before-any-of-the-code)

## Mock data

Data invented for building and testing an app, standing in for whatever a real
system would eventually supply. Every product in this app comes from
[`MockProductData.cs`](../src/ShopEase/Models/MockProductData.cs), not a real
catalog or outside service. Covered in [01, "Where the Product Data Actually Comes From."](01-Business-Logic-Foundations.md#where-the-product-data-actually-comes-from)

## Namespace

A named grouping that keeps related classes organized and prevents naming
collisions — `ShopEase.Models` holds `Product` and `MockProductData`;
`ShopEase.Services` holds `Cart` and `ShopDatabase`.

## Object

One real instance built from a class's blueprint — not "the idea of a product,"
but one specific product with its own name and price. Every item `MockProductData`
returns is an object built from the `Product` class. Covered in [01, "Classes, Properties, and Methods."](01-Business-Logic-Foundations.md#classes-properties-and-methods-before-any-of-the-code)

## Object-oriented programming (OOP)

An approach to structuring code that keeps related data (properties) and behavior
(methods) grouped together in one class, instead of passing loose variables
between separate functions. `Product` and `Cart` are both examples. Covered in [01, "Classes, Properties, and Methods."](01-Business-Logic-Foundations.md#classes-properties-and-methods-before-any-of-the-code)

## Primary key

A unique identifier for one record, letting code find or remove that exact record
without comparing every other field. `Product.ProductID` plays this role — it's
what `Cart.RemoveProduct(int productId)` matches against. Covered in [01, "Meet the Building Block."](01-Business-Logic-Foundations.md#meet-the-building-block-the-product-class)

## Property

A named piece of data a class carries. `Name`, `Price`, and `Category` are
properties of `Product`. Covered in [01, "Classes, Properties, and Methods."](01-Business-Logic-Foundations.md#classes-properties-and-methods-before-any-of-the-code)

## Razor page

A `.razor` file marked with an `@page` directive at the top, giving it its own web
address. `CartTest.razor` is one, reachable at `/cart-test`. Covered in [01, "Proving It Works."](01-Business-Logic-Foundations.md#proving-it-works-the-carttest-page)

## Simulated database

An in-memory stand-in for a real database, used here because this app is a static,
server-less Blazor WebAssembly site with nowhere to run a real MySQL connection
from. [`ShopDatabase.cs`](../src/ShopEase/Services/ShopDatabase.cs) mirrors the
shape of the brief's MySQL `Shop`/`Products` requirement — insert, delete, read —
entirely in browser memory, and says so directly in its own doc comment. Covered
in [01, "The Database Requirement, Honestly."](01-Business-Logic-Foundations.md#the-database-requirement-honestly)

## Singleton

A dependency-injection lifetime meaning exactly one instance is created and shared
by the whole app for as long as the browser tab stays open. `Cart` and
`ShopDatabase` are both registered as singletons, which is what lets cart state
stay consistent across different pages. Covered in [01, "Sharing One Cart Across Pages."](01-Business-Logic-Foundations.md#sharing-one-cart-across-pages-dependency-injection)
