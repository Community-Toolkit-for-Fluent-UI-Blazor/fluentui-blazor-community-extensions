using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the reset password form component, providing functionality and customization options for resetting a
/// user's password.
/// </summary>
/// <remarks>Use this component to present a password reset form in your application. The component supports
/// customization of UI labels and handles the necessary data for password reset operations.</remarks>
public partial class ResetPassword
{
    /// <summary>
    /// Represents the model for the reset password form.
    /// </summary>
    private readonly ResetPasswordModel _model = new();

    /// <summary>
    /// Represents the list of error messages to be displayed on the form.
    /// </summary>
    private readonly List<string> _errors = [];

    /// <summary>
    /// Value indicating whether the form is currently being processed.
    /// </summary>
    private bool _isProcessing;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component in the component hierarchy.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    [SupplyParameterFromQuery]
    private string? Code { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a reset password event occurs.
    /// </summary>
    /// <remarks>Use this property to handle reset password actions initiated by the user. The callback
    /// receives a <see cref="ResetPasswordEventArgs"/> instance containing details about the reset request. Assign a
    /// delegate to perform custom logic, such as validating input or updating user credentials, when the event is
    /// triggered.</remarks>
    [Parameter]
    public EventCallback<ResetPasswordEventArgs> OnResetPassword { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(ResetPassword)} must be used within a {nameof(FluentCxLogin)} component.");
        }

        if (string.IsNullOrEmpty(Code))
        {
            await Parent.SetViewAsync(AccountManagerView.InvalidPasswordReset);
        }
        else
        {
            _model.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
        }
    }

    /// <summary>
    /// Handles the asynchronous reset password operation, raising the appropriate events based on the outcome.
    /// </summary>
    /// <remarks>This method clears any existing errors, initiates the reset password process, and updates the
    /// processing state. It raises the reset password event and, depending on the result, either updates the error
    /// collection or signals a successful reset. This method is typically invoked in response to a user action, such as
    /// submitting a reset password form.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnResetPasswordAsync()
    {
        _errors.Clear();
        _isProcessing = true;
        await InvokeAsync(StateHasChanged);

        var e = new ResetPasswordEventArgs(_model.Email, _model.Code, _model.Password, _model.ConfirmPassword);

        if (OnResetPassword.HasDelegate)
        {
            await OnResetPassword.InvokeAsync(e);
        }

        _isProcessing = false;

        if (!e.IsSuccessful &&
            e.Errors.Count != 0)
        {
            _errors.AddRange(e.Errors);
        }
        else
        {
            await Parent!.SetViewAsync(AccountManagerView.ResetPasswordConfirmation);
        }
    }
}
