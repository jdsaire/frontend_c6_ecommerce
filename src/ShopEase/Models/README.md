# Models/

Plain C# data classes — no Blazor, no database, no dependency on anything else
in this project.

- [`Product.cs`](Product.cs) — one product's `ProductID`, `Name`, `Price`, and
  `Category`, plus `GetDetails()`, which formats those four fields in the exact
  layout the capstone brief fixes. The storefront-bridge run added `Stock`
  (int) and `ImageUrl` (a path under `wwwroot/`, see
  [`../wwwroot/images/README.md`](../wwwroot/images/README.md)) alongside
  those four, without changing any of them or `GetDetails()`'s output.
- [`MockProductData.cs`](MockProductData.cs) — a static seed catalog, used by
  both `CartTest.razor` and `Products.razor`. Activity 1 originally seeded
  four made-up products; the storefront-bridge run expanded that to twelve
  across four categories (Electronics, Home Goods, Computer Accessories,
  Audio), keeping the original four unchanged so Activity 1's documented
  arithmetic examples stay traceable. There is no real product catalog or
  outside data source behind any of it.

See [`../../../learning-mode/01-Business-Logic-Foundations.md`](../../../learning-mode/01-Business-Logic-Foundations.md)
for the full plain-language walkthrough of how `Product` fits into the rest of
the app.
