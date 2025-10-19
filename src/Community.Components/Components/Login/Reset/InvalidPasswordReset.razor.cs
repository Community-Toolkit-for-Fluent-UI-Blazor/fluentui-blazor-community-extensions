using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the component displayed when a password reset attempt fails due to an invalid or expired reset code.
/// </summary>
public partial class InvalidPasswordReset
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
            throw new InvalidOperationException($"{nameof(InvalidPasswordReset)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
    }
}
