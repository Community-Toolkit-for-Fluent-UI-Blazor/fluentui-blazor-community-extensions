using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that handles required two-factor authentication during the login process.
/// </summary>
/// <remarks>This class provides parameters for configuring two-factor authentication, including the return URL
/// after successful login, whether to remember the user, custom labels for UI elements, and a callback for login
/// events. It is typically used within authentication workflows to enforce two-factor verification before granting
/// access.</remarks>
public partial class RequiredTwoFactor
{
    /// <summary>
    /// Represents the model for two-factor authentication.
    /// </summary>
    private readonly TwoFactorModel _model = new();

    /// <summary>
    /// Represents an optional message to be displayed to the user, such as error messages or informational prompts.
    /// </summary>
    private string? _errorMessage;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component in the component hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a two-factor authentication login event occurs.
    /// </summary>
    /// <remarks>Use this property to handle two-factor authentication events, such as when a user submits a
    /// verification code. The event provides details about the two-factor login attempt through the <see
    /// cref="TwoFactorEventArgs"/> parameter.</remarks>
    [Parameter]
    public EventCallback<TwoFactorEventArgs> OnTwoFactorLogin { get; set; }

    /// <summary>
    /// Gets or sets the current login state for the component.
    /// </summary>
    [Inject]
    private AccountState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the NavigationManager used for managing URI navigation and location state within the application.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(RequiredTwoFactor)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    private async Task OnValidSubmitAsync()
    {
        var authenticatorCode = _model.TwoFactorCode?.Replace(" ", string.Empty) ?? string.Empty;
        var e = new TwoFactorEventArgs(authenticatorCode, _model.RememberMachine);

        if (OnTwoFactorLogin.HasDelegate)
        {
            await OnTwoFactorLogin.InvokeAsync(e);

            if (e.IsSuccessful)
            {
                NavigationManager.NavigateTo(State.ReturnUrl ?? "/");
            }
            else
            {
                switch(e.FailReason)
                {
                    case TwoFactorFailReason.InvalidCode:
                        _errorMessage = Parent!.Labels.InvalidTwoFactorCodeMessage;
                        break;

                    case TwoFactorFailReason.LockedOut:
                        if(Parent is not null)
                        {
                            await Parent.SetViewAsync(AccountManagerView.AccountLocked);
                        }

                        break;

                    default:
                        _errorMessage = Parent!.Labels.TwoFactorUnknownError;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Invokes the login process using a recovery code asynchronously if a handler is assigned.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginWithRecoveryCodeAsync()
    {
        if (Parent is not null)
        {
            await Parent.SetViewAsync(AccountManagerView.RecoveryCode);
        }
    }
}
