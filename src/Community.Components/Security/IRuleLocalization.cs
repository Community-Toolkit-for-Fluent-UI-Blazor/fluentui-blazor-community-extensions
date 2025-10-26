namespace FluentUI.Blazor.Community.Components.Security;

/// <summary>
/// Defines localized messages and formatting for some validation rules.
/// </summary>
/// <remarks>Implement this interface to provide culture-specific error messages or descriptions for value
/// requirements. This is typically used to display user-friendly, localized feedback when validating value in
/// applications that support multiple languages.</remarks>
public interface IRuleLocalization
{
    /// <summary>
    /// Gets the error message displayed when a value is null or empty.
    /// </summary>
    string ValueCannotBeNullOrEmpty { get; }

    /// <summary>
    /// Gets the error message to display when an email address is not valid.
    /// </summary>
    string IsNotValidEmail { get; }

    /// <summary>
    /// Returns a validation error message if the specified minimum length requirement is not met.
    /// </summary>
    /// <param name="minLength">The minimum number of characters required. Must be greater than or equal to 0.</param>
    /// <returns>A string containing the error message if the input does not meet the minimum length; otherwise, null if the
    /// requirement is satisfied.</returns>
    string MinimumLength(int minLength);

    /// <summary>
    /// Returns a string consisting of the maximum allowed number of characters, as specified by the provided length.
    /// </summary>
    /// <param name="maxLength">The maximum number of characters to include in the returned string. Must be greater than or equal to zero.</param>
    /// <returns>A string with a length equal to <paramref name="maxLength"/>. Returns an empty string if <paramref
    /// name="maxLength"/> is zero.</returns>
    string MaximumLength(int maxLength);

    /// <summary>
    /// Returns a string consisting of the specified number of randomly selected lowercase alphabetic characters.
    /// </summary>
    /// <param name="count">The number of lowercase characters to include in the returned string. Must be non-negative.</param>
    /// <returns>A string containing exactly the specified number of randomly chosen lowercase letters. Returns an empty string
    /// if count is 0.</returns>
    string RequireLowercase(int count);

    /// <summary>
    /// Returns a string containing the specified number of uppercase letters.
    /// </summary>
    /// <param name="count">The number of uppercase letters to include in the returned string. Must be non-negative.</param>
    /// <returns>A string consisting of exactly the specified number of uppercase letters. Returns an empty string if count is 0.</returns>
    string RequireUppercase(int count);

    /// <summary>
    /// Returns a string containing the specified number of random numeric digits.
    /// </summary>
    /// <param name="count">The number of digits to include in the returned string. Must be non-negative.</param>
    /// <returns>A string consisting of exactly the specified number of random digits. Returns an empty string if count is 0.</returns>
    string RequireDigit(int count);

    /// <summary>
    /// Returns a string containing a special value based on the specified count.
    /// </summary>
    /// <param name="count">The number of special items to include in the result. Must be greater than or equal to zero.</param>
    /// <returns>A string representing the special value generated from the specified count. Returns an empty string if count is
    /// zero.</returns>
    string RequireSpecial(int count);

    /// <summary>
    /// Returns a formatted message indicating that the provided passwords do not match.
    /// </summary>
    /// <param name="args">An array of strings containing the arguments to be used for formatting the message. The specific usage of each
    /// argument depends on the implementation.</param>
    /// <returns>A formatted string message indicating a password mismatch, or null if no message is generated.</returns>
    string? PasswordNotMatch(params string[] args);
}
