namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of localized labels and display texts used for login and authentication user interface elements.
/// </summary>
/// <remarks>Use this record to provide custom or localized text for login forms, registration prompts, and
/// related authentication UI components. Predefined instances such as <see cref="Default"/> and <see cref="French"/>
/// are available for common scenarios, or you can create custom instances for additional languages or branding
/// requirements.</remarks>
public record AccountLabels
{
    /// <summary>
    /// Gets the default set of labels for login-related user interface elements.
    /// </summary>
    /// <remarks>Use this property to access a standard set of login labels when no customization is required.
    /// The returned instance is read-only and shared across the application.</remarks>
    public static AccountLabels Default { get; } = new AccountLabels();

    /// <summary>
    /// Gets the set of login-related labels localized in French.
    /// </summary>
    /// <remarks>Use this property to access French translations for common authentication UI elements, such
    /// as login, registration, and password fields. This is useful for applications that support multiple languages and
    /// need to display user interface text in French.</remarks>
    public static AccountLabels French { get; } = new AccountLabels
    {
        Login = "Connexion",
        Logout = "Déconnexion",
        Register = "Inscription",
        UserName = "Nom d'utilisateur",
        Password = "Mot de passe",
        RememberMe = "Se souvenir de moi",
        ExternalLogin = "Ou utilisez un service externe pour vous connecter",
        ConnectWithProvider = "Se connecter avec {0}",
        SignUpWithProvider = "S'inscire avec {0}",
        Email = "Adresse courriel",
        EmailPlaceholder = "Entrez votre adresse courriel",
        PasswordPlaceholder = "Entrez votre mot de passe",
        ForgotPassword = "Mot de passe oublié ?",
        DontHaveAnAccount = "Vous n'avez pas de compte ?",
        SignUp = "S'inscrire",
        AlreadyHaveAnAccount = "Vous avez déjà un compte ?",
        ForgotPasswordInstructions = "Entrez l'adresse courriel associée à votre compte et nous vous enverrons un courriel avec les instructions pour réinitialiser votre mot de passe.",
        SendInstructions = "Envoyer les instructions",
        ConfirmPassword = "Confirmer le mot de passe",
        ConfirmPasswordPlaceholder = "Saisissez à nouveau votre mot de passe",
        DisplayName = "Nom d'affichage",
        DisplayNamePlaceholder = "Entrez votre nom d'affichage",
        ForgotPasswordConfirmation = "Confirmation de mot de passe oublié",
        ForgotPasswordConfirmationInstructions = "Si un compte avec cette adresse courriel existe, vous recevrez un courriel avec les instructions pour réinitialiser votre mot de passe.",
        EmailNotReceived = "Vous n'avez pas reçu le courriel ?",
        ResendEmail = "Renvoyer le courriel",
        ResetPassword = "Réinitialiser le mot de passe",
        Reset = "Réinitialiser",
        ResetPasswordError = "Erreur lors de la réinitialisation de votre mot de passe.",
        InvalidPasswordReset = "Echec de la redéfinition du mot de passe",
        ResetPasswordInvalidLink = "Le lien de réinitialisation du mot de passe est invalide ou a expiré. Veuillez demander une nouvelle réinitialisation du mot de passe.",
        ResetPasswordConfirmation = "Confirmation de réinitialisation du mot de passe",
        ResetPasswordConfirmationInstructions = "Votre mot de passe a été réinitialisé. Vous pouvez maintenant vous connecter avec votre nouveau mot de passe.",
        InvalidCredentials = "Identifiants invalides",
        InvalidCredentialsMessage = "Le nom d'utilisateur ou le mot de passe que vous avez saisi est incorrect. Veuillez réessayer.",
        AuthenticationCode = "Code d'authentification",
        AuthenticationTwoFactor = "Authentification à deux facteurs",
        AuthenticationTwoFactorMessage = "Votre connexion est protégée par une application d'authentification. Saisissez votre code depuis votre application d'authentification ci-dessous :",
        RememberMachine = "Se souvenir de cette machine",
        DontHaveTwoFactorAccess = "Vous n'avez pas accès à votre application d'authentification ?",
        LoginWithRecoveryCode = "Se connecter avec un code de récupération",
        SummaryErrorMessage = "Veuillez corriger les erreurs suivantes pour continuer :",
        InvalidRecoveryCodeMessage = "Le code de récupération que vous avez saisi est invalide. Veuillez réessayer.",
        RecoveryCode = "Code de récupération",
        RecoveryCodePlaceholder = "Entrez votre code de récupération",
        VerifyRecoveryCode = "Vérifier le code de récupération",
        VerifyRecoveryCodeDescription = "Vous avez demandé à vous connecter en utilisant un code de récupération. Cette connexion ne sera pas mémorisée tant que vous n'aurez pas fourni un code d'application d'authentification lors de la connexion ou désactivé l'authentification à deux facteurs et vous être reconnecté.",
        AccountDisabled = "Compte désactivé",
        AccountDisabledMessage = "Votre compte a été désactivé. Veuillez contacter le support pour obtenir de l'aide.",
        AccountLocked = "Verrouillé",
        AccountLockedMessage = "Votre compte a été verrouillé en raison de plusieurs tentatives de connexion infructueuses. Veuillez réessayer plus tard ou contacter le support pour obtenir de l'aide.",
        EmailNotFoundErrorMessage = "L'adresse courriel fournie {0} est introuvable.",
        EmailAlreadyInUseErrorMessage = "L'adresse courriel {0} est déjà utilisée.",
        NoServerResponseErrorMessage = "Aucune réponse du serveur. Veuillez réessayer plus tard.",
        UserNameAlreadyInUse = "Le nom d'utilisateur {0} est déjà utilisé.",
        UserNotConfirmed = "Utilisateur non confirmé",
        UserNotConfirmedMessage = "Votre compte n'a pas été confirmé. Veuillez vérifier votre courriel pour le lien de confirmation ou contacter le support pour obtenir de l'aide.",
        UnknownError = "Erreur inconnue",
        UnknownErrorMessage = "Une erreur inconnue s'est produite. Veuillez réessayer plus tard.",
        AssociateProviderNameAccount = "Associez votre compte {0}.",
        AssociateProviderNameAccountMessage = "Vous vous êtes authentifié avec succès via {0}. Veuillez entrer une adresse courriel pour ce site ci-dessous et cliquer sur le bouton d'inscription pour terminer la connexion.",
        ExternalProviderAuthentication = "Authentification par un fournisseur externe",
        ExternalProviderProcessing = "Traitement de l'authentification par un fournisseur externe...",
        EmailConfirmation = "Confirmation de l'adresse courriel",
        EmailConfirmationMessage = "Votre courriel a été confirmé avec succès. Vous pouvez maintenant vous connecter avec vos identifiants.",
        EmailConfirmationProcessing = "Traitement de la confirmation de l'adresse courriel...",
        ErrorEmailMessage = "Une erreur s'est produite lors de la confirmation du courriel. Veuillez réessayer plus tard ou contacter le support pour obtenir de l'aide.",
        RegisterConfirmation = "Confirmation d'inscription",
        RegisterConfirmationMessage = "Merci pour votre inscription ! Votre compte a été créé avec succès. Veuillez vérifier votre courriel pour confirmer votre compte.",
        ProcessingLogin = "Connexion en cours...",
        InvalidTwoFactorCodeMessage = "Le code d'authentification à deux facteurs que vous avez saisi est invalide. Veuillez réessayer.",
        TwoFactorUnknownError = "Une erreur inconnue s'est produite lors de l'authentification à deux facteurs. Veuillez réessayer plus tard.",
        ExternalLoginError = "Erreur de connexion externe",
        ExternalLoginErrorMessage = "Une erreur s'est produite lors de la connexion externe. Veuillez réessayer plus tard.",
        ExternalLoginFailed = "Échec de la connexion externe",
        ExternalLoginFailedMessage = "La connexion externe a échoué. Veuillez réessayer.",
        InvalidAuthenticatorCode = "Code d'authentificateur invalide",
        InvalidAuthenticatorCodeMessage = "Le code d'authentificateur que vous avez saisi est invalide. Veuillez réessayer.",
        MissingAuthenticator = "Authentificateur manquant",
        MissingAuthenticatorMessage = "Aucun application d'authentification n'est configurée pour votre compte.",
        TwoFactorDisabled = "Authentification à deux facteurs désactivée",
        TwoFactorDisabledMessage = "L'authentification à deux facteurs est désactivée pour votre compte.",
        UserNotAllowed = "Utilisateur non autorisé",
        UserNotAllowedMessage = "Vous n'êtes pas autorisé à vous connecter."
    };

