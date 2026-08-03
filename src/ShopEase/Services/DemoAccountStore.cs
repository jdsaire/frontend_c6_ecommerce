namespace ShopEase.Services;

/// <summary>
/// One demo sign-in credential. These values are deliberately fake and are displayed
/// openly on the login page as demo credentials — nothing here is meant to be secret.
/// </summary>
public record DemoAccount(string Username, string Password);

/// <summary>
/// Simulated in-memory stand-in for ASP.NET Identity's user store. This app is a static
/// Blazor WebAssembly site with no server process to run real ASP.NET Identity against,
/// so this class holds a small, fixed list of obviously-fake demo accounts instead. It
/// is not ASP.NET Identity: no password hashing, no persistence, no real user
/// management. See docs/security-decisions.md for why.
/// </summary>
public class DemoAccountStore
{
    private static readonly IReadOnlyList<DemoAccount> Accounts = new List<DemoAccount>
    {
        new("demo_shopper1", "Demo#2026Test1"),
        new("demo_shopper2", "Demo#2026Test2"),
    };

    public IReadOnlyList<DemoAccount> DemoAccounts => Accounts;

    public bool Validate(string username, string password)
    {
        return Accounts.Any(a =>
            string.Equals(a.Username, username, StringComparison.Ordinal) &&
            string.Equals(a.Password, password, StringComparison.Ordinal));
    }
}
