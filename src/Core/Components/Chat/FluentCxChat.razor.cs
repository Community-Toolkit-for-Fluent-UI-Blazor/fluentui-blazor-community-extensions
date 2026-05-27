using System.Linq.Expressions;
using FluentUI.Blazor.Community.Components.Chat;
using FluentUI.Blazor.Community.Components.Chat.Engine;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.UI.Writers;
using FluentUI.Blazor.Community.Components.Components.Chat;
using FluentUI.Blazor.Community.Components.Emojis;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the complete chat.
/// </summary>
public partial class FluentCxChat<TFile>
    : FluentComponentBase where TFile : class, new()
{
    private readonly RenderFragment _renderChatRoom;

    /// <summary>
    /// Gets or sets the chat engine that manages the state and behavior of the chat component.
    /// </summary>
    [Inject]
    private ChatEngine ChatEngine { get; set; } = default!;

    /// <summary>
    /// Gets or sets the owner of the chat.
    /// </summary>
    [Parameter]
    public required ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the capabilities of the chat room, such as whether it can be blocked, deleted, or hidden.
    /// </summary>
    [Parameter]
    public ChatRoomCapabilities Capabilities { get; set; } = ChatRoomCapabilities.All;

    /// <summary>
    /// Gets or sets the template for rendering a chat room. The template receives a <see cref="ChatRoom"/> as its context, allowing you to customize the appearance and behavior of each chat room in the chat component.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatRoom>? RoomTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template for rendering the content when a chat room is empty.
    /// </summary>
    [Parameter]
    public RenderFragment? RoomEmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the template for rendering the content when a chat room is loading.
    /// </summary>
    [Parameter]
    public RenderFragment? RoomLoadingContent { get; set; }

    /// <summary>
    /// Gets or sets the provider for chat room items.
    /// </summary>
    [Parameter]
    public required ChatRoomItemsProvider? RoomItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider for the last message in a chat room.
    /// </summary>
    [Parameter]
    public required ChatLastMessageProvider? LastMessageProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider for chat room users.
    /// </summary>
    [Parameter]
    public required ChatRoomUsersProvider? RoomUsersProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider for unread messages in a chat room.
    /// </summary>
    [Parameter]
    public required ChatUnreadMessagesProvider? UnreadMessagesProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider for searching chat rooms.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, CancellationToken, Task<IEnumerable<ChatRoom>>>? RoomSearchProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider for searching chat users.
    /// </summary>
    [Parameter]
    public required Func<string?, StringComparison, CancellationToken, Task<IEnumerable<ChatUser>>>? UserSearchProvider { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the archive state of a chat room changes.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomArchiveChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the block state of a chat room changes.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomBlockChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the hide state of a chat room changes.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomHideChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the mute state of a chat room changes.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomMuteChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the pin state of a chat room changes.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomPinChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when a delete action is performed on a chat room.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomDelete { get; set; }

    /// <summary>
    /// Gets or sets the delegate that is invoked when a new chat room is created.
    /// </summary>
    [Parameter]
    public ChatRoomCreateDelegate? OnRoomCreate { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the rename action of a chat room is performed.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRoomRename { get; set; }

    /// <summary>
    /// Gets or sets the string comparison method used for comparing chat room names.
    /// </summary>
    [Parameter]
    public StringComparison RoomNameComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Gets or sets the string comparison method used for comparing chat user names.
    /// </summary>
    [Parameter]
    public StringComparison UserNameComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Gets or sets the <see cref="RenderFragment"/> for the loading content.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RenderFragment{ChatFileEventArgs}"/> for a file in the chat message list view.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatFileEventArgs>? FileTemplate { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RenderFragment"/> for an empty room content.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyRoomContent { get; set; }

    /// <summary>
    /// Gets or sets the template for a message.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatMessage>? MessageTemplate { get; set; }

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
    /// Gets or sets the settings for the emoji dialog.
    /// </summary>
    [Parameter]
    public EmojiSettings EmojiSettings { get; set; } = new();

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
    /// Gets or sets the options for the import of a media file.
    /// </summary>
    [Parameter]
    public ChatFileImportOptions ImportOptions { get; set; } = ChatFileImportOptions.Both;

    /// <summary>
    /// Gets or sets the callback to send a gift.
    /// </summary>
    [Parameter]
    public EventCallback OnGift { get; set; }

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
    public Expression<Func<ChatMessage, bool>>? Filter { get; set; }

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
    /// Gets or sets the icon to be used for the media button in the chat message writer.
    /// </summary>
    [Parameter]
    public ChatViewIcons? ChatViewIcons { get; set; } = new ChatViewIcons();

    /// <summary>
    /// Gets or sets the rendering mode of the chat files.
    /// </summary>
    [Parameter]
    public ChatFileRenderingMode ChatFileRenderingMode { get; set; } = ChatFileRenderingMode.Badge;

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
    /// Gets or sets a value indicating if the <see cref="ChatMessageWriter"/> is visible or not.
    /// </summary>
    [Parameter]
    public bool IsMessageWriterVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a provider to retrieve a specific chat message.
    /// </summary>
    [Parameter]
    public ChatMessageItemCollectionProvider? ItemRetrieveProvider { get; set; }

    /// <summary>
    /// Gets or sets a provider to retrieve a chat files.
    /// </summary>
    [Parameter]
    public ChatMessageFileCollectionProvider? FilesProvider { get; set; }

    /// <summary>
    /// Gets or sets a provider to retrieve reactions for a chat message.
    /// </summary>
    [Parameter]
    public ChatMessageReactionCollectionProvider? ReactionsProvider { get; set; }

    /// <summary>
    /// Gets or sets a provider to retrieve the chat messages to render in the view.
    /// </summary>
    [Parameter]
    public ChatMessageItemsProvider? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider to retrieve the total count of messages.
    /// </summary>
    [Parameter]
    public ChatMessageCountProvider? CountProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider to retrieve the user state of the messages.
    /// </summary>
    [Parameter]
    public ChatMessageUserStateProvider? ReadUserStateProvider { get; set; }

    /// <summary>
    /// Gets or sets the callback which is invoked when a message must be deleted.
    /// </summary>
    [Parameter]
    public ChatMessageDeleteMessageProvider? OnDelete { get; set; }

    /// <summary>
    /// Gets or sets the callback which is invoked when a message must be pinned or unpinned.
    /// </summary>
    [Parameter]
    public EventCallback<PinMessageEventArgs> OnPinOrUnpin { get; set; }

    /// <summary>
    /// Gets or sets the provider used to create chat message items.
    /// </summary>
    [Parameter]
    public ChatMessageItemsCreationProvider? OnCreate { get; set; }

    /// <summary>
    /// Gets or sets the provider used to edit chat message.
    /// </summary>
    [Parameter]
    public ChatMessageEditProvider? OnEditMessage { get; set; }

    /// <summary>
    /// Gets or sets the provider used to react to a message.
    /// </summary>
    [Parameter]
    public ChatMessageReactProvider? OnReactMessage { get; set; }

    /// <summary>
    /// Gets the css class for the chat component.
    /// </summary>
    private string? InternalCss => DefaultClassBuilder
        .AddClass("fluentcx-chat")
        .Build();

    /// <summary>
    /// Gets the style for the chat component to make the splitter bars transparent.
    /// </summary>
    private static string? TransparentBarStyle => new StyleBuilder()
        .AddStyle("--fluent-multi-splitter-background-color", "transparent")
        .AddStyle("--fluent-multi-splitter-background-color-active", "transparent")
        .AddStyle("--fluent-multi-splitter-hover-opacity", "0")
        .Build();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ChatEngine.ConnectAsync();
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await ChatEngine.DisconnectAsync();
        await base.DisposeAsync();
    }
}
