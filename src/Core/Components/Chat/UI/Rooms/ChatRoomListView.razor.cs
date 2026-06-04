using System.Globalization;
using System.Linq.Expressions;
using FluentUI.Blazor.Community.Components.Chat;
using FluentUI.Blazor.Community.Components.Chat.Engine;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view for displaying a list of chat rooms. 
/// </summary>
public partial class ChatRoomListView
    : FluentComponentBase
{
    /// <summary>
    /// Represents the different views available for the chat room list.
    /// </summary>
    private enum ListView
    {
        Normal,
        Blocked,
        Hidden,
        Archived
    }

    /// <summary>
    /// Value indicating whether the owner of the chat room has changed, which is used to determine if the chat room list needs to be updated based on the new owner context.
    /// </summary>
    private bool _hasOwnerChanged;

    /// <summary>
    /// Represents the cancellation token source used for canceling ongoing operations when updating the chat room list.
    /// </summary>
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Represents the selected chat rooms.
    /// </summary>
    private List<ChatRoom> _selectedRooms = [];

    /// <summary>
    /// Represents all chat rooms.
    /// </summary>
    private readonly List<ChatRoom> _chatRooms = [];

    /// <summary>
    /// Represents the blocked chat rooms.
    /// </summary>
    private readonly List<ChatRoom> _blockedRooms = [];

    /// <summary>
    /// Represents the hidden chat rooms.
    /// </summary>
    private readonly List<ChatRoom> _hiddenRooms = [];

    /// <summary>
    /// Represents the archived chat rooms.
    /// </summary>
    private readonly List<ChatRoom> _archivedRooms = [];

    /// <summary>
    /// Represents the selected chat room.
    /// </summary>
    private string? _selectedRoom;

    /// <summary>
    /// Represents the list view type.
    /// </summary>
    private ListView _listView;

    /// <summary>
    /// Represents the virtualized component for rendering the chat rooms efficiently when there are a large number of rooms to display.
    /// </summary>
    private Virtualize<ChatRoom>? _virtualized;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomListView"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatRoomListView(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the dialog service for showing dialogs.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the new group chat feature is enabled.
    /// </summary>
    [Parameter]
    public bool CanCreateNewGroup { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a new chat group is created.
    /// </summary>
    [Parameter]
    public ChatRoomCreateDelegate? OnNewChatGroup { get; set; }

    /// <summary>
    /// Gets or sets the chat state, which contains the current chat room and loading state.
    /// </summary>
    [Inject]
    private ChatState ChatState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the chat room state, which contains the state of the chat rooms, such as blocked, hidden, or archived rooms.
    /// </summary>
    [Inject]
    private ChatRoomState RoomState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the items provider for fetching chat rooms.
    /// </summary>
    [Parameter]
    public ChatRoomItemsProvider? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the users provider for fetching users in a chat room.
    /// </summary>
    [Parameter]
    public ChatRoomUsersProvider? UsersProvider { get; set; }

    /// <summary>
    /// Gets or sets the unread messages provider for fetching the count of unread messages in a chat room.
    /// </summary>
    [Parameter]
    public ChatUnreadMessagesProvider? UnreadMessagesProvider { get; set; }

    /// <summary>
    /// Gets or sets the last message provider for fetching the last message in a chat room.
    /// </summary>
    [Parameter]
    public ChatLastMessageProvider? LastMessageProvider { get; set; }

    /// <summary>
    /// Gets or sets the item template fragment to render a chat room option.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatRoom>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the function to search for users in the chat room.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, CancellationToken, Task<IEnumerable<ChatUser>>>? UserSearchProvider { get; set; }

    /// <summary>
    /// Gets or sets the function to search for chat rooms.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, CancellationToken, Task<IEnumerable<ChatRoom>>>? RoomSearchProvider { get; set; }

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
    /// Gets or sets the owner of the chat room, which is used to determine the context of the chat.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when the owner of the chat room changes, allowing the component to update the displayed chat rooms based on the new owner context.
    /// </summary>
    [Parameter]
    public EventCallback<ChatUser?> OwnerChanged { get; set; }

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
    /// Gets or sets a value indicating whether the chat room can be pinned.
    /// </summary>
    [Parameter]
    public bool CanPin { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be muted.
    /// </summary>
    [Parameter]
    public bool CanMute { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be archived.
    /// </summary>
    [Parameter]
    public bool CanArchive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room can be unarchived.
    /// </summary>
    [Parameter]
    public bool CanUnarchive { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is deleted.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnDelete { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is blocked.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnBlockChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is hidden.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnHideChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is archived or unarchived.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnArchiveChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is pinned.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnPinChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is muted.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnMuteChanged { get; set; }

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
    /// Gets or sets a value indicating whether the unblock functionality is enabled for the chat room list.
    /// </summary>
    [Parameter]
    public bool CanUnhide { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show deleted rooms in the chat room list.
    /// </summary>
    [Parameter]
    public bool ShowDeletedRoom { get; set; }

    /// <summary>
    /// Gets or sets the fragment for displaying when there are no chat rooms to show in the list.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the logger for the component, which is used for logging information and errors related to the chat room list view.
    /// </summary>
    [Inject]
    private ILogger<ChatRoomListView> Logger { get; set; } = default!;

    /// <summary>
    /// Gets or sets the chat engine, which is used for managing chat rooms and messages in real-time.
    /// </summary>
    [Inject]
    private ChatEngine ChatEngine { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ItemsProvider is null)
        {
            throw new InvalidOperationException("The ItemsProvider parameter must be set to a valid ChatRoomItemsProvider function.");
        }

        if (UsersProvider is null)
        {
            throw new InvalidOperationException("The UsersProvider parameter must be set to a valid ChatRoomUsersProvider function.");
        }

        if (LastMessageProvider is null)
        {
            throw new InvalidOperationException("The LastMessageProvider parameter must be set to a valid ChatLastMessageProvider function.");
        }

        if (UnreadMessagesProvider is null)
        {
            throw new InvalidOperationException("The UnreadMessagesProvider parameter must be set to a valid ChatUnreadMessagesProvider function.");
        }

        ChatEngine.SetLastMessageProvider(LastMessageProvider);
        ChatEngine.SetUnreadProvider(UnreadMessagesProvider);
        ChatEngine.SetUsersProvider(UsersProvider);

        RoomState.RoomsChanged += OnRoomsChanged;
        RoomState.RoomUpdated += OnRoomUpdated;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        RoomState.RoomsChanged -= OnRoomsChanged;
        RoomState.RoomUpdated -= OnRoomUpdated;

        return base.DisposeAsync();
    }

    /// <summary>
    /// Handles the rooms changed event and triggers a component state update.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void OnRoomsChanged(object? sender, EventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the room updated event and triggers a UI refresh.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void OnRoomUpdated(object? sender, EventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles changes to the selected chat rooms and updates the component state.
    /// </summary>
    /// <param name="value">The new collection of selected chat rooms.</param>
    private void OnSelectedRoomsChanged(IEnumerable<ChatRoom> value)
    {
        _selectedRooms = [.. value];
        StateHasChanged();
    }

    /// <summary>
    /// Gets the current list of chat rooms based on the selected view (normal, blocked, or hidden).
    /// </summary>
    private ICollection<ChatRoom> CurrentRooms => _listView switch
    {
        ListView.Normal => OrderRooms(_selectedRooms.Count > 0 ? _selectedRooms : _chatRooms),
        ListView.Blocked => OrderRooms(_blockedRooms),
        ListView.Hidden => OrderRooms(_hiddenRooms),
        ListView.Archived => OrderRooms(_archivedRooms),
        _ => []
    };

    /// <summary>
    /// Sorts chat rooms by pinned status and last message date.
    /// </summary>
    /// <remarks>The input list is sorted in-place.</remarks>
    /// <param name="value">The list of chat rooms to sort.</param>
    /// <returns>The sorted collection with pinned rooms first, ordered by most recent message date.</returns>
    private static List<ChatRoom> OrderRooms(List<ChatRoom> value)
    {
        value.Sort((a, b) =>
        {
            if (a.IsPinned && !b.IsPinned)
            {
                return -1;
            }
            else if (!a.IsPinned && b.IsPinned)
            {
                return 1;
            }
            else
            {
                return a.CreatedDate.CompareTo(b.CreatedDate);
            }
        });

        return value;
    }

    /// <summary>
    /// Updates the chat rooms collection by canceling any pending operations, clearing the existing rooms, and fetching
    /// new rooms using the provided filter expression.
    /// </summary>
    /// <param name="listViewValue">The list view mode to set for the room display.</param>
    /// <param name="rooms">The collection to populate with filtered chat rooms.</param>
    /// <param name="value">The filter expression to apply when retrieving chat rooms.</param>
    /// <param name="localizedMessage">The localization key for the log message in case of operation cancellation.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    private async Task OnUpdateRoomsAsync(
       ListView listViewValue,
       List<ChatRoom> rooms,
       Expression<Func<ChatRoom, bool>> value,
       string localizedMessage)
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        _listView = listViewValue;
        rooms.Clear();

        if (ItemsProvider is not null)
        {
            try
            {
                var filter = PredicateBuilder<ChatRoom>.True;
                filter = PredicateBuilder<ChatRoom>.And(filter, value);
                filter = Owner is not null ? PredicateBuilder<ChatRoom>.And(filter, x => x.OwnerId == Owner.Id) : filter;

                if (!ShowDeletedRoom)
                {
                    filter = PredicateBuilder<ChatRoom>.And(filter, x => !x.IsDeleted);
                }

                var items = await ItemsProvider(new(filter), token);
                rooms.AddRange(items);
            }
            catch (OperationCanceledException ex)
            {
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    var message = Localizer[localizedMessage];
                    Logger.LogInformation(ex, message, Owner?.UserName);
                }
            }
        }
    }

    /// <summary>
    /// Displays all non-blocked and non-hidden chat rooms in the normal list view.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of updating the room list view.</returns>
    private async Task OnShowRoomsAsync()
    {
        await OnUpdateRoomsAsync(
            ListView.Normal,
            _chatRooms,
            x => !x.IsBlocked && !x.IsHidden && !x.IsArchived,
            LanguageResource.CX_Chat_Room_ShowAllRooms_OperationCanceled);
    }

    /// <summary>
    /// Occurs when the unblock rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the blocked rooms view.</returns>
    private async Task OnUnblockRoomsAsync()
    {
        await OnUpdateRoomsAsync(ListView.Blocked, _blockedRooms, x => x.IsBlocked, LanguageResource.CX_Chat_Room_Blocked_OperationCanceled);
    }

    /// <summary>
    /// Occurs when the unhide rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the hidden rooms view.</returns>
    private async Task OnUnhideRoomsAsync()
    {
        await OnUpdateRoomsAsync(ListView.Hidden, _hiddenRooms, x => x.IsHidden, LanguageResource.CX_Chat_Room_Hidden_OperationCanceled);
    }

    /// <summary>
    /// Occurs when the unarchive rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the archived rooms view.</returns>
    private async Task OnUnarchiveRoomsAsync()
    {
        await OnUpdateRoomsAsync(ListView.Archived, _archivedRooms, x => x.IsArchived, LanguageResource.CX_Chat_Room_Archived_OperationCanceled);
    }

    /// <summary>
    /// Occurs when the room search action is triggered.
    /// </summary>
    /// <param name="e">Events args associated to the method.</param>
    /// <returns>Returns a task which displays the rooms based on the predicate <see cref="RoomSearchProvider"/> when completed.</returns>
    private async Task OnChatRoomSearchAsync(OptionsSearchEventArgs<ChatRoom> e)
    {
        if (RoomSearchProvider is not null)
        {
            if (_cts is not null)
            {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            e.Items = await RoomSearchProvider(e.Text, RoomNameComparison, token);
        }
    }

    /// <summary>
    /// Occurs when the new chat group action is triggered.
    /// </summary>
    /// <returns>Returns a task which creates a new chat group when completed.</returns>
    private async Task OnNewChatGroupAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<ChatUserGroupSelectorDialog>(
            a =>
            {
                a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogOk];
                a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogCancel];
                a.Parameters.Add(nameof(ChatUserGroupSelectorDialog.OnSearchProvider), UserSearchProvider);
                a.Parameters.Add(nameof(ChatUserGroupSelectorDialog.Owner), Owner);
                a.Parameters.Add(nameof(ChatUserGroupSelectorDialog.StringComparison), UsernameComparison);
            }
        );

        if (dialog.Cancelled)
        {
            return;
        }

        if (Owner is not null &&
            OnNewChatGroup is not null &&
            dialog.Value is IEnumerable<ChatUser> users)
        {
            var full = new List<string?>(users.Count() + 1)
            {
                Owner.DisplayName
            };

            full.AddRange(users.Select(x => x.DisplayName));

            var room = new ChatRoom()
            {
                CreatedDate = DateTime.Now,
                OwnerId = Owner.Id,
                Owner = Owner,
                Name = string.Join(" & ", full)
            };

            var result = await OnNewChatGroup(new ChatGroupCreateRequest()
            {
                Users = [Owner, .. users],
                Room = room
            });

            if (!result.Success)
            {
                throw new ChatRoomListException("An error occured during the creation of the room.");
            }

            _chatRooms.Add(room);
            ChatState.Room = room;
            _selectedRoom = room.Id.ToString(CultureInfo.InvariantCulture);

            ChatEngine.SetOwner(Owner!.Id);

            if (_cts is not null)
            {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            await ChatEngine.RegisterRoomAsync(room, _cts.Token);
            await ChatEngine.SendCreatedRoomAsync(room, _cts.Token);

            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Loads the chat rooms asynchronously based on the provided ID.
    /// </summary>
    /// <param name="id">Identifier of the room.</param>
    /// <returns>Returns a task which loads the room when completed.</returns>
    private async Task LoadChatRoomsAsync(long id = -1)
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        ChatState.IsRoomLoading = true;
        _chatRooms.Clear();

        if (ItemsProvider is not null)
        {
            try
            {
                var predicate = PredicateBuilder<ChatRoom>.True;

                if (!CanUnblock)
                {
                    predicate = PredicateBuilder<ChatRoom>.And(x => !x.IsBlocked);
                }

                if (!CanUnhide)
                {
                    predicate = PredicateBuilder<ChatRoom>.And(x => !x.IsHidden);
                }

                if (!CanUnarchive)
                {
                    predicate = PredicateBuilder<ChatRoom>.And(x => !x.IsArchived);
                }

                if (!ShowDeletedRoom)
                {
                    predicate = PredicateBuilder<ChatRoom>.And(predicate, x => !x.IsDeleted);
                }

                if (Owner is not null)
                {
                    predicate = PredicateBuilder<ChatRoom>.And(predicate, x => x.OwnerId == Owner.Id);
                }

                var items = await ItemsProvider(new(predicate), token);

                foreach (var item in items)
                {
                    if (item.IsBlocked)
                    {
                        _blockedRooms.Add(item);
                    }
                    else if (item.IsHidden)
                    {
                        _hiddenRooms.Add(item);
                    }
                    else if (item.IsArchived)
                    {
                        _archivedRooms.Add(item);
                    }
                    else
                    {
                        _chatRooms.Add(item);
                    }

                    await ChatEngine.RegisterRoomAsync(item, _cts.Token);
                }
            }
            catch (OperationCanceledException ex)
            {
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    var message = Localizer[LanguageResource.CX_Chat_Room_Loading_OperationCanceled];
                    Logger.LogInformation(ex, message, Owner?.UserName);
                }
            }
        }

        if (id != -1)
        {
            var sources = new (IEnumerable<ChatRoom> Rooms, ListView View)[]
            {
                (_chatRooms, ListView.Normal),
                (_blockedRooms, ListView.Blocked),
                (_hiddenRooms, ListView.Hidden),
                (_archivedRooms, ListView.Archived)
            };

            ChatRoom? found = null;

            foreach (var (rooms, view) in sources)
            {
                found = rooms.FirstOrDefault(x => x.Id == id);

                if (found is not null)
                {
                    _listView = view;
                    break;
                }
            }

            ChatState.Room = found;
        }

        _selectedRoom = ChatState.Room is null ? "-1" : ChatState.Room.Id.ToString(CultureInfo.InvariantCulture);
        ChatState.IsRoomLoading = false;

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Occurs when the rename action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which renames the chat room when completed.</returns>
    private async Task OnRenameAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowDialogAsync<ChatRoomRenameDialog>(a =>
            {
                a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogOk];
                a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogCancel];
                a.Parameters.Add(nameof(ChatRoomRenameDialog.Value), ChatState.Room?.Name);
            }),
            applyChange: async (value) =>
            {
                ChatState.Room?.Name = value as string;
                await ChatEngine.SendUpdatedRoomAsync(ChatState.Room);
            },
            callback: OnRename,
            clearRoom: false
        );
    }

    /// <summary>
    /// Occurs when the delete action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which deletes the chat room when completed.</returns>
    private async Task OnDeleteAsync()
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_DeleteRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_DeleteRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (value) =>
            {
                if (value is ChatRoom cr)
                {
                    return ChatEngine.UnregisterRoomAsync(cr, _cts.Token);
                }

                return Task.CompletedTask;
            },
            callback: OnDelete,
            clearRoom: true
        );
    }

    /// <summary>
    /// Occurs when the hide action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which hides the chat room when completed.</returns>
    private async Task OnHideAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_HideRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_HideRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.Room is not null)
                {
                    ChatState.Room.IsHidden = true;
                    _hiddenRooms.Add(ChatState.Room);
                    _chatRooms.Remove(ChatState.Room);
                }

                return Task.CompletedTask;
            },
            callback: OnHideChanged
        );
    }

    /// <summary>
    /// Occurs when the hide action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which archives the chat room when completed.</returns>
    private async Task OnArchiveAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_ArchiveRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_ArchiveRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.Room is not null)
                {
                    ChatState.Room.IsArchived = true;
                    _archivedRooms.Add(ChatState.Room);
                    _chatRooms.Remove(ChatState.Room);
                }

                return Task.CompletedTask;
            },
            callback: OnArchiveChanged
        );
    }

    /// <summary>
    /// Occurs when the block action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which blocks the room when completed.</returns>
    private async Task OnBlockAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_BlockRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_BlockRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.Room is not null)
                {
                    ChatState.Room.IsBlocked = true;
                    _blockedRooms.Add(ChatState.Room);
                    _chatRooms.Remove(ChatState.Room);
                }

                return Task.CompletedTask;
            },
            callback: OnBlockChanged
        );
    }

    /// <summary>
    /// Occurs when the pin action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which pins the room when completed.</returns>
    private async Task OnPinAsync()
    {
        await ExecuteRoomActionAsync(
            applyChange: () =>
            {
                ChatState.Room?.IsPinned = true;

                return Task.CompletedTask;
            },
            callback: OnPinChanged
        );
    }

    /// <summary>
    /// Occurs when the unpin action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which pins the room when completed.</returns>
    private async Task OnUnpinAsync()
    {
        await ExecuteRoomActionAsync(
            applyChange: () =>
            {
                ChatState.Room?.IsPinned = false;

                return Task.CompletedTask;
            },
            callback: OnPinChanged
        );
    }

    /// <summary>
    /// Occurs when the pin action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which mutes the room when completed.</returns>
    private async Task OnMuteAsync()
    {
        await ExecuteRoomActionAsync(
            applyChange: () =>
            {
                ChatState.Room?.IsMuted = true;

                return Task.CompletedTask;
            },
            callback: OnMuteChanged
        );
    }

    /// <summary>
    /// Occurs when the unmute action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which unmutes the room when completed.</returns>
    private async Task OnUnmuteAsync()
    {
        await ExecuteRoomActionAsync(
            applyChange: () =>
            {
                ChatState.Room?.IsMuted = false;

                return Task.CompletedTask;
            },
            callback: OnMuteChanged
        );
    }

    /// <summary>
    /// Occurs when the unblock action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which unblocks the room when completed.</returns>
    private async Task OnUnblockAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_UnblockRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_UnblockRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.Room is not null)
                {
                    ChatState.Room.IsBlocked = false;
                    _blockedRooms.Remove(ChatState.Room);
                    _chatRooms.Add(ChatState.Room);
                }

                return Task.CompletedTask;
            },
            callback: OnBlockChanged,
            clearRoom: true
        );
    }

    /// <summary>
    /// Occurs when the unhide action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which unhides the room when completed.</returns>
    private async Task OnUnhideAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_UnhideRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_UnhideRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.Room is not null)
                {
                    ChatState.Room.IsHidden = false;
                    _hiddenRooms.Remove(ChatState.Room);
                    _chatRooms.Add(ChatState.Room);
                }

                return Task.CompletedTask;
            },
            callback: OnHideChanged,
            clearRoom: true
        );
    }

    /// <summary>
    /// Occurs when the unarchive action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which unarchives the room when completed.</returns>
    private async Task OnUnarchiveAsync()
    {
        await ExecuteRoomActionAsync(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_UnarchiveRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_UnarchiveRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.Room is not null)
                {
                    ChatState.Room.IsArchived = false;
                    _archivedRooms.Remove(ChatState.Room);
                    _chatRooms.Add(ChatState.Room);
                }

                return Task.CompletedTask;
            },
            callback: OnArchiveChanged,
            clearRoom: true
        );
    }

    /// <summary>
    /// Executes a chat room action by showing a dialog, applying changes to the chat room based on the dialog result,
    /// invoking a callback, and optionally reloading the chat rooms and clearing the selected room.
    /// </summary>
    /// <param name="showDialog">A function that shows a dialog and returns the result.</param>
    /// <param name="applyChange">A function that applies changes to the chat room based on the dialog result.</param>
    /// <param name="callback">A callback to invoke after the action is applied.</param>
    /// <param name="clearRoom">Indicates whether to clear the selected chat room after the action.</param>
    /// <returns>Returns a task representing the asynchronous operation.</returns>
    private Task ExecuteRoomActionAsync(
        Func<Task<DialogResult>> showDialog,
        Func<object?, Task>? applyChange = null,
        EventCallback<ChatRoom>? callback = null,
        bool clearRoom = true)
    {
        return ExecuteRoomActionCoreAsync(showDialog, applyChange, callback, clearRoom);
    }

    /// <summary>
    /// Executes a chat room action by applying changes to the chat room, invoking a callback, and optionally reloading the chat rooms.
    /// </summary>
    /// <param name="applyChange">A function that applies changes to the chat room based on the dialog result.</param>
    /// <param name="callback">A callback to invoke after the action is applied.</param>
    /// <param name="clearRoom">Indicates whether to clear the selected chat room after the action.</param>
    /// <returns>Returns a task representing the asynchronous operation.</returns>
    private Task ExecuteRoomActionAsync(
        Func<Task> applyChange,
        EventCallback<ChatRoom>? callback = null,
        bool clearRoom = false)
    {
        return ExecuteRoomActionCoreAsync(
            showDialog: null,
            applyChange: async _ => await applyChange(),
            callback: callback,
            clearRoom: clearRoom);
    }

    /// <summary>
    /// Executes a room action with optional dialog interaction before applying changes.
    /// </summary>
    /// <param name="showDialog">Optional function to display a dialog and return its result.</param>
    /// <param name="applyChange">Optional function to apply changes with the provided value.</param>
    /// <param name="callback">Optional event callback to invoke with the affected chat room.</param>
    /// <param name="clearRoom">Indicates whether to clear the room during execution.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ExecuteRoomActionCoreAsync(
        Func<Task<DialogResult>>? showDialog,
        Func<object?, Task>? applyChange,
        EventCallback<ChatRoom>? callback,
        bool clearRoom)
    {
        if (showDialog is not null)
        {
            var dialog = await showDialog();

            if (dialog.Cancelled)
            {
                return;
            }

            await ExecuteInternalAsync(dialog.Value, applyChange, callback, clearRoom);
        }
        else
        {
            await ExecuteInternalAsync(null, applyChange, callback, clearRoom);
        }
    }

    /// <summary>
    /// Executes an internal operation that manages chat state loading status, applies optional changes, invokes
    /// callbacks, and optionally clears the chat room.
    /// </summary>
    /// <param name="value">The value to pass to the change application function.</param>
    /// <param name="applyChange">An optional function to apply changes using the provided value.</param>
    /// <param name="callback">An optional event callback to invoke with the current chat room.</param>
    /// <param name="clearRoom">Indicates whether to clear the chat room after execution.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ExecuteInternalAsync(
        object? value,
        Func<object?, Task>? applyChange,
        EventCallback<ChatRoom>? callback,
        bool clearRoom)
    {
        ChatState.IsRoomLoading = true;

        if (applyChange is not null)
        {
            await applyChange(value);
        }

        if (callback?.HasDelegate == true && ChatState.Room is not null)
        {
            await callback.Value.InvokeAsync(ChatState.Room);
        }

        if (clearRoom)
        {
            ChatState.Room = null;
        }

        ChatState.IsRoomLoading = false;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        await LoadChatRoomsAsync();
    }

    /// <summary>
    /// Retrieves the available actions for the sleek dial based on current view state and permissions.
    /// </summary>
    /// <returns>A collection of tuples containing the localized title, icon, and asynchronous action handler for each available
    /// dial action.</returns>
    private IEnumerable<(string Title, Icon Icon, Func<Task> Action)> GetSleekDialActions()
    {
        if (CanCreateNewGroup &&
            _listView == ListView.Normal)
        {
            yield return (Localizer[LanguageResource.CX_Chat_Room_NewGroup], new Size24.Add(), OnNewChatGroupAsync);
        }

        if (CanUnblock)
        {
            if (_listView != ListView.Blocked)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowBlockedRooms], new Size24.PresenceBlocked(), OnUnblockRoomsAsync);
            }
            else
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowAllRooms], new Size24.PresenceAvailable(), OnShowRoomsAsync);
            }
        }

        if (CanUnhide)
        {
            if (_listView != ListView.Hidden)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowHiddenRooms], new Size24.Eye(), OnUnhideRoomsAsync);
            }
            else
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowAllRooms], new Size24.EyeOff(), OnShowRoomsAsync);
            }
        }

        if (CanUnarchive)
        {
            if (_listView != ListView.Archived)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowArchivedRooms], new Size24.Archive(), OnUnarchiveRoomsAsync);
            }
            else
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowAllRooms], new Size24.ArchiveArrowBack(), OnShowRoomsAsync);
            }
        }
    }

    /// <summary>
    /// Gets the available more menu actions for the current chat room based on the list view and user permissions.
    /// </summary>
    /// <returns>A collection of chat room actions available for the current context.</returns>
    private IEnumerable<ChatRoomAction> GetMoreMenuActions()
    {
        if (ChatState.Room is null)
        {
            yield break;
        }

        var room = ChatState.Room;

        if (_listView == ListView.Normal)
        {
            if (CanRename)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Rename_Menu],
                    Icon = new Size24.Edit(),
                    Action = OnRenameAsync
                };
            }

            if (CanPin)
            {
                yield return new ChatRoomAction
                {
                    Label = room.IsPinned ? Localizer[LanguageResource.CX_Chat_Room_Unpin] : Localizer[LanguageResource.CX_Chat_Room_Pin],
                    Icon = room.IsPinned ? new Size24.PinOff() : new Size24.Pin(),
                    Action = room.IsPinned ? OnUnpinAsync : OnPinAsync
                };
            }

            if (CanMute)
            {
                yield return new ChatRoomAction
                {
                    Label = room.IsMuted ? Localizer[LanguageResource.CX_Chat_Room_Unmute] : Localizer[LanguageResource.CX_Chat_Room_Mute],
                    Icon = room.IsMuted ? new Size24.Speaker2() : new Size24.SpeakerMute(),
                    Action = room.IsMuted ? OnUnmuteAsync : OnMuteAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (CanArchive)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Archive],
                    Icon = new Size24.Archive(),
                    Action = OnArchiveAsync
                };
            }

            if (CanBlock)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Block],
                    Icon = new Size24.PresenceBlocked(),
                    Action = OnBlockAsync
                };
            }

            if (CanHide)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Hide],
                    Icon = new Size24.EyeOff(),
                    Action = OnHideAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (CanDelete)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
        else if (_listView == ListView.Blocked)
        {
            if (CanArchive)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Archive],
                    Icon = new Size24.Archive(),
                    Action = OnArchiveAsync
                };
            }

            if (CanUnblock)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Unblock],
                    Icon = new Size24.PresenceAvailable(),
                    Action = OnUnblockAsync
                };
            }

            if (CanHide)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Hide],
                    Icon = new Size24.Eye(),
                    Action = OnHideAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (CanDelete)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
        else if (_listView == ListView.Hidden)
        {
            if (CanArchive)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Archive],
                    Icon = new Size24.Archive(),
                    Action = OnArchiveAsync
                };
            }

            if (CanUnhide)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Unhide],
                    Icon = new Size24.Eye(),
                    Action = OnUnhideAsync
                };
            }

            if (CanBlock)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Block],
                    Icon = new Size24.PresenceBlocked(),
                    Action = OnBlockAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (CanDelete)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
        else if (_listView == ListView.Archived)
        {
            if (CanUnarchive)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Unarchive],
                    Icon = new Size24.ArchiveArrowBack(),
                    Action = OnUnarchiveAsync
                };
            }

            if (CanBlock)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Block],
                    Icon = new Size24.PresenceBlocked(),
                    Action = OnBlockAsync
                };
            }

            if (CanHide)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Hide],
                    Icon = new Size24.EyeOff(),
                    Action = OnHideAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (CanDelete)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
    }

    /// <summary>
    /// Handles the click event on a chat room item. If the clicked room is not already selected, it updates the selected room in the chat state and sets the selected room ID for UI purposes.
    /// </summary>
    /// <param name="room">The chat room that was clicked.</param>
    private void HandleRoomClick(ChatRoom room)
    {
        if (ChatState.Room?.Id == room.Id)
        {
            return;
        }

        ChatState.Room = room;
        _selectedRoom = Invariant.ToString(room.Id);
    }

    /// <summary>
    /// Retrieves the label for the currently selected list view of chat rooms, which can be "Normal", "Blocked", "Archived", or "Hidden".
    /// </summary>
    /// <returns>The label for the currently selected list view of chat rooms.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the list view state is invalid.</exception>
    private string GetSelectedRoomsLabel()
    {
        return _listView switch
        {
            ListView.Normal => Localizer[LanguageResource.CX_Chat_Room_Normal],
            ListView.Blocked => Localizer[LanguageResource.CX_Chat_Room_Blocked],
            ListView.Archived => Localizer[LanguageResource.CX_Chat_Room_Archived],
            ListView.Hidden => Localizer[LanguageResource.CX_Chat_Room_Hidden],
            _ => throw new InvalidOperationException("Invalid list view state.")
        };
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasOwnerChanged = parameters.TryGetValue<ChatUser>(nameof(Owner), out var newOwner) && newOwner?.Id != Owner?.Id;
        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasOwnerChanged)
        {
            _hasOwnerChanged = false;
            ChatState.Room = null;

            if (OwnerChanged.HasDelegate)
            {
                await OwnerChanged.InvokeAsync(Owner);
            }

            await LoadChatRoomsAsync();
        }
    }
}
