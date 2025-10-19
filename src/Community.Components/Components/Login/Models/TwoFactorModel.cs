using System.ComponentModel.DataAnnotations;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data required for two-factor authentication, including the verification code and the option to
/// remember the current device.
/// </summary>
/// <remarks>This model is typically used to capture user input during a two-factor authentication challenge, such
/// as when signing in or confirming identity. The <see cref="TwoFactorCode"/> property should contain the code provided
/// by the user, while <see cref="RememberMachine"/> indicates whether the device should be trusted for future
/// authentication attempts.</remarks>
internal class TwoFactorModel
{
    /// <summary>
    /// Gets or sets the two-factor authentication code provided by the user.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    public string? TwoFactorCode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current device should be remembered for future authentication
    /// attempts.
    /// </summary>
    public bool RememberMachine { get; set; }
}
