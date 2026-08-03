using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ShopEase.Services;

/// <summary>
/// DataAnnotations validation attribute that every text-entry model in this app
/// (search, login, checkout) applies to its string fields. Wraps
/// <see cref="InputValidationService"/> directly rather than duplicating its rules, so
/// the "idiomatic Blazor forms" validation UI (<c>DataAnnotationsValidator</c>,
/// <c>ValidationMessage</c>) and the pure-C# service share one rule source. Client-side
/// only — see <see cref="InputValidationService"/>'s remarks on what that does and does
/// not guarantee.
/// </summary>
public class SafeTextAttribute : ValidationAttribute
{
    public SafeTextAttribute()
    {
        ErrorMessage = "This field can only contain letters, numbers, spaces, and basic punctuation.";
    }

    public override bool IsValid(object? value)
    {
        var text = value as string;
        if (string.IsNullOrEmpty(text))
        {
            // Presence is enforced separately by [Required] where needed.
            return true;
        }

        if (InputValidationService.ContainsInjectionPattern(text))
        {
            return false;
        }

        return Regex.IsMatch(text, InputValidationService.SafeTextPattern);
    }
}
