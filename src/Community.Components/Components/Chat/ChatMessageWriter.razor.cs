using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chat message writer component.
/// </summary>
public partial class ChatMessageWriter
{
    /// <summary>
    /// Represents the fragment to render the action toolbar of the chat message writer.
    /// </summary>
    private readonly RenderFragment<(Orientation, string)> _toolbarFragment;

    /// <summary>
    /// Gets or sets the icon to be used for the media button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MediaIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon to be used for the emoji button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? EmojiIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon to be used for the gift button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? GiftIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon to be used for the send button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? SendIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon to be used for the cancel edit button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? DismissIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Dismiss();

    /// <summary>
    /// Gets or sets the icon to be used for the commit edit button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? CheckmarkIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Checkmark();

    /// <summary>
    /// Gets or sets the content to be displayed in the chat message writer when a message is currently sending.
    /// </summary>
    [Parameter]
    public RenderFragment? SendingContent { get; set; }

    /// <summary>
    /// Gets or sets the render mode for sending messages in the chat message writer.
    /// </summary>
    [Parameter]
    public ChatMessageSendingRenderMode SendingRenderMode { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the toolbar in the chat message writer.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets a value indicating if the message writer is in reply mode.
    /// </summary>
    [Parameter]
    public bool IsReply { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the message writer is in edit mode.
    /// </summary>
    [Parameter]
    public bool IsEdit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the message writer is in mobile view.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the emoji button.
    /// </summary>
    [Parameter]
    public bool ShowEmojiButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the media button.
    /// </summary>
    [Parameter]
    public bool ShowMediaButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the gift button.
    /// </summary>
    [Parameter]
    public bool ShowGiftButton { get; set; } = true;

    /// <summary>
    /// Gets or sets the draft message to be used in the writer.
    /// </summary>
    [Parameter]
    public ChatMessageDraft? Draft { get; set; }

    /// <summary>
    /// Gets or sets the labels to be used in the chat message writer.
    /// </summary>
    [Parameter]
    public ChatMessageListLabels Labels { get; set; } = ChatMessageListLabels.Default;

    /// <summary>
    /// Gets or sets the owner of the chat message writer.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the reply message is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the media button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnImportMedia { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the emoji button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnEmoji { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the gift button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnGift { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the send button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnSendMessage { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the cancel button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnCancel { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the edit button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnEditMessage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message writer is currently sending a message.
    /// </summary>
    [Parameter]
    public bool IsSending { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message writer should show a counter badge.
    /// </summary>
    [Parameter]
    public bool ShowCounterBadge { get; set; } = true;

    /// <summary>
    /// Gets the number of rows to display in the chat message writer based on the current state (mobile, reply, etc.).
    /// </summary>
    /// <returns>Returns the number of rows to display.</returns>
    private int GetNumberOfRows()
    {
        if (IsMobile)
        {
            return IsReply ? 2 : 4;
        }

        return IsReply ? 5 : 7;
    }
}
