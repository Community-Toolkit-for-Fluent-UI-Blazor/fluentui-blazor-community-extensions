using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public class ChatMessageListViewSettings<TItem>
    : FluentComponentBase where TItem : class, new()
{
    /// <summary>
    /// Gets or sets the <see cref="RenderFragment"/> for the loading content.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RenderFragment"/> for an empty room content.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyRoomContent { get; set; }

    /// <summary>
    /// Gets or sets the list of labels the view uses.
    /// </summary>
    [Parameter]
    public ChatMessageListLabels ChatMessageListLabels { get; set; } = new();

    /// <summary>
    /// Gets or sets the template for a message.
    /// </summary>
    [Parameter]
    public RenderFragment<IChatMessage>? MessageTemplate { get; set; }

    /// <summary>
    /// Gets or sets the size of an item.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 180;

    /// <summary>
    /// Gets or sets the maximum number of item the view renders.
    /// </summary>
    [Parameter]
    public int MaxItemCount { get; set; } = 30;

    /// <summary>
    /// Gets or sets the number of items the view renders after the viewport.
    /// </summary>
    [Parameter]
    public int OverscanCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the provider for the emojis.
    /// </summary>
    [Parameter]
    public GEmojiProviderDelegate? GEmojiItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the chat allows the insertion of medias.
    /// </summary>
    [Parameter]
    public bool IsMediaInsertionAllowed { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the chat allows the insertion of emojis.
    /// </summary>
    [Parameter]
    public bool IsEmojiInsertionAllowed { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the chat allows a gift to be send.
    /// </summary>
    /// <remarks>The gift must be implemented by the user.</remarks>
    [Parameter]
    public bool IsGiftAllowed { get; set; } = true;

    /// <summary>
    /// Gets or sets the render mode of the sending of a message.
    /// </summary>
    [Parameter]
    public ChatMessageSendingRenderMode SendingRenderMode { get; set; } = ChatMessageSendingRenderMode.Overlay;

    /// <summary>
    /// Gets or sets the orientation of the message writer in the chat message list view.
    /// </summary>
    [Parameter]
    public Orientation MessageWriterOrientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Gets or sets the callback to send a gift.
    /// </summary>
    [Parameter]
    public EventCallback OnGift { get; set; }

    /// <summary>
    /// Gets or sets the callback to import media.
    /// </summary>
    /// <remarks>If the callback isn't set, an internal import component is used.</remarks>
    [Parameter]
    public EventCallback<ChatMediaImporterEventArgs<TItem>> OnImportMedia { get; set; }

    /// <summary>
    /// Gets or sets the emoji selector.
    /// </summary>
    /// <remarks>If the callback isn't set, an internal component is used.</remarks>
    [Parameter]
    public RenderFragment? EmojiView { get; set; }

    /// <summary>
    /// Gets or sets the columns labels for the internal file importer component.
    /// </summary>
    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = FileListDataGridColumnLabels.Default;

    /// <summary>
    /// Gets or sets the file extension type labels for the internal file importer component.
    /// </summary>
    [Parameter]
    public FileExtensionTypeLabels FileExtensionTypeLabels { get; set; } = FileExtensionTypeLabels.Default;

    /// <summary>
    /// Gets or sets the details labels for the internal file importer component.
    /// </summary>
    [Parameter]
    public FileManagerDetailsLabels DetailsLabels { get; set; } = FileManagerDetailsLabels.Default;

    /// <summary>
    /// Gets or sets the details labels for the internal file importer component.
    /// </summary>
    [Parameter]
    public FileManagerLabels FileManagerLabels { get; set; } = FileManagerLabels.French;

    /// <summary>
    /// Gets or sets a value indicating if the files are appended when the user imports the file to send
    ///  in many steps.
    /// </summary>
    [Parameter]
    public bool AppendFiles { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the translation is enabled or not.
    /// </summary>
    [Parameter]
    public bool IsTranslationEnabled { get; set; }

    /// <summary>
    /// Gets or sets the filter to filter the messages.
    /// </summary>
    [Parameter]
    public Expression<Func<IChatMessage, bool>>? Filter { get; set; }

    /// <summary>
    /// Gets or sets the option to split the message into multiple parts or not.
    /// </summary>
    [Parameter]
    public ChatMessageSplitOption MessageSplitOption { get; set; }

    /// <summary>
    /// Gets or sets a custom sending content.
    /// </summary>
    /// <remarks>If not used, a default sending content is used.</remarks>
    [Parameter]
    public RenderFragment? SendingContent { get; set; }

    /// <summary>
    /// Gets or sets a template for a deleted message.
    /// </summary>
    [Parameter]
    public RenderFragment? DeletedMessageTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the deleted messages are shown or not.
    /// </summary>
    /// <remarks>
    /// If this value is set to <see langword="false"/>, no message is displayed.
    /// If this value is set to <see langword="true" />, a placeholder with a deleted message is shown.
    /// </remarks>
    [Parameter]
    public bool ShowDeletedMessages { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of the hub to use to send message.
    /// </summary>
    [Parameter]
    public string HubName { get; set; } = "/hubs/chat";

    /// <summary>
    /// Gets or sets the icon to be used for the media button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MediaIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Image();

    /// <summary>
    /// Gets or sets the icon to be used for the emoji button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? EmojiIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Emoji();

    /// <summary>
    /// Gets or sets the icon to be used for the gift button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? GiftIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Gift();

    /// <summary>
    /// Gets or sets the icon to be used for the send button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? SendIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Send();

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
    /// Gets or sets the icon to be used for the audio recorder button when not recording in chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MicroIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Mic();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when recording in chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MicroOffIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.MicOff();

    /// <summary>
    /// Gets or sets the rendering mode of the chat files.
    /// </summary>
    [Parameter]
    public ChatFileRenderingMode ChatFileRenderingMode { get; set; } = ChatFileRenderingMode.Discrete;

    /// <summary>
    /// Gets or sets the template to use to render a chat file in the chat message list view.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatFileEventArgs>? ChatFileTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template to use to render a chat file in the chat message list view when the file is loading.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingFileContent { get; set; }

    /// <summary>
    /// Gets or sets the template to use when the user records an audio message.
    /// </summary>
    [Parameter]
    public RenderFragment? AudioWaveVisualizerContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the chat allows the record of an audio message.
    /// </summary>
    [Parameter]
    public bool IsRecordingAudioEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the eventcallback to process the audio from the micro.
    /// </summary>
    [Parameter]
    public EventCallback<RecordedAudioEventArgs> ProcessAudio { get; set; }
}
