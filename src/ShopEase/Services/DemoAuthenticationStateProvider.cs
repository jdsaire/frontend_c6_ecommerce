using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ShopEase.Services;

/// <summary>
/// Simulates ASP.NET Identity's authentication plumbing for a static host that has no
/// server to run real ASP.NET Identity against. Extends Blazor's actual
/// AuthenticationStateProvider abstraction — the same type real ASP.NET Identity apps
/// use — so AuthorizeView, CascadingAuthenticationState, and everything downstream are
/// genuine Blazor authentication plumbing; only this class's backing store is simulated.
///
/// Sign-in state lives in this object's memory only, for the lifetime of the browser
/// tab. It is INTENTIONALLY LOST on refresh — persisting it is Activity 5's job, not
/// this run's. This is not ASP.NET Identity: no token, no cookie, no server-side
/// session, nothing a network request could intercept, because nothing here ever
/// leaves the browser. See docs/security-decisions.md for the full position.
/// </summary>
public class DemoAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    private ClaimsPrincipal _currentUser = Anonymous;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public void MarkUserAsAuthenticated(string username)
    {
        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, username) },
            authenticationType: "DemoAuthentication");
        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkUserAsLoggedOut()
    {
        _currentUser = Anonymous;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
