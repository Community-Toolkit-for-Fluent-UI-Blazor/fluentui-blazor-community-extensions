using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the confirmation component displayed after a user initiates a password reset request.
/// </summary>
public partial class ForgotPasswordConfirmation
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the parent login component that contains this confirmation component.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the resend email action is triggered.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnResendEmail { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the component.
    /// </summary>
    [Parameter]
    public string? Email { get; set; }

    /// <summary>
    /// Invokes the resend email callback asynchronously if a valid email address is available.
    /// </summary>
    /// <remarks>The callback is only invoked if the email address is not null, empty, or whitespace, and if a
    /// delegate is assigned to handle the resend email action.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnResendEmailAsync()
    {
        if (OnResendEmail.HasDelegate &&
            !string.IsNullOrWhiteSpace(Email))
        {
            await OnResendEmail.InvokeAsync(Email);
        }
    }

    /// <summary>
    /// Handles the login action by updating the parent view to display the login interface asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginAsync()
    {
        if (Parent is not null)
        {
            await Parent.SetViewAsync(AccountManagerView.Login);
        }
    }
}
