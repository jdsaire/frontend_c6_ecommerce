namespace ShopEase.Models;

// One item in the ShopEase catalog. This is a plain C# class with no Blazor
// or database dependency of its own. Activity 1 fixes ProductID, Name,
// Price, and Category, plus GetDetails()'s exact output — Stock and
// ImageUrl below are storefront additions layered on top, added without
// touching any of the original four.
public class Product
{
    // Unique identifier for this product. Cart.RemoveProduct(int) looks up
    // items by this value rather than by reference, the same way a real
    // Products table would use a primary key.
    public int ProductID { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;

    // Units currently on hand. Displayed on the storefront card; not yet
    // enforced against cart quantity — see docs/ for the deferral.
    public int Stock { get; set; }

    // Relative path (under wwwroot/) to this product's placeholder image.
    // One locally-authored SVG per category — see wwwroot/images/README.md.
    public string ImageUrl { get; set; } = string.Empty;

    // Formats this product's details on one line, in the exact layout the
    // capstone brief fixes:
    //   Product: Laptop | Price: $999.99 | Category: Electronics
    // "F2" forces exactly two decimal places so a price like 999.9 still
    // prints as "999.90" instead of "999.9".
    public string GetDetails()
    {
        return $"Product: {Name} | Price: ${Price:F2} | Category: {Category}";
    }
}
