using System.ComponentModel.DataAnnotations;
using FluentUI.Blazor.Community.Components.Security;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data model for user login, including email, password, and remember-me option.
/// </summary>
internal class LoginModel
{
    /// <summary>
    /// Gets or sets the email associated with the account.
    /// </summary>
    [Required]
    [EmailAddressRule]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password associated with the user or account.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [PasswordRule]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user's login credentials should be remembered for future sessions.
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// Gets a value indicating whether both the email and password fields contain non-empty, non-whitespace values.
    /// </summary>
    public bool IsValid =>  !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
}
