# Services/

The business logic that manages cart state, plus the simulated database
underneath it.

- [`Cart.cs`](Cart.cs) — `AddProduct`, `RemoveProduct`, `DisplayCartItems`, and
  `CalculateTotal`. Registered in [`Program.cs`](../Program.cs) as a DI
  singleton, so every page shares the same cart.
- [`ShopDatabase.cs`](ShopDatabase.cs) — **simulated.** This app is a Blazor
  WebAssembly client on static GitHub Pages hosting, with no server process to
  open a real MySQL connection from. This class mirrors the shape of the
  brief's MySQL `Shop`/`Products` requirement (insert, delete, read) entirely
  in browser memory. It is not a real database connection — no ADO.NET or
  Entity Framework integration exists anywhere in this project. The same
  statement lives in the XML doc comment on the class itself.

See [`../../../learning-mode/01-Business-Logic-Foundations.md`](../../../learning-mode/01-Business-Logic-Foundations.md)
for the full walkthrough, including why the database had to be simulated.