    /// <summary>
    /// Gets the login identifier associated with the user or entity.
    /// </summary>
    public string Login { get; init; } = "Login";

    /// <summary>
    /// Gets the display text for the logout action.
    /// </summary>
    public string Logout { get; init; } = "Logout";

    /// <summary>
    /// Gets the name of the register associated with this instance.
    /// </summary>
    public string Register { get; init; } = "Register";

    /// <summary>
    /// Gets the user name associated with the current instance.
    /// </summary>
    public string UserName { get; init; } = "User Name";

    /// <summary>
    /// Gets the password associated with the current instance.
    /// </summary>
    public string Password { get; init; } = "Password";

    /// <summary>
    /// Gets the display text for the 'Remember Me' option.
    /// </summary>
    public string RememberMe { get; init; } = "Remember Me";

    /// <summary>
    /// Gets the display text for the external login option.
    /// </summary>
    public string ExternalLogin { get; init; } = "Or use an external service to login";

    /// <summary>
    /// Gets the format string used to display the 'Connect with' prompt for a specific provider.
    /// </summary>
    /// <remarks>The format string should contain a placeholder (such as "{0}") that will be replaced with the
    /// provider's name at runtime.</remarks>
    public string ConnectWithProvider { get; init; } = "Connect with {0}";

    /// <summary>
    /// Gets the format string used to display a sign-up prompt with a specific authentication provider.
    /// </summary>
    /// <remarks>This string is intended to be formatted with the name of the authentication provider. For
    /// example, formatting with "Google" produces "Sign up with Google".</remarks>
    public string SignUpWithProvider { get; init; } = "Sign up with {0}";

