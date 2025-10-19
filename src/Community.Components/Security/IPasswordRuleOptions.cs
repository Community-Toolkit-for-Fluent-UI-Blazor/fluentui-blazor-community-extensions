namespace FluentUI.Blazor.Community.Security;

/// <summary>
/// Defines a set of options for configuring password rule validation.
/// </summary>
public interface IPasswordRuleOptions
{
    /// <summary>
    /// Gets the minimum allowed length for the value.
    /// </summary>
    int? MinimumLength { get; }

    /// <summary>
    /// Gets the maximum allowed length for the value.
    /// </summary>
    int? MaximumLength { get;}

    /// <summary>
    /// Gets the minimum number of lowercase characters required in the value.
    /// </summary>
    int? MinimumLowercaseCharacters { get; }

    /// <summary>
    /// Gets the minimum number of uppercase characters required in a password.
    /// </summary>
    int? MinimumUppercaseCharacters { get; }

    /// <summary>
    /// Gets the minimum number of digits to display in the formatted output.
    /// </summary>
    /// <remarks>If the formatted value contains fewer digits than this property specifies, leading zeros are
    /// added to meet the minimum digit count. The value must be greater than or equal to 1.</remarks>
    int? MinimumDigits { get; }

    /// <summary>
    /// Gets the minimum number of special characters required in a password.
    /// </summary>
    int? MinimumSpecialCharacters { get; }
}
