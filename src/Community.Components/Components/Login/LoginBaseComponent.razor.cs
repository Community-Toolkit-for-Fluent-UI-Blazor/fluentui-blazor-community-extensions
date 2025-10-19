using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base component for rendering customizable login, registration, and password recovery views with support
/// for external authentication providers and configurable UI elements.
/// </summary>
/// <remarks>Use this component as a foundation for building authentication interfaces in Blazor applications. The
/// component exposes parameters for customizing content, labels, icons, and layout, enabling integration with various
/// authentication scenarios. Thread safety is not guaranteed; use this component within the standard Blazor component
/// lifecycle.</remarks>
public partial class LoginBaseComponent
{
    /// <summary>
    /// Represents a collection of RenderFragment instances, each associated with a specific account manager view.
    /// </summary>
    /// <remarks>
    /// Use a dictionary when an enumeration has more than 10 values is more efficient than a switch statement.
    /// </remarks>
    private readonly Dictionary<AccountManagerView, RenderFragment> _accountViewFragments = new(EqualityComparer<AccountManagerView>.Default);

    /// <summary>
    /// Represents the email address associated with the user, used for operations like password recovery.
    /// </summary>
    private string? _email;

    /// <summary>
    /// Gets or sets the content to be rendered within the login component, allowing for customization of the login form
    /// </summary>
    [Parameter]
    public RenderFragment? LoginContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered within the registration view of the component, allowing for customization.
    /// </summary>
    [Parameter]
    public RenderFragment? RegisterContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered within the forgot password view of the component, allowing for customization.
    /// </summary>
    [Parameter]
    public RenderFragment? ForgotPasswordContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the account is disabled.
    /// </summary>
    [Parameter]
    public RenderFragment? AccountDisabledContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the user's account is locked.
    /// </summary>
    [Parameter]
    public RenderFragment? AccountLockedContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when user credentials are invalid.
    /// </summary>
    [Parameter]
    public RenderFragment? InvalidCredentialsContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when an unknown error occurs.
    /// </summary>
    [Parameter]
    public RenderFragment? UnknownErrorContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the user has not confirmed their account.
    /// </summary>
    [Parameter]
    public RenderFragment? UserNotConfirmedContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the forgot password confirmation view.
    /// </summary>
    [Parameter]
    public RenderFragment? ForgotPasswordConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the custom content to display in the reset password section of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ResetPasswordContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when a password reset attempt is invalid.
    /// </summary>
    [Parameter]
    public RenderFragment? InvalidPasswordResetContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when two-factor authentication is required.
    /// </summary>
    [Parameter]
    public RenderFragment? RequiredTwoFactorContent { get; set; }

