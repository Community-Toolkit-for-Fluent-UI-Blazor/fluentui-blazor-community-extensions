using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a login form component that supports customizable labels, external authentication providers, and optional
/// icon customization.
/// </summary>
/// <remarks>Use this component to present a user login interface with support for both standard and external
/// authentication methods. The component allows customization of form labels and icons, enabling localization and
/// branding. Set the relevant parameters to configure the appearance and behavior of the login form according to your
/// application's requirements.</remarks>
public partial class LoginView
{
    /// <summary>
    /// Represents a value indicating whether the login process is currently in progress.
    /// </summary>
    private bool _isLoading;

    /// <summary>
    /// References the internal <see cref="FluentEditForm"/> used for handling user input and validation.
    /// </summary>
    private FluentEditForm? _fluentEditForm;

    /// <summary>
    /// Gets the parent <see cref="FluentCxLogin"/> component in the component hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether external authentication providers are enabled.
    /// </summary>
    [Parameter]
    public bool UseExternalProviders { get; set; }

    /// <summary>
    /// Gets or sets a delegate that provides a custom icon for a given icon name.
    /// </summary>
    /// <remarks>If set, this delegate is invoked to retrieve an icon when a custom icon is required. The
    /// delegate receives the icon name as a parameter and should return an appropriate <see cref="Icon"/> instance, or
    /// <see langword="null"/> if no icon is available for the specified name.</remarks>
    [Parameter]
    public Func<string, Icon>? ExternalIconProvider { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display text for external authentication providers.
    /// </summary>
    [Parameter]
    public bool ShowExternalProviderText { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a login event occurs.
    /// </summary>
    /// <remarks>Use this property to handle login events, such as when a user successfully signs in. The
    /// callback receives a <see cref="LoginEventArgs"/> instance containing details about the login event.</remarks>
    [Parameter]
    public EventCallback<LoginEventArgs> OnLogin { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the user has successfully logged in.
    /// </summary>
    /// <remarks>Use this callback to perform additional actions after a successful login, such as redirecting
    /// the user or updating application state.</remarks>
    [Parameter]
    public EventCallback OnLoggedIn { get; set; }

    /// <summary>
    /// Gets the current login model containing user input and authentication state for the login process.
    /// </summary>
    private LoginModel Model { get; } = new();

    /// <summary>
    /// Gets a value indicating whether the form is currently disabled due to loading or validation errors.
    /// </summary>
    private bool IsDisabled => _isLoading || (_fluentEditForm?.EditContext?.GetValidationMessages().Any() ?? false) || !Model.IsValid;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(LoginView)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <summary>
    /// Handles the forgot password action by updating the parent view asynchronously.
    /// </summary>
    /// <remarks>This method changes the parent view to the forgot password screen if a parent is available.
    /// It should be awaited to ensure the view transition completes before proceeding.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnForgotPasswordAsync()
    {
        if (Parent is not null)
        {
            await Parent.SetViewAsync(AccountManagerView.ForgotPassword);
        }
    }

    /// <summary>
    /// Initiates the sign-up process by updating the parent view to the registration screen asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSignUpAsync()
    {
        if (Parent is not null)
        {
            await Parent.SetViewAsync(AccountManagerView.Register);
        }
    }

    /// <summary>
    /// Handles the login process asynchronously, raising the appropriate events based on the login outcome.
    /// </summary>
    /// <remarks>This method triggers the login event with the current user credentials and raises either the
    /// successful or failed login event depending on the result. It is typically called in response to a user action,
    /// such as submitting a login form.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginAsync()
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);

        var e = new LoginEventArgs(Model.Email!, Model.Password!, Model.RememberMe);

        if (OnLogin.HasDelegate)
        {
            await OnLogin.InvokeAsync(e);
            _isLoading = false;

            if (e.IsSuccessful &&
                OnLoggedIn.HasDelegate)
            {
                await OnLoggedIn.InvokeAsync();
            }
            else 
            {
                var view = e.FailReason switch
                {
                    LoginFailReason.InvalidCredentials => AccountManagerView.InvalidCredentials,
                    LoginFailReason.UserNotConfirmed => AccountManagerView.UserNotConfirmed,
                    LoginFailReason.AccountLocked => AccountManagerView.AccountLocked,
                    LoginFailReason.AccountDisabled => AccountManagerView.AccountDisabled,
                    LoginFailReason.UnknownError => AccountManagerView.UnknownError,
                    LoginFailReason.RequiredTwoFactor => AccountManagerView.RequiredTwoFactor,
                    _ => throw new NotImplementedException("Unsupported login failure reason.")
                };

                await Parent!.SetViewAsync(view);
            }
        }
        else
        {
            _isLoading = false;
        }
    }

    /// <summary>
    /// Transitions the parent view to display the external provider selection interface asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnExternalProviderSelectedAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.ExternalProvider);
    }
}