    /// <summary>
    /// Gets the email address associated with the login model.
    /// </summary>
    public string Email { get; init; } = "Email";

    /// <summary>
    /// Gets the placeholder text displayed in the email input field.
    /// </summary>
    public string EmailPlaceholder { get; init; } = "Enter your email";

    /// <summary>
    /// Gets the placeholder text displayed in the password input field.
    /// </summary>
    public string PasswordPlaceholder { get; init; } = "Enter your password";

    /// <summary>
    /// Gets the display text for the 'Forgot Password' link or button.
    /// </summary>
    public string ForgotPassword { get; init; } = "Forgot Password ?";

    /// <summary>
    /// Gets the default message displayed to users who do not have an account.
    /// </summary>
    public string DontHaveAnAccount { get; init; } = "Don't have an account ?";

    /// <summary>
    /// Gets the localized text displayed to prompt users who already have an account.
    /// </summary>
    public string AlreadyHaveAnAccount { get; init; } = "Already have an account ?";

    /// <summary>
    /// Gets the localized display text for the sign-up action.
    /// </summary>
    public string SignUp { get; init; } = "Sign Up";

    /// <summary>
    /// Gets or sets the instructions displayed to users for resetting a forgotten password.
    /// </summary>
    public string ForgotPasswordInstructions { get; init; } = "Enter the email associated to your account and we'll send an email with instruction to reset your password.";

    /// <summary>
    /// Gets or sets the text displayed on the button to send password reset instructions.
    /// </summary>
    public string SendInstructions { get; init; } = "Send Instructions";

    /// <summary>
    /// Gets the confirmation password value used for validating user input.
    /// </summary>
    public string ConfirmPassword { get; init; } = "Confirm Password";

    /// <summary>
    /// Gets the placeholder text displayed in the confirm password input field.
    /// </summary>
    public string ConfirmPasswordPlaceholder { get; init; } = "Re-enter your password";

    /// <summary>
    /// Gets the display name value used for validating user input.
    /// </summary>
    public string DisplayName { get; init; } = "Display Name";

    /// <summary>
    /// Gets the placeholder text displayed in the display name input field before the user enters a value.
    /// </summary>
    public string DisplayNamePlaceholder { get; init; } = "Enter your display name";

