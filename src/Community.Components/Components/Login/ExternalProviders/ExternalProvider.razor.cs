using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an external provider component that manages account-related display labels.
/// </summary>
public partial class ExternalProvider
{
    /// <summary>
    /// Represents the model for external provider data.
    /// </summary>
    private readonly ExternalProviderModel _model = new();

    /// <summary>
    /// Represents the FluentEditForm component used for user input.
    /// </summary>
    private FluentEditForm? _fluentEditForm;

    /// <summary>
    /// Represents the collection of error messages.
    /// </summary>
    private readonly List<string> _errors = [];

    /// <summary>
    /// Value indicating whether the component is currently processing an operation.
    /// </summary>
    private bool _isProcessing = true;

    /// <summary>
    /// Value indicating whether the user must register an account.
    /// </summary>
    private bool _mustRegister;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set by the Blazor framework to provide access to the nearest
    /// ancestor <see cref="FluentCxLogin"/> component. It should not be set manually in application code.</remarks>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets or sets the account state service that provides information about the current account status.
    /// </summary>
    [Inject]
    private AccountState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when external provider processing occurs.
    /// </summary>
    [Parameter]
    public EventCallback<ExternalProviderProcessingEventArgs> OnProcessing { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used for managing URI navigation and location state within the
    /// application.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when a user registers using an external authentication provider.
    /// </summary>
    [Parameter]
    public EventCallback<ExternalProviderRegisterEventArgs> OnRegister { get; set; }

    /// <summary>
    /// Gets a value indicating whether the registration action is disabled based on the email field in the model.
    /// </summary>
    private bool IsDisabled => _fluentEditForm?.EditContext?.GetValidationMessages()?.Any() ?? false;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(ExternalProvider)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <summary>
    /// Formats and returns the associate provider name and account information using the current culture.
    /// </summary>
    /// <returns>A string containing the formatted associate provider name and account, localized according to the current
    /// culture.</returns>
    private string GetAssociateProviderNameAccount()
    {
        return string.Format(CultureInfo.CurrentCulture, Parent!.Labels.AssociateProviderNameAccount, State.Provider);
    }

    /// <summary>
    /// Generates a formatted message associating the provider name with the account, using the current culture.
    /// </summary>
    /// <returns>A string containing the localized message with the provider name inserted.</returns>
    private string GetAssociateProviderNameAccountMessage()
    {
        return string.Format(CultureInfo.CurrentCulture, Parent!.Labels.AssociateProviderNameAccountMessage, State.Provider);
    }

    /// <summary>
    /// Handles the asynchronous registration process for the current context.
    /// </summary>
    /// <returns>A task that represents the asynchronous registration operation.</returns>
    private async Task OnRegisterAsync()
    {
        var e = new ExternalProviderRegisterEventArgs(_model.Email!);
        _errors.Clear();
        await InvokeAsync(StateHasChanged);

        if (OnRegister.HasDelegate)
        {
            await OnRegister.InvokeAsync(e);
        }

        if (e.Errors.Count > 0)
        {
            _errors.AddRange(e.Errors);
            await InvokeAsync(StateHasChanged);
            return;
        }

        if (e.RequireConfirmedAccount)
        {
            await Parent!.SetViewAsync(AccountManagerView.RegisterConfirmation);
        }
        else if (e.IsSuccessful)
        {
            NavigationManager.NavigateTo(State.ReturnUrl ?? "/");
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await OnFirstAfterRenderAsync();
        }
    }

    /// <summary>
    /// Handles the initial post-render processing for an external authentication provider, updating the UI and
    /// navigation state based on the outcome of the external provider event.
    /// </summary>
    /// <remarks>This method should be called after the component's first render to process the result of an
    /// external authentication attempt. Depending on the outcome, it may navigate to a return URL, update the view to
    /// prompt for registration, or display account lockout information.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task OnFirstAfterRenderAsync()
    {
        var e = new ExternalProviderProcessingEventArgs();

        if (OnProcessing.HasDelegate)
        {
            await OnProcessing.InvokeAsync(e);

            if (e.IsSuccessful)
            {
                NavigationManager.NavigateTo(State.ReturnUrl ?? "/");
            }
            else
            {
                _isProcessing = false;

                switch (e.FailReason)
                {
                    case ExternalProviderProcessingFailReason.LoginInfoUnavailable:
                        await Parent!.SetViewAsync(AccountManagerView.Login);

                        break;

                    case ExternalProviderProcessingFailReason.LockedOut:
                        await Parent!.SetViewAsync(AccountManagerView.AccountLocked);

                        break;

                    case ExternalProviderProcessingFailReason.AskForRegister:
                        _mustRegister = true;
                        _model.Email = e.Email ?? string.Empty;
                        await InvokeAsync(StateHasChanged);

                        break;
                }
            }
        }
        else
        {
            _isProcessing = false;
        }
    }
}

