namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for events related to two-factor authentication, including the authentication code, user preference
/// for remembering the device, and the result of the authentication attempt.
/// </summary>
/// <param name="TwoFactoryCode">The two-factor authentication code provided by the user.</param>
/// <param name="RememberMachine">A value indicating whether the user has chosen to be remembered on the device for future authentication attempts.</param>
public record TwoFactorEventArgs(string TwoFactoryCode, bool RememberMachine)
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully without any two-factor authentication
    /// failures.
    /// </summary>
    public bool IsSuccessful => FailReason == TwoFactorFailReason.None;

    /// <summary>
    /// Gets the reason for the two-factor authentication failure.
    /// </summary>
    public TwoFactorFailReason FailReason { get; init; }
}
