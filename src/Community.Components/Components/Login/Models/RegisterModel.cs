using System.ComponentModel.DataAnnotations;
using FluentUI.Blazor.Community.Components.Security;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data required to register a new user, including display name, email address, and password
/// information.
/// </summary>
/// <remarks>This record is typically used as a data transfer object for user registration scenarios, such as
/// account creation forms or API endpoints. All properties are required and should be populated with valid, non-empty
/// values to ensure successful registration. The password and confirmation password must match to validate the
/// registration request.</remarks>
public record RegisterModel
{
    /// <summary>
    /// Gets or sets the display name associated with the object.
    /// </summary>
    /// <remarks>This property is required and should be set to a non-empty value to ensure proper
    /// identification in user interfaces or logs.</remarks>
    [Required]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    [Required]
    [EmailAddressRule]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password used for authentication.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [PasswordRule]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the confirmation password entered by the user.
    /// </summary>
    /// <remarks>This property is typically used to verify that the user has correctly re-entered their
    /// password during registration or password change operations. The value should match the original password to
    /// ensure successful confirmation.</remarks>
    [Required]
    [DataType(DataType.Password)]
    [PasswordRule]
    [PasswordCompare(nameof(Password))]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// Determines whether the current object contains valid, non-empty values for all required fields and that the
    /// password fields match.
    /// </summary>
    /// <returns>true if all required fields are non-empty and the password matches the confirmation; otherwise, false.</returns>
    public bool IsValid() => !string.IsNullOrWhiteSpace(DisplayName)
        && !string.IsNullOrWhiteSpace(Email)
        && !string.IsNullOrWhiteSpace(Password)
        && !string.IsNullOrWhiteSpace(ConfirmPassword)
        && string.Equals(Password, ConfirmPassword);
}
