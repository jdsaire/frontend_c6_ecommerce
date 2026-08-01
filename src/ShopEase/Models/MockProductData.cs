namespace ShopEase.Models;

// Seed catalog used by CartTest.razor (Activity 1) and Products.razor
// (Activity 2). There is no database or outside service behind this —
// every value here is made up for the purpose of testing the app.
public static class MockProductData
{
    public static List<Product> GetSeedProducts()
    {
        return new List<Product>
        {
            new Product { ProductID = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics" },
            new Product { ProductID = 2, Name = "Wireless Mouse", Price = 24.99m, Category = "Electronics" },
            new Product { ProductID = 3, Name = "Coffee Mug", Price = 9.99m, Category = "Home Goods" },
            new Product { ProductID = 4, Name = "Desk Lamp", Price = 34.50m, Category = "Home Goods" },
        };
    }
}
