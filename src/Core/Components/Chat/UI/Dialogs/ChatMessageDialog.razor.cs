using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// Displays a chat message in the user interface.
/// </summary>
public partial class ChatMessageDialog
{
    /// <summary>
    /// Gets or sets the dynamic state for the chat message, which includes read states, reactions, and files.
    /// </summary>
    [Inject]
    private ChatMessageDynamicState DynamicState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state for the chat, which includes information about the current room and user.
    /// </summary>
    [Inject]
    private ChatState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the message to be displayed in the viewer.
    /// </summary>
    [Parameter]
    public ChatMessage Message { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether to show the controls for the slideshow.
    /// </summary>
    [Parameter]
    public bool ShowControls { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to show the indicators for the slideshow.
    /// </summary>
    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        await DialogInstance.CloseAsync();
    }

    private IReadOnlyList<IChatFile> GetFiles()
    {
        if (State.Room is null)
        {
            return [];
        }

        return DynamicState.GetFiles(State.Room, Message.Id);
    }
}
