using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that handles user recovery code input with customizable labels.
/// </summary>
public partial class RecoveryCode
{
    /// <summary>
    /// Represents the model for recovery code input.
    /// </summary>
    private readonly RecoveryCodeModel _model = new();

    /// <summary>
    /// Represents the FluentEditForm instance for handling form submissions and validations.
    /// </summary>
    private FluentEditForm? _fluentEditForm;

    /// <summary>
    /// Represents the error message to be displayed for invalid input.
    /// </summary>
    private string? _errorMessage;

    /// <summary>
    /// Gets or sets the event callback that is invoked when a recovery code is submitted.
    /// </summary>
    [Parameter]
    public EventCallback<RecoveryCodeEventArgs> OnRecoveryCode { get; set; }

    /// <summary>
    /// Gets or sets the current login state for the component.
    /// </summary>
    [Inject]
    private AccountState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the navigation manager for handling URL navigation.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets a value indicating whether the form is currently disabled due to validation errors.
    /// </summary>
    private bool IsDisabled => _fluentEditForm?.EditContext?.GetValidationMessages().Any() ?? false;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(RecoveryCode)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <summary>
    /// Handles the submission of a valid recovery code form asynchronously, triggering recovery code processing and
    /// navigation based on the result.
    /// </summary>
    /// <remarks>If the recovery code is successfully processed, the user is navigated to the return URL or
    /// the home page. If the recovery code fails due to being locked out or invalid, appropriate events or error
    /// messages are triggered. This method should be called after form validation has succeeded.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnValidSubmitAsync()
    {
        var e = new RecoveryCodeEventArgs(_model.RecoveryCode!);

        await OnRecoveryCode.InvokeAsync(e);

        if (e.IsSuccessful)
        {
            NavigationManager.NavigateTo(State.ReturnUrl ?? "/");
        }
        else
        {
            switch (e.FailReason)
            {
                case RecoveryFailReason.LockedOut:
                    await Parent!.SetViewAsync(AccountManagerView.AccountLocked);

                    break;

                case RecoveryFailReason.InvalidCode:
                    _errorMessage = Parent!.Labels.InvalidRecoveryCodeMessage;
                    await InvokeAsync(StateHasChanged);

                    break;
            }
        }
    }
}
