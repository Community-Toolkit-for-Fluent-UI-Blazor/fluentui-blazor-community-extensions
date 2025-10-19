using System.ComponentModel.DataAnnotations;
using FluentUI.Blazor.Community.Security;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data required to reset a user's password, including the email address, new password, confirmation
/// password, and reset code.
/// </summary>
/// <remarks>This model is typically used in password reset workflows to capture and validate user input when
/// resetting a forgotten password. All properties are required for a successful password reset operation.</remarks>
public record ResetPasswordModel
{
    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the user account.
    /// </summary>
    /// <remarks>The password must be between 6 and 100 characters in length. This property is typically used
    /// for user registration or authentication scenarios. The value is not stored in plain text and should be handled
    /// securely to protect user credentials.</remarks>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [PasswordRule]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confirmation password entered by the user.
    /// </summary>
    /// <remarks>This property is typically used to verify that the user has entered their intended password
    /// correctly by requiring the same value as the main password field. The value is compared to the Password property
    /// to ensure they match.</remarks>
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [PasswordRule]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the code that uniquely identifies the entity.
    /// </summary>
    [Required]
    public string Code { get; set; } = string.Empty;
}
