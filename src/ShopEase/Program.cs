using Microsoft.AspNetCore.Components.Authorization;
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

// Simulated authentication: AddAuthorizationCore wires up the AuthorizeView
// plumbing without pulling in a server-hosted Identity package.
// DemoAuthenticationStateProvider is this app's stand-in for the real
// Identity-backed provider -- see its doc comment for what that does and
// does not mean.
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, DemoAuthenticationStateProvider>();
builder.Services.AddSingleton<DemoAccountStore>();

await builder.Build().RunAsync();
