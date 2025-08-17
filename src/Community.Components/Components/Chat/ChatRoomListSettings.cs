using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public class ChatRoomListSettings: FluentComponentBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the new group chat feature is enabled.
    /// </summary>
    [Parameter]
    public bool IsNewGroupChatEnabled { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a new chat group is created.
    /// </summary>
    [Parameter]
    public EventCallback<ChatGroupEventArgs> OnNewChatGroup { get; set; }

    /// <summary>
    /// Gets or sets the item template fragment to render a chat room option.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatRoom>? RoomItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the items provider for fetching chat rooms.
    /// </summary>
    [Parameter]
    public ChatRoomItemsProvider? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the item template fragment to render a chat room option.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatRoom>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the function to search for users in the chat room.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, Task<IEnumerable<ChatUser>>>? UserSearchFunction { get; set; }

    /// <summary>
    /// Gets or sets the function to search for chat rooms.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, Task<IEnumerable<ChatRoom>>>? RoomSearchFunction { get; set; }

    /// <summary>
    /// Gets or sets the string comparison to compare the name of the room.
    /// </summary>
    [Parameter]
    public StringComparison RoomNameComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Gets or sets the string comparison to compare the username.
    /// </summary>
    [Parameter]
    public StringComparison UsernameComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be deleted.
    /// </summary>
    [Parameter]
    public bool CanDelete { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be hidden.
    /// </summary>
    [Parameter]
    public bool CanHide { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be unhide.
    /// </summary>
    [Parameter]
    public bool CanUnhide { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be blocked.
    /// </summary>
    [Parameter]
    public bool CanBlock { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be renamed.
    /// </summary>
    [Parameter]
    public bool CanRename { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is deleted.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnDelete { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is blocked.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnBlock { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is unblocked.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnUnblock { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is hidden.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnHide { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is unhide.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnUnhide { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is renamed.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnRename { get; set; }

    /// <summary>
    /// Gets or sets the content to display while the chat rooms are loading.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the search functionality is enabled for the chat room list.
    /// </summary>
    [Parameter]
    public bool IsSearchEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the unblock functionality is enabled for the chat room list.
    /// </summary>
    [Parameter]
    public bool CanUnblock { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show deleted rooms in the chat room list.
    /// </summary>
    [Parameter]
    public bool ShowDeletedRoom { get; set; }
}
