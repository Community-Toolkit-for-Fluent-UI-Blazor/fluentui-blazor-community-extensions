namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the reason for a failure when attempting to authenticate with an external identity provider.
/// </summary>
/// <remarks>Use this enumeration to determine the cause of an unsuccessful authentication attempt with an
/// external provider, such as OAuth or OpenID Connect. The value can be used to inform the user or to implement custom
/// handling logic based on the specific failure reason.</remarks>
public enum ExternalProviderProcessingFailReason
{
    /// <summary>
    /// Indicates that there was no failure and the operation was successful.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that login information is not available.
    /// </summary>
    LoginInfoUnavailable,

    /// <summary>
    /// Indicates whether the user account is currently locked out.
    /// </summary>
    LockedOut,

    /// <summary>
    /// Indicates that the user needs to register an account.
    /// </summary>
    AskForRegister
}
