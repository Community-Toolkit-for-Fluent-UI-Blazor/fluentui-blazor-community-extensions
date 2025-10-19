using System.ComponentModel.DataAnnotations;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a recovery code used for account recovery or multi-factor authentication processes.
/// </summary>
internal record RecoveryCodeModel
{
    /// <summary>
    /// Gets or sets the recovery code used to restore access to an account in case of lost credentials.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string? RecoveryCode { get; set; }
}
