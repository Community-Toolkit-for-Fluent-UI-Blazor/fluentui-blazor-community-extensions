using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays an account locked message and provides options for user login.
/// </summary>
/// <remarks>Use this component to inform users that their account is locked and to offer a way to initiate the
/// login process. The component can be customized with specific labels and a callback for login actions.</remarks>
public partial class AccountLocked
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
            throw new InvalidOperationException($"{nameof(AccountLocked)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
    }
}
