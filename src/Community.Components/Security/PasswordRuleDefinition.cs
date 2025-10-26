namespace FluentUI.Blazor.Community.Components.Security;

/// <summary>
/// Defines a set of rules for password composition, including length and character type requirements.
/// </summary>
/// <remarks>Use this structure to specify constraints for password validation, such as minimum and maximum
/// length, and required counts of lowercase, uppercase, digit, and special characters. The rules ensure that any
/// password validated against this definition meets all specified criteria. If the sum of minimum required character
/// types exceeds the minimum length, an exception will be thrown during initialization.</remarks>
public readonly struct PasswordRuleDefinition
{
    /// <summary>
    /// Initializes a new instance of the PasswordRuleDefinition class with the specified password complexity
    /// requirements.
    /// </summary>
    /// <remarks>If any of the specified requirements are invalid, an exception may be thrown during
    /// initialization. Use this constructor to enforce custom password policies for validation purposes.</remarks>
    /// <param name="minimumLength">The minimum number of characters required for a valid password. Must be greater than zero and less than or equal
    /// to <paramref name="maximumLength"/>.</param>
    /// <param name="maximumLength">The maximum number of characters allowed for a valid password. Must be greater than or equal to <paramref
    /// name="minimumLength"/>.</param>
    /// <param name="minimumLowercaseCharacters">The minimum number of lowercase alphabetic characters required in the password. Must be zero or greater.</param>
    /// <param name="minimumUppercaseCharacters">The minimum number of uppercase alphabetic characters required in the password. Must be zero or greater.</param>
    /// <param name="minimumDigits">The minimum number of numeric digits required in the password. Must be zero or greater.</param>
    /// <param name="minimumSpecialCharacters">The minimum number of special (non-alphanumeric) characters required in the password. Must be zero or greater.</param>
    public PasswordRuleDefinition(
        int minimumLength = 1,
        int maximumLength = 8,
        int minimumLowercaseCharacters = 1,
        int minimumUppercaseCharacters = 1,
        int minimumDigits = 1,
        int minimumSpecialCharacters = 1)
    {
        MinimumLength = minimumLength;
        MaximumLength = maximumLength;
        MinimumLowercaseCharacters = minimumLowercaseCharacters;
        MinimumUppercaseCharacters = minimumUppercaseCharacters;
        MinimumDigits = minimumDigits;
        MinimumSpecialCharacters = minimumSpecialCharacters;

        CheckValidation();
    }

    /// <summary>
    /// Gets or sets the minimum length allowed for the password.
    /// </summary>
    public int MinimumLength { get; init; }

    /// <summary>
    /// Gets or sets the maximum length allowed for the password.
    /// </summary>
    public int MaximumLength { get; init; }

    /// <summary>
    /// Gets or sets the minimum number of lowercase character allowed for the password.
    /// </summary>
    public int MinimumLowercaseCharacters { get; init; }

    /// <summary>
    /// Gets or sets the minimum number of uppercase character allowed for the password.
    /// </summary>
    public int MinimumUppercaseCharacters { get; init; }

    /// <summary>
    /// Gets or sets the minimum number of digit allowed for the password.
    /// </summary>
    public int MinimumDigits { get; init; }

    /// <summary>
    /// Gets or sets the minimum number of special character allowed for the password.
    /// </summary>
    public int MinimumSpecialCharacters { get; init; }

    /// <summary>
    /// Validates that the minimum password length is sufficient to accommodate the required number of digits,
    /// uppercase, lowercase, and special characters.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the minimum password length is less than the total number of required digits, uppercase, lowercase,
    /// and special characters.</exception>
    private void CheckValidation()
    {
        var requiredLength = MinimumDigits + MinimumUppercaseCharacters + MinimumLowercaseCharacters + MinimumSpecialCharacters;

        if (MinimumLength < requiredLength)
        {
            throw new ArgumentOutOfRangeException($"The password must have a minimum length of {requiredLength} characters");
        }
    }
}
