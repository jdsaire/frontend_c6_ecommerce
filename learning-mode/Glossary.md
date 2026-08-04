# Glossary

Not meant to be read start to finish — dip into it whenever a word in the
walkthroughs or the `/src` folder READMEs doesn't ring a bell. Each entry says what
the term means here, and where to see it in the project. Grows alongside the
walkthrough files — it currently covers everything introduced in
[`01-Business-Logic-Foundations.md`](01-Business-Logic-Foundations.md),
[`02-Building-the-Product-Card.md`](02-Building-the-Product-Card.md),
[`03-Responsive-UI-and-Accessibility.md`](03-Responsive-UI-and-Accessibility.md),
[`04-Input-Validation-and-Authentication.md`](04-Input-Validation-and-Authentication.md), and
[`05-State-Management-and-Persistence.md`](05-State-Management-and-Persistence.md).

## ARIA live region

A part of the page marked so assistive technology (like a screen reader)
announces changes to it automatically, without the visitor needing to move
focus there. This project uses `aria-live="polite"` on the storefront's "Show
more" count, and now also on every form's validation messages and the
checkout confirmation. Covered in [04, "The Search Box and Login Form."](04-Input-Validation-and-Authentication.md#the-search-box-and-login-form-editform-and-dataannotations)

## Authentication

Verifying who someone is before letting them do something the app restricts —
here, adding a product to the cart. This project simulates authentication
rather than using a real account system, since it's a static site with no
server to check a real password against. Covered in [04, "Authentication Without a Server."](04-Input-Validation-and-Authentication.md#authentication-without-a-server)

## AuthorizeView

A Blazor component that renders one thing when the current visitor is signed
in and another when they aren't, via its `<Authorized>` and `<NotAuthorized>`
sections. This project uses it on the login page, the header, every product
card, and the checkout entry point. Covered in [04, "Authentication Without a Server."](04-Input-Validation-and-Authentication.md#authentication-without-a-server)

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

## Color contrast

A measured ratio between a foreground and background color, not a subjective
"looks readable" judgment. WCAG requires at least 4.5:1 for normal body text
and 3:1 for larger text and meaningful UI boundaries like borders; this
project's border color was measured, found short of that at about 1.26:1, and
replaced with one measuring about 3.62:1. Covered in [03, "Accessibility: Contrast, Keyboard Use, and Focus."](03-Responsive-UI-and-Accessibility.md#accessibility-contrast-keyboard-use-and-focus)

## Combobox

An accessible UI pattern pairing a text input with a list of suggestions the visitor can
navigate with the keyboard as well as the mouse — `role="combobox"` on the input,
`role="listbox"` on the suggestion list, with ARIA attributes (`aria-expanded`,
`aria-controls`, `aria-activedescendant`) tying the two together so assistive technology
tracks which suggestion is active. The storefront's search box uses this pattern for its
catalog autocomplete. Covered in [04, "The Search Box, Rebuilt as a Combobox."](04-Input-Validation-and-Authentication.md#the-search-box-rebuilt-as-a-combobox)

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

## Cross-site scripting (XSS)

An attack where malicious script gets echoed back into a page and runs in
another visitor's browser — classically, typing `<script>...</script>` into a
field that isn't encoded before being displayed. Blazor HTML-encodes `@`
interpolated values by default, so this project was never vulnerable to it,
and has zero uses of the one thing (`MarkupString`) that would turn that
protection off. Covered in [04, "Cross-Site Scripting and Blazor's Default Encoding."](04-Input-Validation-and-Authentication.md#cross-site-scripting-and-blazors-default-encoding)

## Data sanitization

Cleaning up input that's basically fine but needs tidying — trimming stray
whitespace, for instance — as opposed to validation, which checks whether
input should be accepted at all.
[`InputValidationService.Sanitize`](../src/ShopEase/Services/InputValidationService.cs)
does only this, deliberately never rewriting or stripping characters, so
rejection always happens visibly through a validation message instead.
Covered in [04, "What Input Validation and Sanitization Mean."](04-Input-Validation-and-Authentication.md#what-input-validation-and-sanitization-mean)

## Dependency injection (DI)

The mechanism that lets a page or class ask for a shared service ("I need one of
these") without constructing it itself, and get back the one instance everyone else
is also sharing. `Cart` and `ShopDatabase` are both registered this way in
[`Program.cs`](../src/ShopEase/Program.cs). Covered in [01, "Sharing One Cart Across Pages."](01-Business-Logic-Foundations.md#sharing-one-cart-across-pages-dependency-injection)

## EditForm

Blazor's built-in component for building a validated form: it tracks an
`EditContext` for a model object, and pairs with `DataAnnotationsValidator`
and `ValidationMessage` to show inline errors next to the field they belong
to. This project's search box, login form, and checkout form are all built
this way. Covered in [04, "The Search Box and Login Form."](04-Input-Validation-and-Authentication.md#the-search-box-and-login-form-editform-and-dataannotations)

## Event callback

A component's way of reporting "this happened" back out to whatever is using it,
typed as `EventCallback<T>`. `ProductCard`'s `OnAddToCart` is one — it hands the
clicked product back to `Products.razor` without `ProductCard` ever touching `Cart`
itself. Covered in [02, "Event-Driven Development."](02-Building-the-Product-Card.md#event-driven-development-getting-the-click-back-out)

## Event-driven development

A programming approach where a user action — a button click, here — triggers code
to run in response, rather than the program deciding on its own when to act.
`ProductCard`'s "Add to Cart" button is this project's example. Covered in [02, "Event-Driven Development."](02-Building-the-Product-Card.md#event-driven-development-getting-the-click-back-out)

## Focus indicator

The visible sign — usually an outline or a glow — that shows which element on
the page currently has keyboard focus. This project extends its existing
focus-ring style to a few controls (the quantity stepper, the remove control,
the toolbar dropdowns) that weren't covered by it before, rather than
suppressing or replacing it. Covered in [03, "Accessibility: Contrast, Keyboard Use, and Focus."](03-Responsive-UI-and-Accessibility.md#accessibility-contrast-keyboard-use-and-focus)

## Input validation

Checking that user input follows expected rules — length, allowed
characters — before it's used for anything, as opposed to sanitization, which
cleans up input that's already acceptable.
[`InputValidationService`](../src/ShopEase/Services/InputValidationService.cs)
is this project's shared validation logic, applied to every text field the
app accepts. Covered in [04, "What Input Validation and Sanitization Mean."](04-Input-Validation-and-Authentication.md#what-input-validation-and-sanitization-mean)

## JS interop (JavaScript interop)

Blazor's mechanism for calling JavaScript from C# (and back), via
`IJSRuntime.InvokeAsync`/`InvokeVoidAsync` — needed for browser APIs, like
`localStorage`, that have no C# equivalent of their own.
[`cartStorage.js`](../src/ShopEase/wwwroot/js/cartStorage.js) is this
project's only JS interop: three one-line wrapper functions around
`window.localStorage`, called from
[`CartStorageService`](../src/ShopEase/Services/CartStorageService.cs).
Covered in [05, "JS Interop: How C# Talks to `localStorage`."](05-State-Management-and-Persistence.md#js-interop-how-c-talks-to-localstorage)

## Keyboard operability

Whether every interactive control can be reached with the Tab key and
activated with Enter or Space, without a mouse. True here mostly "for free"
because the app uses real `<button>` and `<select>` elements everywhere and
never overrides tab order with a custom `tabindex`. Covered in [03, "Accessibility: Contrast, Keyboard Use, and Focus."](03-Responsive-UI-and-Accessibility.md#accessibility-contrast-keyboard-use-and-focus)

## `List<T>`

.NET's growable, ordered collection type — the natural fit for "however many
products happen to be in the cart right now." Both `Cart.Items` and the simulated
database's internal storage are `List<Product>`. Covered in [01, "The Cart Class."](01-Business-Logic-Foundations.md#the-cart-class-storing-removing-displaying-totaling)

## Local storage

A browser storage API that keeps whatever's written until something explicitly
clears it — including across closing and reopening the browser entirely. This
project uses it for the shopping cart, storing a JSON array of `ProductID`
numbers under the key `shopease.cart.v1`. Contrast with session storage, below.
Covered in [05, "Local Storage vs. Session Storage."](05-State-Management-and-Persistence.md#local-storage-vs-session-storage)

## Media query

A block of CSS that only applies when some condition about the browser
window — usually its width — is true. This project's storefront grid uses
`@media (min-width: 641px)` and `@media (min-width: 1025px)` to go from one
column to two to three as the window widens. Covered in [03, "What a Media Query Is."](03-Responsive-UI-and-Accessibility.md#what-a-media-query-is)

## Method

A named action a class can perform, written as a function that lives inside it.
`GetDetails()`, `AddProduct()`, and `CalculateTotal()` are all methods. Covered in
[01, "Classes, Properties, and Methods."](01-Business-Logic-Foundations.md#classes-properties-and-methods-before-any-of-the-code)

## Mobile-first

A way of writing responsive CSS where the plain, un-wrapped rule targets the
narrowest screen, and each media query layers on a change for progressively
wider ones — rather than writing for desktop first and carving out exceptions
for small screens. Covered in [03, "What a Media Query Is."](03-Responsive-UI-and-Accessibility.md#what-a-media-query-is)

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
address. `CartTest.razor` is one, reachable at `/cart-test` — still true after
the storefront-bridge run removed it from the sidebar nav, since removing a nav
link doesn't remove the `@page` directive underneath it. Covered in [01, "Proving It Works."](01-Business-Logic-Foundations.md#proving-it-works-the-carttest-page)

## Session storage

A browser storage API that survives a page refresh but is cleared the moment
the tab or browser closes — good for protecting an in-progress form from an
accidental refresh without meaning to keep it forever. This project doesn't use
it anywhere: the cart uses local storage instead, and the checkout form isn't
draft-saved. Covered in [05, "Local Storage vs. Session Storage."](05-State-Management-and-Persistence.md#local-storage-vs-session-storage)

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

## SQL injection

An attack where untrusted input is able to change the meaning of a SQL query
— for example typing `' OR '1'='1` into a login field so the query matches
every row instead of one. This project's simulated database never builds a
query string in the first place, so there's no query for this attack to
change; the real defense in an app that does use SQL is a parameterized
query on the server, not input filtering on the client. Covered in [04, "SQL Injection With No SQL."](04-Input-Validation-and-Authentication.md#sql-injection-with-no-sql)

## Visual hierarchy

The idea that the most important thing on screen should be the easiest to
notice, and less important things should visibly recede — through size,
weight, color, and spacing rather than through explanation. On a product
card, the price is made larger and bolder than the category or stock line for
exactly this reason. Covered in [03, "Building a Clearer Visual Hierarchy."](03-Responsive-UI-and-Accessibility.md#building-a-clearer-visual-hierarchy)
