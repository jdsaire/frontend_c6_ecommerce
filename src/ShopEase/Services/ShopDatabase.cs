using ShopEase.Models;

namespace ShopEase.Services;

/// <summary>
/// Simulated in-memory stand-in for the MySQL "Shop" database's "Products" table.
/// This app is a Blazor WebAssembly client deployed to static GitHub Pages hosting,
/// which has no server process and therefore cannot open a real MySQL connection.
/// This class mirrors the shape of that database — insert, delete, and read of
/// Products rows — entirely in browser memory, for the lifetime of the page.
/// It is not a real database connection; no ADO.NET or Entity Framework
/// integration exists here.
/// </summary>
public class ShopDatabase
{
    // Stands in for the rows of the "Products" table. Held only in this
    // object's memory, so it resets whenever the browser tab is closed or
    // reloaded — there is no disk or server-side persistence behind it.
    private readonly List<Product> _products = new();

    // Mirrors a SQL INSERT into Products. Called by Cart.AddProduct so every
    // product added to the cart is also reflected in this simulated table.
    public void InsertProduct(Product product)
    {
        _products.Add(product);
    }

    // Mirrors a SQL DELETE FROM Products WHERE ProductID = @productId.
    // Called by Cart.RemoveProduct so removals write through here too.
    public void DeleteProduct(int productId)
    {
        _products.RemoveAll(p => p.ProductID == productId);
    }

    // Mirrors a SQL SELECT * FROM Products. Returns a read-only view so
    // callers can't mutate the simulated table except through Insert/Delete.
    public IReadOnlyList<Product> SelectAllProducts()
    {
        return _products.AsReadOnly();
    }
}
