using System.ComponentModel.DataAnnotations;
using FluentUI.Blazor.Community.Components.Security;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data required to initiate a password reset request.
/// </summary>
internal class ForgotPasswordModel
{
    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    [Required]
    [EmailAddressRule]
    public string? Email { get; set; }
}
