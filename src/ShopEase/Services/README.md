# Services/

The business logic that manages cart state, plus the simulated database
underneath it.

- [`Cart.cs`](Cart.cs) — `AddProduct`, `RemoveProduct`, `DisplayCartItems`, and
  `CalculateTotal`, Activity 1's four required methods, unchanged since. The
  storefront-bridge run layered quantity controls on top as additive members
  — `GetQuantity`, `GetGroupedItems`, `IncrementQuantity`,
  `DecrementQuantity` — plus an `OnChange` event that independent components
  (the header's `CartSummary`) subscribe to instead of polling. Activity 4's
  checkout screen added one more additive member, `ClearCart()`, appended
  after the four frozen methods. None of it restructures `Items`;
  `IncrementQuantity` reuses `AddProduct` directly, the explicit per-line
  remove control reuses `RemoveProduct` directly, and `ClearCart` reuses the
  same simulated-database delete pattern the quantity helpers already used.
  Registered in [`Program.cs`](../Program.cs) as a DI singleton, so every
  page shares the same cart.
- [`ShopDatabase.cs`](ShopDatabase.cs) — **simulated.** This app is a Blazor
  WebAssembly client on static GitHub Pages hosting, with no server process to
  open a real MySQL connection from. This class mirrors the shape of the
  brief's MySQL `Shop`/`Products` requirement (insert, delete, read) entirely
  in browser memory. It is not a real database connection — no ADO.NET or
  Entity Framework integration exists anywhere in this project. The same
  statement lives in the XML doc comment on the class itself. Activity 4
  documents that this class never builds a query string, so there is no
  injectable SQL anywhere in it — see
  [`../../../docs/security-decisions.md`](../../../docs/security-decisions.md).
- [`InputValidationService.cs`](InputValidationService.cs) — Activity 4's
  validation and sanitization logic: an allow-list character pattern, a
  length bound, and detection of the metacharacter patterns associated with
  SQL injection and XSS. Pure C#, no Blazor or DI dependency, so it's
  unit-testable in isolation from the UI that calls it. Every text input the
  app accepts (search, login, checkout) is validated through this one class.
- [`SafeTextAttribute.cs`](SafeTextAttribute.cs) — a `ValidationAttribute`
  that wraps `InputValidationService` so `DataAnnotationsValidator`-based
  forms can reference the same rules directly, instead of duplicating them.
- [`DemoAccountStore.cs`](DemoAccountStore.cs) — **simulated.** A small,
  fixed list of obviously-fake demo accounts standing in for ASP.NET
  Identity's user store, which needs a server this static site doesn't have.
  Shown openly on the login page since these are demo values, not secrets.
- [`DemoAuthenticationStateProvider.cs`](DemoAuthenticationStateProvider.cs)
  — **simulated backing store, real Blazor plumbing.** Extends Blazor's
  actual `AuthenticationStateProvider` abstraction — the same type real
  ASP.NET Identity apps use — so `AuthorizeView` and
  `CascadingAuthenticationState` are genuine Blazor authentication
  mechanics; only this class's credential check is simulated. Sign-in state
  lives in memory only and is lost on refresh by design — see
  [`../../../docs/security-decisions.md`](../../../docs/security-decisions.md).

See [`../../../learning-mode/01-Business-Logic-Foundations.md`](../../../learning-mode/01-Business-Logic-Foundations.md)
for the cart/database walkthrough, and
[`../../../learning-mode/04-Input-Validation-and-Authentication.md`](../../../learning-mode/04-Input-Validation-and-Authentication.md)
for validation and authentication.