    /// <summary>
    /// Gets the confirmation message displayed after a successful password reset request.
    /// </summary>
    public string ForgotPasswordConfirmation { get; init; } = "Forgot password confirmation";

    /// <summary>
    /// Gets the confirmation instructions displayed to users after they request a password reset.
    /// </summary>
    public string ForgotPasswordConfirmationInstructions { get; init; } = "If an account with that email address exists, you will receive an email with instructions to reset your password.";

    /// <summary>
    /// Gets the message displayed when an email has not been received.
    /// </summary>
    public string EmailNotReceived { get; init; } = "Haven't received the email ?";

    /// <summary>
    /// Gets the display text for the resend email action.
    /// </summary>
    public string ResendEmail { get; init; } = "Resend Email";

    /// <summary>
    /// Gets the display text for the reset password action.
    /// </summary>
    public string ResetPassword { get; init; } = "Reset Password";

    /// <summary>
    /// Gets the string value representing the reset action.
    /// </summary>
    public string Reset { get; init; } = "Reset";

    /// <summary>
    /// Gets the error message to display when a password reset operation fails.
    /// </summary>
    public string ResetPasswordError { get; init; } = "Error resetting your password.";

    /// <summary>
    /// Gets the default message displayed when a password reset request is invalid.
    /// </summary>
    public string InvalidPasswordReset { get; init; } = "Password reset failed";

    /// <summary>
    /// Gets the message displayed when a password reset link is invalid or has expired.
    /// </summary>
    public string ResetPasswordInvalidLink { get; init; } = "The password reset link is invalid or has expired. Please request a new password reset.";

    /// <summary>
    /// Gets the confirmation message displayed to the user after a password reset operation.
    /// </summary>
    public string ResetPasswordConfirmation { get; init; } = "Reset password confirmation";

    /// <summary>
    /// Gets the confirmation instructions displayed to the user after a successful password reset.
    /// </summary>
    public string ResetPasswordConfirmationInstructions { get; init; } = "Your password has been reset. You can now log in with your new password.";

    /// <summary>
    /// Gets the default error message displayed when user credentials are invalid.
    /// </summary>
    public string InvalidCredentials { get; init; } = "Invalid Credentials";

    /// <summary>
    /// Gets the default message displayed when user credentials are invalid during authentication.
    /// </summary>
    public string InvalidCredentialsMessage { get; init; } = "The username or password you entered is incorrect. Please try again.";

    /// <summary>
    /// Gets the display name for two-factor authentication.
    /// </summary>
    public string AuthenticationTwoFactor { get; init; } = "Two-Factor Authentication";

    /// <summary>
    /// Gets the message displayed to users when two-factor authentication is required during login.
    /// </summary>
    public string AuthenticationTwoFactorMessage { get; init; } = "Your login is protected by an authentication app. Enter your code from your authentication app below :";

    /// <summary>
    /// Gets the authentication code used to verify user identity during authentication processes.
    /// </summary>
    public string AuthenticationCode { get; init; } = "Authentication Code";

    /// <summary>
    /// Gets the display text used to prompt the user to remember this machine during authentication.
    /// </summary>
    public string RememberMachine { get; init; } = "Remember this machine";

    /// <summary>
    /// Gets the message displayed when the user does not have access to their two-factor authentication app.
    /// </summary>
    public string DontHaveTwoFactorAccess { get; init; } = "Don't have access to your authentication app ?";

    /// <summary>
    /// Gets the text to display for the option to log in using a recovery code.
    /// </summary>
    public string LoginWithRecoveryCode { get; init; } = "Login with a recovery code";

    /// <summary>
    /// Gets the default error message displayed when validation fails for a summary section.
    /// </summary>
    public string SummaryErrorMessage { get; init; } = "Please correct the following errors to continue :";

    /// <summary>
    /// Gets the label to display for verifying a recovery code.
    /// </summary>
    public string VerifyRecoveryCode { get; init; } = "Verify Recovery Code";

    /// <summary>
    /// Gets the description text displayed when verifying a recovery code.
    /// </summary>
    public string VerifyRecoveryCodeDescription { get; init; } = "You have requested to log in using a recovery code. This login will not be remembered until you provide an authentication app code during login or disable two-factor authentication and log in again.";

