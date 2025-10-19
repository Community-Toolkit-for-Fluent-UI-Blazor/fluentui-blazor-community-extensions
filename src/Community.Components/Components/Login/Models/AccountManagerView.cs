namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available views for user authentication, such as login or registration forms.
/// </summary>
public enum AccountManagerView
{
    /// <summary>
    /// Represents the login view, typically used for user authentication.
    /// </summary>
    Login,

    /// <summary>
    /// Represents the registration view, typically used for new user sign-up.
    /// </summary>
    Register,

    /// <summary>
    /// Represents the forgot password view, typically used for password recovery.
    /// </summary>
    ForgotPassword,

    /// <summary>
    /// Represents the forgot password confirmation view, typically used to confirm that a password reset.
    /// </summary>
    ForgotPasswordConfirmation,

    /// <summary>
    /// Indicates that authentication failed due to invalid credentials.
    /// </summary>
    InvalidCredentials,

    /// <summary>
    /// Indicates that the user has not completed the required confirmation process.
    /// </summary>
    UserNotConfirmed,

    /// <summary>
    /// Indicates whether the account is currently locked.
    /// </summary>
    AccountLocked,

    /// <summary>
    /// Indicates that the account is disabled and cannot be used for authentication or access.
    /// </summary>
    AccountDisabled,

    /// <summary>
    /// Represents an error condition that does not match any known or specific error type.
    /// </summary>
    UnknownError,

    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is required for the user.
    /// </summary>
    RequiredTwoFactor,

    /// <summary>
    /// Represents the reset password view, typically used for resetting a user's password.
    /// </summary>
    ResetPassword,

    /// <summary>
    /// Represents the invalid password reset view, typically used when a password reset attempt fails.
    /// </summary>
    InvalidPasswordReset,

    /// <summary>
    /// Represents the reset password confirmation view, typically used to confirm that a password has been successfully reset.
    /// </summary>
    ResetPasswordConfirmation,

    /// <summary>
    /// Represents the recovery code view, typically used for account recovery via a recovery code.
    /// </summary>
    RecoveryCode,

    /// <summary>
    /// Represents the external provider view, typically used for third-party authentication.
    /// </summary>
    ExternalProvider,

    /// <summary>
    /// Represents the register confirmation view, typically used to confirm successful user registration.
    /// </summary>
    RegisterConfirmation,

    /// <summary>
    /// Represents the email confirmation view, typically used for confirming a user's email address.
    /// </summary>
    EmailConfirmation
}
