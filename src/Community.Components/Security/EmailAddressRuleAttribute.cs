using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Blazor.Community.Security;

/// <summary>
/// Specifies that a property must contain a valid email address format.
/// </summary>
/// <remarks>Apply this attribute to a property to enforce email address validation when using data
/// annotation-based validation frameworks. The validation checks that the property's value conforms to a standard email
/// address pattern, but does not guarantee that the email address exists or is deliverable.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed partial class EmailAddressRuleAttribute : ValidationAttribute
{
    /// <summary>
    /// Represents the regular expression pattern used to validate email addresses according to RFC 5322 specifications.
    /// </summary>
    /// <remarks>This pattern supports most valid email address formats, including quoted strings and domain
    /// literals. It may not cover all edge cases defined by the RFC, but is suitable for general-purpose email
    /// validation in typical applications.</remarks>
    private const string emailPattern =
        @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)])";

    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var localizer = validationContext.GetService<IRuleLocalization>() ?? throw new InvalidOperationException("IRuleLocalization must be registered in the DI container.");

        var email = value as string;

        if (string.IsNullOrEmpty(email))
        {
            return new ValidationResult(localizer.ValueCannotBeNullOrEmpty);
        }

        if (EmailRegex().IsMatch(email))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(localizer.IsNotValidEmail);
    }

    [GeneratedRegex(emailPattern, RegexOptions.IgnoreCase, "fr-FR")]
    private static partial Regex EmailRegex();
}
