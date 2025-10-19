namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the reason for a two-factor authentication failure.
/// </summary>
/// <remarks>Use this enumeration to determine the specific cause when a two-factor authentication attempt does
/// not succeed. This can help inform user feedback or trigger additional security measures.</remarks>
public enum TwoFactorFailReason
{
    /// <summary>
    /// Indicates that there is no specific reason for the failure.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the provided two-factor authentication code is invalid.
    /// </summary>
    InvalidCode,

    /// <summary>
    /// Indicates whether the account is currently locked out due to multiple failed sign-in attempts.
    /// </summary>
    LockedOut
}
