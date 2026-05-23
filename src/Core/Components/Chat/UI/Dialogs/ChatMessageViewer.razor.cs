using FluentUI.Blazor.Community.Components.Chat.Messages;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// Displays a chat message in the user interface.
/// </summary>
public partial class ChatMessageViewer
{
    /// <summary>
    /// Gets or sets the message to be displayed in the viewer.
    /// </summary>
    [Parameter]
    public IChatMessage Message { get; set; } = default!;
}
