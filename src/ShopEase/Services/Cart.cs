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

    // Per-line quantity bounds for the storefront's stepper control.
    public const int MinQuantity = 1;
    public const int MaxQuantity = 10;

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

    // ---- Storefront quantity controls -------------------------------
    // Additive only: everything below reads or adjusts Items without
    // changing its shape, so AddProduct, RemoveProduct, DisplayCartItems,
    // and CalculateTotal above keep working exactly as before.

    // How many units of this product are currently in the cart.
    public int GetQuantity(int productId)
    {
        return Items.Count(p => p.ProductID == productId);
    }

    // One line per distinct product, for a grouped storefront display —
    // a read-only projection over Items, not a replacement for it.
    public IEnumerable<CartLine> GetGroupedItems()
    {
        return Items
            .GroupBy(p => p.ProductID)
            .Select(g => new CartLine(g.First(), g.Count()));
    }

    // Adds one more unit of a product already in the cart, up to
    // MaxQuantity. Reuses AddProduct directly rather than duplicating its
    // Items/database write-through logic.
    public bool IncrementQuantity(int productId)
    {
        var product = Items.FirstOrDefault(p => p.ProductID == productId);
        if (product is null || GetQuantity(productId) >= MaxQuantity)
        {
            return false;
        }

        AddProduct(product);
        return true;
    }

    // Removes exactly one unit of a product, down to MinQuantity — never
    // the last one. Full-line removal is RemoveProduct(int) above,
    // triggered only by its own explicit control: the resolved decision is
    // that decrementing at the floor must not silently delete the line.
    public bool DecrementQuantity(int productId)
    {
        if (GetQuantity(productId) <= MinQuantity)
        {
            return false;
        }

        var index = Items.FindLastIndex(p => p.ProductID == productId);
        Items.RemoveAt(index);

        // Resync the simulated database's row count for this product with
        // Items, without changing what Insert/DeleteProduct do: a bulk
        // delete-then-reinsert, since ShopDatabase only exposes bulk
        // insert-one/delete-all-matching operations, not a single-row delete.
        _shopDatabase.DeleteProduct(productId);
        foreach (var remaining in Items.Where(p => p.ProductID == productId))
        {
            _shopDatabase.InsertProduct(remaining);
        }

        return true;
    }
}

// One grouped cart line for storefront display: a product and how many
// units of it are currently in the cart.
public record CartLine(Product Product, int Quantity)
{
    public decimal LineTotal => Product.Price * Quantity;
}
