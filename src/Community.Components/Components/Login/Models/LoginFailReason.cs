namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the reason for a failed login attempt.
/// </summary>
public enum LoginFailReason
{
    /// <summary>
    /// Indicates that the login attempt was successful and there was no failure.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the login attempt failed due to invalid credentials.
    /// </summary>
    InvalidCredentials,

    /// <summary>
    /// Indicates that the login attempt failed because the user account is locked.
    /// </summary>
    AccountLocked,

    /// <summary>
    /// Indicates that the login attempt failed because the user account is disabled.
    /// </summary>
    AccountDisabled,

    /// <summary>
    /// Indicates that the login attempt failed because two-factor authentication is required.
    /// </summary>
    RequiredTwoFactor,

    /// <summary>
    /// Indicates that the login attempt failed due to an unknown error.
    /// </summary>
    UnknownError,

    /// <summary>
    /// Indicates that the user has not completed the required confirmation process.
    /// </summary>
    UserNotConfirmed,

    /// <summary>
    /// Indicates that the login attempt failed because the user is not allowed to sign in.
    /// </summary>
    IsNotAllowed,

    /// <summary>
    /// Indicates that the login attempt failed because two-factor authentication is disabled for the user.
    /// </summary>
    TwoFactorDisabled,

    /// <summary>
    /// Indicates that the login attempt failed because the authenticator app is not configured for the user.
    /// </summary>
    MissingAuthenticator,

    /// <summary>
    /// Indicates that the login attempt failed due to an invalid two-factor authentication code.
    /// </summary>
    InvalidAuthenticatorCode,

    /// <summary>
    /// Indicates that the login attempt failed due to an error with an external login provider.
    /// </summary>
    ExternalLoginError,

    /// <summary>
    /// Indicates that the login attempt failed due to an external login failure.
    /// </summary>
    ExternalLoginFailed
}
