using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view component that renders the user registration interface, including options for external
/// authentication providers.
/// </summary>
public partial class RegisterView
{
    /// <summary>
    /// Represents a value indicating whether the registration process is currently in progress.
    /// </summary>
    private bool _isLoading;

    /// <summary>
    /// Represents the FluentEditForm component used for user input in the registration view.
    /// </summary>
    private FluentEditForm? _fluentEditForm;

    /// <summary>
    /// Represents the model containing user registration data.
    /// </summary>
    private readonly RegisterModel _model = new();

    /// <summary>
    /// Represents an error message to be displayed in the registration view.
    /// </summary>
    private string? _errorMessage;

    /// <summary>
    /// Gets or sets a value indicating whether external authentication providers are enabled.
    /// </summary>
    [Parameter]
    public bool UseExternalProviders { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display text for external authentication providers.
    /// </summary>
    [Parameter]
    public bool ShowExternalProviderText { get; set; }

    /// <summary>
    /// Gets or sets a delegate that provides a custom icon for a given icon name.
    /// </summary>
    [Parameter]
    public Func<string, Icon>? ExternalIconProvider { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a user completes the sign-up process.
    /// </summary>
    [Parameter]
    public EventCallback<RegisterEventArgs> OnSignUp { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current operation is disabled.
    /// </summary>
    private bool IsDisabled => _isLoading || (_fluentEditForm?.EditContext?.GetValidationMessages().Any() ?? false);

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(RegisterView)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <summary>
    /// Handles the user sign-up process asynchronously, raising relevant events based on the outcome.
    /// </summary>
    /// <remarks>This method triggers the sign-up event with the user's registration details. If registration
    /// succeeds, it raises a completion event; otherwise, it raises a failure event with the reason for failure. The
    /// method is typically called in response to a user action, such as submitting a registration form.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSignUpAsync()
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);

        var e = new RegisterEventArgs(_model.DisplayName!, _model.Email!, _model.Password!, _model.ConfirmPassword!);

        if (OnSignUp.HasDelegate)
        {
            await OnSignUp.InvokeAsync(e);
        }

        _isLoading = false;

        if (e.IsSuccessful)
        {
            await Parent!.SetViewAsync(AccountManagerView.Login);
        }
        else
        {
            switch (e.FailReason)
            {
                case RegisterFailReason.DisplayNameAlreadyInUse:
                    {
                        _errorMessage = string.Format(CultureInfo.CurrentCulture, Parent!.Labels.UserNameAlreadyInUse, e.DisplayName);
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case RegisterFailReason.EmailAlreadyInUse:
                    {
                        _errorMessage = Parent!.Labels.EmailAlreadyInUseErrorMessage;
                        await InvokeAsync(StateHasChanged);
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// Transitions the parent view to the login screen asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
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