    /// <summary>
    /// Gets the label for the recovery code input field.
    /// </summary>
    public string RecoveryCode { get; init; } = "Recovery Code";

    /// <summary>
    /// Gets the placeholder text displayed in the recovery code input field.
    /// </summary>
    public string RecoveryCodePlaceholder { get; init; } = "Enter your recovery code";

    /// <summary>
    /// Gets the message displayed when a user enters an invalid recovery code.
    /// </summary>
    public string InvalidRecoveryCodeMessage { get; init; } = "The recovery code you entered is invalid. Please try again.";

    /// <summary>
    /// Gets the status message indicating that the user account is locked out.
    /// </summary>
    public string AccountLocked { get; init; } = "Locked out";

    /// <summary>
    /// Gets the message displayed to users when their account is locked due to multiple unsuccessful login attempts.
    /// </summary>
    public string AccountLockedMessage { get; init; } = "Your account has been locked due to multiple unsuccessful login attempts. Please try again later or contact support for assistance.";

    /// <summary>
    /// Gets the status message indicating that the user account is disabled.
    /// </summary>
    public string AccountDisabled { get; init; } = "Account disabled";

    /// <summary>
    /// Gets the default message displayed when a user's account has been disabled.
    /// </summary>
    public string AccountDisabledMessage { get; init; } = "Your account has been disabled. Please contact support for assistance.";

    /// <summary>
    /// Gets the error message displayed when a specified email address is not found.
    /// </summary>
    public string EmailNotFoundErrorMessage { get; init; } = "The provided email address {0} was not found.";

    /// <summary>
    /// Gets the error message displayed when an email address is already associated with an existing account.
    /// </summary>
    public string EmailAlreadyInUseErrorMessage { get; init; } = "The email address {0} is already in use.";

    /// <summary>
    /// Gets the error message displayed when there is no response from the server.
    /// </summary>
    public string NoServerResponseErrorMessage { get; init; } = "No response from the server. Please try again later.";

    /// <summary>
    /// Gets the message template displayed when a username is already in use.
    /// </summary>
    public string UserNameAlreadyInUse { get; init; } = "The username {0} is already in use.";

    /// <summary>
    /// Gets the label displayed when a user has not confirmed their account.
    /// </summary>
    public string UserNotConfirmed { get; init; } = "User Not Confirmed";

    /// <summary>
    /// Gets the message displayed to users whose accounts have not been confirmed.
    /// </summary>
    public string UserNotConfirmedMessage { get; init; } = "Your account has not been confirmed. Please check your email for the confirmation link or contact support for assistance.";

    /// <summary>
    /// Gets the default error message used when the specific error is unknown.
    /// </summary>
    public string UnknownError { get; init; } = "Unknown Error";

    /// <summary>
    /// Gets the default message displayed when an unspecified error occurs.
    /// </summary>
    public string UnknownErrorMessage { get; init; } = "An unknown error has occurred. Please try again later.";

    /// <summary>
    /// Gets or sets the format string used to prompt users to associate their account with a specified provider.
    /// </summary>
    /// <remarks>The format string should include a placeholder, such as "{0}", which will be replaced with
    /// the provider's name at runtime. For example, if the provider is "Microsoft", the resulting prompt will be
    /// "Associate your Microsoft account."</remarks>
    public string AssociateProviderNameAccount { get; init; } = "Associate your {0} account.";

    /// <summary>
    /// Gets or sets the message displayed to users after successful authentication with an external provider, prompting
    /// them to enter an email address and complete registration.
    /// </summary>
    /// <remarks>The message may include a placeholder, such as "{0}", which is replaced with the name of the
    /// external authentication provider at runtime.</remarks>
    public string AssociateProviderNameAccountMessage { get; init; } = "You've successfully authenticated with {0}. Please enter an email address for this site below and click the Register button to finish logging in.";

    /// <summary>
    /// Gets or sets the authentication method used by an external provider.
    /// </summary>
    public string ExternalProviderAuthentication { get; init; } = "External Provider Authentication";

    /// <summary>
    /// Gets or sets the message displayed while processing external provider authentication.
    /// </summary>
    public string ExternalProviderProcessing { get; init; } = "Processing external provider authentication...";

