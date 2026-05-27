using System.Globalization;
using FluentUI.Blazor.Community.Components.Chat.EventArgs;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Rooms;

/// <summary>
/// Represents an item in a chat room list, which can be selected or disabled.
/// It provides visual feedback based on its state and allows for interaction through click events.
/// </summary>
public partial class ChatRoomItem : FluentComponentBase
{
    /// <summary>
    /// Represents the icon used to indicate a pinned chat room in the list view.
    /// </summary>
    private static readonly Icon s_Pin = new Size16.Pin().WithColor(Color.Primary);

    /// <summary>
    /// Represents the icon used to indicate a muted chat room in the list view.
    /// </summary>
    private static readonly Icon s_Mute = new Size16.SpeakerMute().WithColor(Color.Error);

    /// <summary>
    /// Value  indicating whether the mouse is hovering over the component.
    /// </summary>
    private bool _hover;

    /// <summary>
    /// Represents the list of users in the chat room.
    /// </summary>
    private IReadOnlyList<ChatUser> _users = [];

    /// <summary>
    /// Represents the count of unread messages in the chat room for the current user.
    /// </summary>
    private int _unreadMessageCount;

    /// <summary>
    /// Represents the last message in the chat room.
    /// </summary>
    private ChatMessage? _lastMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomItem"/> class with the specified library configuration and
    /// generates a unique identifier.
    /// </summary>
    /// <param name="configuration">The library configuration to use for initializing the chat room item.</param>
    public ChatRoomItem(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets a value indicating whether to show the "more" button for additional actions on the chat room item.
    /// </summary>
    [Parameter]
    public bool ShowMoreButton { get; set; }

    /// <summary>
    /// Gets or sets the owner of the room.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the chat room associated with this item.
    /// </summary>
    [Parameter]
    public ChatRoom? Room { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is selected.
    /// </summary>
    [Parameter]
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled.
    /// </summary>
    private bool IsDisabled => Room?.IsBlocked == true && Owner == Room?.Owner;

    /// <summary>
    /// Gets or sets an event callback that is invoked when the component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets the collection of actions that can be performed on the chat room item.
    /// </summary>
    [Parameter]
    public IEnumerable<ChatRoomAction> MoreMenuActions { get; set; } = [];

    /// <summary>
    /// Gets or sets the function that provides the list of users in the chat room based on the room ID.
    /// </summary>
    [Inject]
    private ChatRoomDynamicState DynamicState { get; set; } = null!;
    
    /// <summary>
    /// Gets the css for the item.
    /// </summary>
    private string? CssClass => DefaultClassBuilder
        .AddClass("chat-room-item")
        .AddClass("selected", IsSelected)
        .AddClass("disabled", IsDisabled)
        .AddClass("hover", _hover && !IsDisabled)
        .Build();

    /// <summary>
    /// Handles the click event on the component.
    /// </summary>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    private async Task HandleClick()
    {
        if (IsDisabled)
        {
            return;
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Room is null)
        {
            throw new InvalidOperationException("Room is required for ChatRoomItem.");
        }

        if (Owner is null)
        {
            throw new InvalidOperationException("Owner is required for ChatRoomItem.");
        }

        _users = DynamicState.GetUsers(Room.Id);
        _lastMessage = DynamicState.GetLastMessage(Room.Id);
        _unreadMessageCount = DynamicState.GetUnreadCount(Room.Id);

        DynamicState.UsersUpdated += OnUsersUpdated;
        DynamicState.LastMessageUpdated += OnLastMessageUpdated;
        DynamicState.UnreadCountUpdated += OnUnreadCountUpdated;
    }

    /// <summary>
    /// Occurs when the unread message count for the chat room is updated, allowing the component to refresh its display accordingly.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments containing the updated unread message count.</param>
    private void OnUnreadCountUpdated(object? sender, UnreadCountUpdatedEventArgs e)
    {
        if (Room?.Id != e.RoomId)
        {
            return;
        }

        _unreadMessageCount = e.Count;
        StateHasChanged();
    }

    /// <summary>
    /// Occurs when the last message in the chat room is updated, allowing the component to refresh its display with the new message preview.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments containing the updated last message.</param>
    private void OnLastMessageUpdated(object? sender, LastMessageUpdatedEventArgs e)
    {
        if (Room?.Id != e.RoomId)
        {
            return;
        }

        _lastMessage = e.Message;
        StateHasChanged();
    }

    /// <summary>
    /// Occurs when the list of users in the chat room is updated, allowing the component to refresh its display with the new user information.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments containing the updated list of users.</param>
    private void OnUsersUpdated(object? sender, UsersUpdatedEventArgs e)
    {
        if (Room?.Id != e.RoomId)
        {
            return;
        }

        _users = e.Users;
        StateHasChanged();
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        DynamicState.UsersUpdated -= OnUsersUpdated;
        DynamicState.LastMessageUpdated -= OnLastMessageUpdated;
        DynamicState.UnreadCountUpdated -= OnUnreadCountUpdated;

        return base.DisposeAsync();
    }

    /// <summary>
    /// Gets the preview text for a chat room, displaying unread message count, empty room message, or the last message.
    /// </summary>
    /// <param name="room">The chat room to generate a preview for.</param>
    /// <returns>A markup string containing the room preview text with appropriate localization and formatting.</returns>
    private MarkupString GetRoomPreview(ChatRoom room)
    {
        if (Owner is null)
        {
            return new MarkupString(string.Empty);
        }

        if (_unreadMessageCount >= 1)
        {
            var text = _unreadMessageCount == 1
                ? Localizer[LanguageResource.CX_Chat_Room_UnreadSingular, _unreadMessageCount]
                : Localizer[LanguageResource.CX_Chat_Room_UnreadPlural, _unreadMessageCount];

            return new MarkupString(text);
        }

        if (room.IsEmpty)
        {
            return new MarkupString(Localizer[LanguageResource.CX_Chat_Room_EmptyRoomMessage]);
        }

        if (_lastMessage is not null)
        {
            return new MarkupString($"<b>{Format(_lastMessage)}</b>");
        }

        return new MarkupString(string.Empty);
    }

    /// <summary>
    /// Formats the chat message for display in the chat room list view.
    /// </summary>
    /// <param name="message">Message to format.</param>
    /// <returns>The formatted message into a <see cref="MarkupString"/>.</returns>
    private MarkupString Format(ChatMessage message)
    {
        if (message.Sections.Count == 0)
        {
            return new(string.Empty);
        }

        switch (message.Type)
        {
            //case ChatMessageType.Audio:
            //    {
            //        var section = message.Sections[0];
            //        int count = section?.Content?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length ?? 0;

            //        if (count > 0)
            //        {
            //            if (message.Sender?.Id == Owner?.Id)
            //            {
            //                string text = string.Format(count <= 1 ? ChatRoomLabels.AudioSenderSingular : ChatRoomLabels.AudioSenderPlural, count);

            //                return new(text);
            //            }
            //            else
            //            {
            //                string text = count <= 1 ? string.Format(ChatRoomLabels.AudioReceiverSingular, message.Sender?.DisplayName) :
            //                                           string.Format(ChatRoomLabels.AudioReceiverPlural, message.Sender?.DisplayName, count);

            //                return new(text);
            //            }
            //        }

            //        return new(string.Empty);
            //    }

            //case ChatMessageType.Video:
            //    {
            //        var section = message.Sections[0];
            //        int count = section?.Content?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length ?? 0;

            //        if (count > 0)
            //        {
            //            if (message.Sender?.Id == Owner?.Id)
            //            {
            //                string text = string.Format(count <= 1 ? ChatRoomLabels.VideoSenderSingular : ChatRoomLabels.VideoSenderPlural, count);

            //                return new(text);
            //            }
            //            else
            //            {
            //                string text = count <= 1 ? string.Format(ChatRoomLabels.VideoReceiverSingular, message.Sender?.DisplayName) :
            //                                           string.Format(ChatRoomLabels.VideoReceiverPlural, message.Sender?.DisplayName, count);

            //                return new(text);
            //            }
            //        }

            //        return new(string.Empty);
            //    }

            //case ChatMessageType.Media:
            //    {
            //        var section = message.Sections[0];
            //        int count = section?.Content?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length ?? 0;

            //        if (count > 0)
            //        {
            //            if (message.Sender?.Id == Owner?.Id)
            //            {
            //                string text = string.Format(count <= 1 ? ChatRoomLabels.MediaSenderSingular : ChatRoomLabels.MediaSenderPlural, count);

            //                return new(text);
            //            }
            //            else
            //            {
            //                string text = count <= 1 ? string.Format(ChatRoomLabels.MediaReceiverSingular, message.Sender?.DisplayName) :
            //                                           string.Format(ChatRoomLabels.MediaReceiverPlural, message.Sender?.DisplayName, count);

            //                return new(text);
            //            }
            //        }

            //        return new(string.Empty);
            //    }

            //case ChatMessageType.Photo:
            //    {
            //        var section = message.Sections[0];
            //        int count = section?.Content?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length ?? 0;

            //        if (count > 0)
            //        {
            //            if (message.Sender?.Id == Owner?.Id)
            //            {
            //                string text = string.Format(count <= 1 ? ChatRoomLabels.PhotoSenderSingular : ChatRoomLabels.PhotoSenderPlural, count);

            //                return new(text);
            //            }
            //            else
            //            {
            //                string text = count <= 1 ? string.Format(ChatRoomLabels.PhotoReceiverSingular, message.Sender?.DisplayName) :
            //                                           string.Format(ChatRoomLabels.PhotoReceiverPlural, message.Sender?.DisplayName, count);

            //                return new(text);
            //            }
            //        }

            //        return new(string.Empty);
            //    }

            case ChatMessageType.Gift:
                {
                    if (message.Sender?.Id == Owner?.Id)
                    {
                        return new(Localizer[LanguageResource.CX_Chat_Room_GiftSender]);
                    }
                    else
                    {
                        return new(string.Format(CultureInfo.CurrentCulture, Localizer[LanguageResource.CX_Chat_Room_GiftReceiver], message.Sender?.DisplayName));
                    }
                }

            case ChatMessageType.Text:
                {
                    var section = message.Sections.FirstOrDefault(x => x.CultureId == Owner?.CultureId);

                    if (section is not null && !string.IsNullOrEmpty(section.Content))
                    {
                        return new(section.Content);
                    }

                    return new();
                }

            default:
                return new();
        }
    }
}
