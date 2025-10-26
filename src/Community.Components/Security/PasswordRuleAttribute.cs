using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Blazor.Community.Components.Security;

/// <summary>
/// Specifies password complexity requirements for a property and validates that the value meets the
/// defined rules.
/// </summary>
/// <remarks>Apply this attribute to string properties or parameters to enforce password policies such as minimum
/// and maximum length, and required counts of lowercase, uppercase, digit, and special characters. Validation error
/// messages are localized using an implementation of IPasswordRuleLocalization, which must be registered in the
/// dependency injection container.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class PasswordRuleAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets the minimum allowed length for the value.
    /// </summary>
    public int MinimumLength { get; set; } = 8;

    /// <summary>
    /// Gets or sets the maximum allowed length for the value.
    /// </summary>
    public int MaximumLength { get; set; } = 16;

    /// <summary>
    /// Gets or sets the minimum number of lowercase characters required in the value.
    /// </summary>
    public int MinimumLowercaseCharacters { get; set; } = 1;

    /// <summary>
    /// Gets or sets the minimum number of uppercase characters required in a password.
    /// </summary>
    public int MinimumUppercaseCharacters { get; set; } = 1;

    /// <summary>
    /// Gets or sets the minimum number of digits to display in the formatted output.
    /// </summary>
    /// <remarks>If the formatted value contains fewer digits than this property specifies, leading zeros are
    /// added to meet the minimum digit count. The value must be greater than or equal to 1.</remarks>
    public int MinimumDigits { get; set; } = 1;

    /// <summary>
    /// Gets or sets the minimum number of special characters required in a password.
    /// </summary>
    public int MinimumSpecialCharacters { get; set; } = 1;

    /// <inheritdoc/>
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var localizer = validationContext.GetService<IRuleLocalization>() ?? throw new InvalidOperationException("IRuleLocalization must be registered in the DI container.");
        var passwordOptions = validationContext.GetService<IPasswordRuleOptions>();

        var password = value as string;

        if (string.IsNullOrEmpty(password))
        {
            return new ValidationResult(localizer.ValueCannotBeNullOrEmpty);
        }

        var rule = new PasswordRule(
            passwordOptions?.MinimumLength ?? MinimumLength,
            passwordOptions?.MaximumLength ?? MaximumLength,
            passwordOptions?.MinimumLowercaseCharacters ?? MinimumLowercaseCharacters,
            passwordOptions?.MinimumUppercaseCharacters ?? MinimumUppercaseCharacters,
            passwordOptions?.MinimumDigits ?? MinimumDigits,
            passwordOptions?.MinimumSpecialCharacters ?? MinimumSpecialCharacters);

        if (rule.IsValid(password))
        {
            return ValidationResult.Success!;
        }

        var errors = rule.GetErrors(password!);

        var messages = errors.Select(error =>
        {
            return error.LocalizedError switch
            {
                "Password.MinimumLength" => localizer.MinimumLength(error.Requirement),
                "Password.MaximumLength" => localizer.MaximumLength(error.Requirement),
                "Password.LowerCase" => localizer.RequireLowercase(error.Requirement),
                "Password.UpperCase" => localizer.RequireUppercase(error.Requirement),
                "Password.Digit" => localizer.RequireDigit(error.Requirement),
                "Password.Special" => localizer.RequireSpecial(error.Requirement),
                _ => $"Invalid password rule: {error.LocalizedError}"
            };
        });

        return new ValidationResult(string.Join(Environment.NewLine, messages));
    }
}
