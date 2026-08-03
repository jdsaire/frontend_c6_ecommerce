namespace ShopEase.Services;

/// <summary>
/// Pure C# validation and sanitization logic for every text input this app accepts
/// (product search, login, checkout). Has no Blazor, DI, or UI dependency, so it can
/// be exercised in isolation — by a unit test project, or by <c>Pages/SecurityTest.razor</c>,
/// which calls it directly.
///
/// CLIENT-SIDE HONESTY NOTE: this class runs entirely in the browser. It improves data
/// quality and rejects the metacharacter patterns associated with SQL injection and XSS
/// so a user gets fast, clear feedback — it is not a security boundary. A visitor with
/// developer tools can bypass every check here. In a real deployment with a server, the
/// actual defense against SQL injection is parameterized queries on that server, and the
/// actual defense against XSS is output encoding at render time (which Blazor already
/// does by default — see the no-<c>MarkupString</c> note in <c>docs/security-decisions.md</c>).
/// Nothing in this class "prevents" or "secures against" anything on its own.
/// </summary>
public static class InputValidationService
{
    /// <summary>
    /// Letters, digits, spaces, and the punctuation that legitimately appears in this
    /// app's catalog names and shipping addresses (hyphen, ampersand, apostrophe, period,
    /// comma, slash, hash). Deliberately permissive enough that real values like
    /// "27-Inch Monitor", "USB-C Hub", or "Apt #4" are never rejected — an allow-list that
    /// rejects legitimate input is a bug, not a stricter defense.
    /// </summary>
    public const string SafeTextPattern = @"^[\p{L}\p{N} .,&'\-/#]*$";

    /// <summary>
    /// Case-insensitive check for the metacharacter patterns associated with SQL
    /// injection and XSS, named in the capstone's own security-concepts material: a
    /// quote-and-OR tautology, a statement terminator, a SQL comment marker, and a
    /// script/event-handler tag. Used by the security-test page and by
    /// <see cref="SafeTextAttribute"/>; does not by itself imply an input is safe to
    /// store or execute anywhere — see the class-level remarks.
    /// </summary>
    public static bool ContainsInjectionPattern(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        string[] patterns =
        {
            "'", "--", ";", "<", ">", "<script", "onerror=", "javascript:",
            " or ", "drop table",
        };

        var lowered = input.ToLowerInvariant();
        foreach (var pattern in patterns)
        {
            if (lowered.Contains(pattern))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Trims leading/trailing whitespace and collapses internal runs of whitespace to a
    /// single space. Does not remove or alter any other character — rejection of
    /// disallowed characters happens via <see cref="SafeTextAttribute"/> so the user sees
    /// why their input was rejected, instead of having it silently rewritten.
    /// </summary>
    public static string Sanitize(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var trimmed = input.Trim();
        return System.Text.RegularExpressions.Regex.Replace(trimmed, @"\s+", " ");
    }
}
