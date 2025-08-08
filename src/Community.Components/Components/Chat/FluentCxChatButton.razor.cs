using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the chat button.
/// </summary>
public partial class FluentCxChatButton
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the callback when the button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnChat { get; set; }

    /// <summary>
    /// Occurs when the button is clicked.
    /// </summary>
    /// <returns>Returns a task which raises the <see cref="OnChat"/> callback when completed.</returns>
    private async Task OnChatAsync()
    {
        if (OnChat.HasDelegate)
        {
            await OnChat.InvokeAsync();
        }
    }
}
