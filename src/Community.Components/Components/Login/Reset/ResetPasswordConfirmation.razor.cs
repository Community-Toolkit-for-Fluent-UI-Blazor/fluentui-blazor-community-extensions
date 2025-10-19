using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the confirmation view displayed after a user successfully resets their password.
/// </summary>
/// <remarks>This component is typically used in authentication workflows to inform users that their password
/// reset was successful. It can be customized via parameters to adjust labels and handle login actions. The component
/// is intended for use within login or account management pages.</remarks>
public partial class ResetPasswordConfirmation
{
    /// <summary>
    /// Gets or sets the callback for when the user clicks the login button.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin Parent { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(ResetPasswordConfirmation)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
    }
}
