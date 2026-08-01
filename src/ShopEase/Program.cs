using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopEase;
using ShopEase.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Singletons so every page shares one Cart and one simulated database for
// the lifetime of the browser tab, instead of each page getting its own.
builder.Services.AddSingleton<ShopDatabase>();
builder.Services.AddSingleton<Cart>();

await builder.Build().RunAsync();
