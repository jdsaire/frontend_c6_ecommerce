# Models/

Plain C# data classes — no Blazor, no database, no dependency on anything else
in this project.

- [`Product.cs`](Product.cs) — one product's `ProductID`, `Name`, `Price`, and
  `Category`, plus `GetDetails()`, which formats those four fields in the exact
  layout the capstone brief fixes.
- [`MockProductData.cs`](MockProductData.cs) — a static seed catalog of four
  made-up products, used by both `CartTest.razor` and `Products.razor`. There
  is no real product catalog or outside data source behind it.

See [`../../../learning-mode/01-Business-Logic-Foundations.md`](../../../learning-mode/01-Business-Logic-Foundations.md)
for the full plain-language walkthrough of how `Product` fits into the rest of
the app.
