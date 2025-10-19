using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays an account disabled message and provides options for user login.
/// </summary>
/// <remarks>Use this class to present users with information and actions when their account is disabled, such as
/// displaying custom labels and handling login attempts. The component can be customized via the provided
/// parameters.</remarks>
public partial class AccountDisabled
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
            throw new InvalidOperationException($"{nameof(AccountDisabled)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
    }
}
