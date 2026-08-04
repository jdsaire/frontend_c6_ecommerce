using System.Text.Json;
using Microsoft.JSInterop;

namespace ShopEase.Services;

// Persists the cart's ProductIDs to the browser's localStorage via JS
// interop, and restores them on load. Cart.cs itself is never touched here
// -- this service is called from the calling layer (MainLayout.razor), the
// same additive pattern this repo has used since v4's checkout gating.
public class CartStorageService
{
    // The single localStorage key this app writes the cart under. Owned
    // here, not in cartStorage.js, so there's one source of truth for it.
    private const string StorageKey = "shopease.cart.v1";

    private readonly IJSRuntime _jsRuntime;

    public CartStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // Serializes the given ProductIDs -- with repeats, one entry per unit,
    // mirroring how Cart.Items already represents quantity -- and writes
    // them to localStorage.
    public async Task SaveAsync(IEnumerable<int> productIds)
    {
        var json = JsonSerializer.Serialize(productIds);
        await _jsRuntime.InvokeVoidAsync("cartStorage.setItem", StorageKey, json);
    }

    // Reads and deserializes the stored ProductIDs. Never throws: a
    // missing, empty, or corrupt stored value is treated as an empty cart.
    public async Task<List<int>> LoadAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("cartStorage.getItem", StorageKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<int>();
            }

            return JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CartStorageService.LoadAsync: ignoring corrupt stored cart ({ex.Message}).");
            return new List<int>();
        }
    }
}
