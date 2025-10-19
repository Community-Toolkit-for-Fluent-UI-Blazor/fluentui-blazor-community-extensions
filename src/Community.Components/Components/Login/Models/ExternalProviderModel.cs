using System.ComponentModel.DataAnnotations;
using FluentUI.Blazor.Community.Security;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a model for external authentication providers.
/// </summary>
public record ExternalProviderModel
{
    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    [Required]
    [EmailAddressRule]
    public string? Email { get; set; }
}
