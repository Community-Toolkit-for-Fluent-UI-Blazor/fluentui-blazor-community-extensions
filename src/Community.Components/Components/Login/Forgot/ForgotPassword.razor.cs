using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view component that provides the user interface and logic for the 'Forgot Password' workflow in a login
/// form.
/// </summary>
/// <remarks>Use this component to display and manage the password recovery process within an authentication UI.
/// The component exposes parameters to customize labels and handle login or sign-up actions. It is typically used as
/// part of a larger authentication or account management system.</remarks>
public partial class ForgotPassword
    : FluentComponentBase
{
    /// <summary>
    /// Represents the data model for the forgot password form, including the email address field.
    /// </summary>
    private readonly ForgotPasswordModel _model = new();

    /// <summary>
    /// Represents the FluentEditForm instance used for form validation and submission.
    /// </summary>
    private FluentEditForm? _fluentEditForm;

    /// <summary>
    /// Represents an error message to be displayed in the forgot password view.
    /// </summary>
    private string? _errorMessage;

    /// <summary>
    /// Represents whether the component is currently processing an operation, such as sending instructions.
    /// </summary>
    private bool _isProcessing;

    /// <summary>
    /// Gets or sets the callback that is invoked when instructions to send a password are triggered.
    /// </summary>
    /// <remarks>Use this event to handle custom logic when password sending instructions need to be
    /// processed, such as sending an email or displaying a notification. The event provides details about the send
    /// operation through the <see cref="SendPasswordEventArgs"/> parameter.</remarks>
    [Parameter]
    public EventCallback<SendPasswordEventArgs> OnSendInstructions { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when instructions have been sent.
    /// </summary>
    /// <remarks>Use this event to execute custom logic after the instructions are successfully sent. The
    /// callback is triggered in response to the relevant user action or process completion.</remarks>
    [Parameter]
    public EventCallback<string> OnInstructionsSent { get; set; }

    /// <summary>
    /// Gets the value for the 'aria-disabled' attribute based on the current processing state.
    /// </summary>
    private string AriaDisabled => _isProcessing ? "true" : "false";

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is set automatically by the Blazor framework when the component is used within
    /// a <see cref="FluentCxLogin"/> context. It should not be set manually.</remarks>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current operation is disabled due to ongoing processing or validation
    /// errors.
    /// </summary>
    /// <remarks>The property returns <see langword="true"/> if either a process is currently active or if
    /// there are validation messages present in the associated edit form. This can be used to control UI elements such
    /// as disabling buttons or inputs when user interaction should be prevented.</remarks>
    private bool IsDisabled => _isProcessing || _fluentEditForm?.EditContext?.GetValidationMessages().Any() == true;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(ForgotPassword)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <summary>
    /// Invokes the login event asynchronously if it is not already being processed.
    /// </summary>
    /// <remarks>If a login operation is already in progress, this method returns immediately without invoking
    /// the login event.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginAsync()
    {
        if (_isProcessing)
        {
            return;
        }

        await Parent!.SetViewAsync(AccountManagerView.Login);
    }

    /// <summary>
    /// Handles the sign-up action asynchronously by invoking the associated sign-up event if it is assigned.
    /// </summary>
    /// <remarks>If a sign-up operation is already in progress, this method returns immediately without
    /// invoking the event. This method should be awaited to ensure that the sign-up process completes before
    /// proceeding.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSignUpAsync()
    {
        if (_isProcessing)
        {
            return;
        }

        await Parent!.SetViewAsync(AccountManagerView.Register);
    }

    /// <summary>
    /// Handles the process of sending password reset instructions asynchronously based on the current model state and
    /// event handlers.
    /// </summary>
    /// <remarks>This method checks for the presence of required event handlers and a valid email address
    /// before attempting to send instructions. Depending on the outcome, it invokes additional event handlers to signal
    /// success or specific failure reasons. This method is typically used within a component to coordinate password
    /// reset workflows.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSendInstructionsAsync()
    {
        if (OnSendInstructions.HasDelegate &&
            !string.IsNullOrEmpty(_model.Email))
        {
            _isProcessing = true;
            await InvokeAsync(StateHasChanged);

            var e = new SendPasswordEventArgs(_model.Email!);
            await OnSendInstructions.InvokeAsync(e);

            _isProcessing = false;

            if (e.Successful &&
                OnInstructionsSent.HasDelegate)
            {
                await OnInstructionsSent.InvokeAsync(e.Email);
            }
            else
            {
                switch (e.FailReason)
                {
                    case SendPasswordFailReason.EmailNotFound:
                        {
                            _errorMessage = string.Format(CultureInfo.CurrentCulture,
                                Parent!.Labels.EmailNotFoundErrorMessage,
                                _model.Email);

                            await InvokeAsync(StateHasChanged);
                        }

                        break;

                    case SendPasswordFailReason.NoServerResponse:
                        {
                            _errorMessage = Parent!.Labels.NoServerResponseErrorMessage;

                            await InvokeAsync(StateHasChanged);
                        }

                        break;
                }
            }
        }
    }
}
