using ShopEase.Models;

namespace ShopEase.Services;

// Holds the shopping cart's products and totals. Registered as a DI
// singleton in Program.cs, so every page that injects Cart shares the same
// instance and sees the same items — this is how "add on one page, see it
// on another" works without any storage of its own.
public class Cart
{
    private readonly ShopDatabase _shopDatabase;

    // The cart's current items, in the order they were added.
    public List<Product> Items { get; } = new();

    public Cart(ShopDatabase shopDatabase)
    {
        _shopDatabase = shopDatabase;
    }

    // Adds a product to the cart and writes it through to the simulated
    // Shop database, per the brief's "AddProduct should save cart items to
    // a local MySQL database" requirement.
    public void AddProduct(Product product)
    {
        Items.Add(product);
        _shopDatabase.InsertProduct(product);
    }

    // Removes every item matching the given ID from both the cart and the
    // simulated database, per the brief's "RemoveProduct should remove cart
    // items from the database" requirement.
    public void RemoveProduct(int productId)
    {
        Items.RemoveAll(p => p.ProductID == productId);
        _shopDatabase.DeleteProduct(productId);
    }

    // Formats every cart item's details for display. This app has no real
    // console, so "printing" here means returning ready-to-render lines
    // that a Razor page (CartTest.razor, Products.razor) shows on screen.
    public IEnumerable<string> DisplayCartItems()
    {
        return Items.Select(p => p.GetDetails());
    }

    // Sums the price of every item currently in the cart.
    public decimal CalculateTotal()
    {
        return Items.Sum(p => p.Price);
    }
}
