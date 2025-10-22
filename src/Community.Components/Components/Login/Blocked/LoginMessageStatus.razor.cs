using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays an specific error message and provides options for user login.
/// </summary>
public partial class LoginMessageStatus
{
    /// <summary>
    /// Gets or sets the callback for when the user clicks the login button.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin Parent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the message to be displayed to the user.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the title of the message.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the icon to be displayed alongside the message.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size48.Prohibited();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(LoginMessageStatus)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
    }
}