    /// <summary>
    /// Gets or sets the confirmation message displayed after a successful registration.
    /// </summary>
    public string RegisterConfirmation { get; init; } = "Register Confirmation";

    /// <summary>
    /// Gets or sets the message displayed to users after successful registration.
    /// </summary>
    public string RegisterConfirmationMessage { get; init; } = "Thank you for registering! Your account has been created successfully. Please, check your email to confirm your account.";

    /// <summary>
    /// Gets or sets the label for email confirmation.
    /// </summary>
    public string EmailConfirmation { get; init; } = "Email Confirmation";

    /// <summary>
    /// Gets or sets the message displayed to users after successful email confirmation.
    /// </summary>
    public string EmailConfirmationMessage { get; init; } = "Your email has been confirmed successfully. You can now log in with your credentials.";

    /// <summary>
    /// Gets or sets the message displayed when an error occurs while confirming an email.
    /// </summary>
    public string ErrorEmailMessage { get; init; } = "There was an error confirming the email. Please try again later, or contact support for assistance.";

    /// <summary>
    /// Gets or sets the message displayed while processing email confirmation.
    /// </summary>
    public string EmailConfirmationProcessing { get; init; } = "Processing email confirmation...";

    /// <summary>
    /// Gets or sets the message displayed while a login operation is in progress.
    /// </summary>
    public string ProcessingLogin { get; init; } = "Processing login...";

    /// <summary>
    /// Gets the error message to display when an unknown error occurs during two-factor authentication.
    /// </summary>
    public string? TwoFactorUnknownError { get; init; } = "An unknown error occurred during two-factor authentication. Please try again later.";

    /// <summary>
    /// Gets the message displayed when a user enters an invalid two-factor authentication code.
    /// </summary>
    public string? InvalidTwoFactorCodeMessage { get; init; } = "The two-factor authentication code you entered is invalid. Please try again.";

    /// <summary>
    /// Gets the message displayed to users who are not permitted to log in.
    /// </summary>
    public string? UserNotAllowedMessage { get; init; } = "You are not allowed to log in.";

    /// <summary>
    /// Gets the message indicating that a user is not allowed to perform the requested action.
    /// </summary>
    public string? UserNotAllowed { get; init; } = "User Not Allowed";

    /// <summary>
    /// Gets the message displayed to users when two-factor authentication is disabled for their account.
    /// </summary>
    public string? TwoFactorDisabledMessage { get; init; } = "Two-factor authentication is disabled for your account.";

    /// <summary>
    /// Gets the label indicating that two-factor authentication is disabled.
    /// </summary>
    public string? TwoFactorDisabled { get; init; } = "Two-Factor Disabled";

    /// <summary>
    /// Gets the message displayed when no authenticator app is configured for the user's account.
    /// </summary>
    public string? MissingAuthenticatorMessage { get; init; } = "No authenticator app is configured for your account.";

    /// <summary>
    /// Gets the message indicating that an authenticator is missing.
    /// </summary>
    public string? MissingAuthenticator { get; init; } = "Missing Authenticator";

    /// <summary>
    /// Gets the message displayed when an invalid authenticator code is entered.
    /// </summary>
    public string? InvalidAuthenticatorCodeMessage { get; init; } = "The authenticator code you entered is invalid. Please try again.";

    /// <summary>
    /// Gets the error message displayed when an authenticator code is invalid.
    /// </summary>
    public string? InvalidAuthenticatorCode { get; init; } = "Invalid Authenticator Code";

    /// <summary>
    /// Gets the error message displayed when an external login attempt fails.
    /// </summary>
    public string? ExternalLoginErrorMessage { get; init; } = "An error occurred during external login. Please try again later.";

    /// <summary>
    /// Gets the error message associated with an external login attempt.
    /// </summary>
    public string? ExternalLoginError { get; init; } = "External Login Error";

    /// <summary>
    /// Gets the error message displayed when an external login attempt fails.
    /// </summary>
    public string? ExternalLoginFailedMessage { get; init; } = "External login failed. Please try again.";

    /// <summary>
    /// Gets the error message displayed when an external login attempt fails.
    /// </summary>
    public string? ExternalLoginFailed { get; init; } = "External Login Failed";
}
