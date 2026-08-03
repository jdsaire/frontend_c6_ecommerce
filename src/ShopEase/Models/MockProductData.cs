namespace ShopEase.Models;

// Seed catalog used by CartTest.razor (Activity 1) and Products.razor
// (storefront). There is no database or outside service behind this —
// every value here is made up for the purpose of testing the app. Products
// 1-4 are Activity 1's original four, kept unchanged so its documented
// arithmetic examples stay traceable; 5-12 are the storefront-bridge
// expansion. ImageUrl points at one shared placeholder SVG per category —
// see wwwroot/images/README.md.
public static class MockProductData
{
    public static List<Product> GetSeedProducts()
    {
        return new List<Product>
        {
            new Product { ProductID = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics", Stock = 8, ImageUrl = "images/electronics.svg" },
            new Product { ProductID = 2, Name = "Wireless Mouse", Price = 24.99m, Category = "Electronics", Stock = 40, ImageUrl = "images/electronics.svg" },
            new Product { ProductID = 3, Name = "Coffee Mug", Price = 9.99m, Category = "Home Goods", Stock = 60, ImageUrl = "images/home-goods.svg" },
            new Product { ProductID = 4, Name = "Desk Lamp", Price = 34.50m, Category = "Home Goods", Stock = 25, ImageUrl = "images/home-goods.svg" },
            new Product { ProductID = 5, Name = "External Hard Drive 1TB", Price = 64.99m, Category = "Electronics", Stock = 22, ImageUrl = "images/electronics.svg" },
            new Product { ProductID = 6, Name = "Mechanical Keyboard", Price = 79.99m, Category = "Computer Accessories", Stock = 18, ImageUrl = "images/computer-accessories.svg" },
            new Product { ProductID = 7, Name = "27-Inch Monitor", Price = 229.99m, Category = "Computer Accessories", Stock = 10, ImageUrl = "images/computer-accessories.svg" },
            new Product { ProductID = 8, Name = "USB-C Hub", Price = 39.99m, Category = "Computer Accessories", Stock = 25, ImageUrl = "images/computer-accessories.svg" },
            new Product { ProductID = 9, Name = "1080p Webcam", Price = 49.99m, Category = "Computer Accessories", Stock = 15, ImageUrl = "images/computer-accessories.svg" },
            new Product { ProductID = 10, Name = "Wireless Earbuds", Price = 59.99m, Category = "Audio", Stock = 30, ImageUrl = "images/audio.svg" },
            new Product { ProductID = 11, Name = "Bluetooth Speaker", Price = 44.99m, Category = "Audio", Stock = 20, ImageUrl = "images/audio.svg" },
            new Product { ProductID = 12, Name = "Over-Ear Headphones", Price = 89.99m, Category = "Audio", Stock = 12, ImageUrl = "images/audio.svg" },
        };
    }
}
