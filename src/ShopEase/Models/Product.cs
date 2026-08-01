namespace ShopEase.Models;

// One item in the ShopEase catalog. This is a plain C# class with no Blazor
// or database dependency of its own — Activity 1 asks for exactly these four
// properties, so that both the Cart's business logic and (later) the
// ProductCard component can work with the same shape of data.
public class Product
{
    // Unique identifier for this product. Cart.RemoveProduct(int) looks up
    // items by this value rather than by reference, the same way a real
    // Products table would use a primary key.
    public int ProductID { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;

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