    /// <summary>
    /// Gets or sets the custom content to display for the recovery code section of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? RecoveryCodeContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the reset password confirmation view.
    /// </summary>
    [Parameter]
    public RenderFragment? ResetPasswordConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display for email confirmation within the component.
    /// </summary>
    [Parameter]
    public RenderFragment? EmailConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the register confirmation area.
    /// </summary>
    [Parameter]
    public RenderFragment? RegisterConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when external provider processing occurs.
    /// </summary>
    [Parameter]
    public EventCallback<ExternalProviderProcessingEventArgs> OnExternalProviderProcessing { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when an external provider registration event occurs.
    /// </summary>
    [Parameter]
    public EventCallback<ExternalProviderRegisterEventArgs> OnExternalProviderRegister { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a password reset is requested.
    /// </summary>
    [Parameter]
    public EventCallback<ResetPasswordEventArgs> OnResetPassword { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether external authentication providers are enabled.
    /// </summary>
    [Parameter]
    public bool UseExternalProviders { get; set; } = true;

    /// <summary>
    /// Gets or sets the view model representing the current login view state.
    /// </summary>
    internal AccountManagerView View { get; private set; }

    /// <summary>
    /// Gets or sets the CSS width value to apply to the component.
    /// </summary>
    /// <remarks>Specify a valid CSS width value, such as "100px", "50%", or "auto". If not set, the component
    /// uses its default width.</remarks>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the labels used in the login component for various UI elements.
    /// </summary>
    [Parameter]
    public AccountLabels Labels { get; set; } = AccountLabels.Default;

    /// <summary>
    /// Gets or sets a delegate that provides an icon for a given icon name from an external source.
    /// </summary>
    /// <remarks>If set, this delegate is called with the icon name to retrieve a custom icon. If not set, the
    /// default icon provider is used. Use this property to supply icons that are not included in the built-in set or to
    /// override existing icons.</remarks>
    [Parameter]
    public Func<string, Icon>? ExternalIconProvider { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a login action is requested.
    /// </summary>
    [Parameter]
    public EventCallback<LoginEventArgs> OnLogin { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a user completes the sign-up process.
    /// </summary>
    [Parameter]
    public EventCallback<RegisterEventArgs> OnSignUp { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the user has successfully logged in.
    /// </summary>
    /// <remarks>Use this callback to perform additional actions after a successful login, such as redirecting
    /// the user or updating application state.</remarks>
    [Parameter]
    public EventCallback OnLoggedIn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display additional text for external authentication providers.
    /// </summary>
    [Parameter]
    public bool ShowExternalProviderText { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a new password is required.
    /// </summary>
    [Parameter]
    public EventCallback<SendPasswordEventArgs> OnSendInstructions { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the login view changes.
    /// </summary>
    /// <remarks>Use this property to handle changes in the current login view, such as switching between
    /// sign-in and registration forms. The callback receives the new <see cref="LoginView"/> value as its
    /// argument.</remarks>
    [Parameter]
    public EventCallback<AccountManagerView> OnViewChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the user requests to resend a verification email.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnResendEmail { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a recovery code is submitted.
    /// </summary>
    [Parameter]
    public EventCallback<RecoveryCodeEventArgs> OnRecoveryCode { get; set; }

    /// <summary>
    /// Gets or sets the custom content to display for an external authentication provider.
    /// </summary>
    [Parameter]
    public RenderFragment? ExternalProviderContent { get; set; }

    /// <summary>
    /// Handles a failed login attempt by updating the login view to reflect the specified failure reason.
    /// </summary>
    /// <remarks>Call this method to update the user interface after a login attempt fails. The view is set
    /// according to the provided failure reason, allowing the user to see the appropriate error or guidance.</remarks>
    /// <param name="reason">The reason for the failed login attempt. Determines which login view is displayed to the user.</param>
    private async Task OnFailedLoginAsync(LoginFailReason reason)
    {
        var view = reason switch
        {
            LoginFailReason.InvalidCredentials => AccountManagerView.InvalidCredentials,
            LoginFailReason.UserNotConfirmed => AccountManagerView.UserNotConfirmed,
            LoginFailReason.AccountLocked => AccountManagerView.AccountLocked,
            LoginFailReason.AccountDisabled => AccountManagerView.AccountDisabled,
            LoginFailReason.UnknownError => AccountManagerView.UnknownError,
            LoginFailReason.RequiredTwoFactor => AccountManagerView.RequiredTwoFactor,
            _ => throw new NotImplementedException("Unsupported login failure reason.")
        };

        await SetViewAsync(view);
    }

    /// <summary>
    /// Handles the completion of sending password reset instructions and updates the view accordingly.
    /// </summary>
    /// <param name="email">The email address to which the password reset instructions were sent. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnInstructionsSentAsync(string email)
    {
        _email = email;
        await SetViewAsync(AccountManagerView.ForgotPasswordConfirmation);
    }

    /// <summary>
    /// Sets the current login view to the specified view and updates the component state.
    /// </summary>
    /// <param name="view">The login view to display. Cannot be null.</param>
    internal async Task SetViewAsync(AccountManagerView view)
    {
        View = view;
        await InvokeAsync(StateHasChanged);

        if (OnViewChanged.HasDelegate)
        {
            await InvokeAsync(() => OnViewChanged.InvokeAsync(view));
        }
    }
}
