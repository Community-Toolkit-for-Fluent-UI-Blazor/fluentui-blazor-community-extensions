using System.Globalization;
using FluentUI.Blazor.Community.Components.Chat;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

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
        Hidden
    }

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
    /// Represents the selected chat room.
    /// </summary>
    private string? _selectedRoom;

    /// <summary>
    /// Render fragment for displaying chat room options.
    /// </summary>
    private readonly RenderFragment<ChatRoom> _renderOptionItem;

    /// <summary>
    /// Represents the list view type.
    /// </summary>
    private ListView _listView;

    /// <summary>
    /// Gets or sets the dialog service for showing dialogs.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the new group chat feature is enabled.
    /// </summary>
    [Parameter]
    public bool IsNewGroupChatEnabled { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a new chat group is created.
    /// </summary>
    [Parameter]
    public ChatRoomCreateDelegate? OnNewChatGroup { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is in mobile view.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the chat state, which contains the current chat room and loading state.
    /// </summary>
    [Inject]
    private ChatState ChatState { get; set; } = default!;

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
    /// Gets or sets the event callback for when the chat room is in mobile view to navigate to the chat.
    /// </summary>
    [Parameter]
    public EventCallback OnMobileNavigation { get; set; }

    /// <summary>
    /// Gets or sets the function to search for users in the chat room.
    /// </summary>
    [Parameter]
    public Func<string?, Task<IEnumerable<ChatUser>>>? UserSearchFunction { get; set; }

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
    /// Gets or sets the owner of the chat room, which is used to determine the context of the chat.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

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
        ListView.Normal => _selectedRooms.Count > 0 ? _selectedRooms : _chatRooms,
        ListView.Blocked => _blockedRooms,
        ListView.Hidden => _hiddenRooms,
        _ => []
    };

    /// <summary>
    /// Occurs when the unblock rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the blocked rooms view.</returns>
    private async Task OnUnblockRoomsAsync()
    {
        _listView = ListView.Blocked;
        _blockedRooms.Clear();

        if (ItemsProvider is not null)
        {
            var items = await ItemsProvider(new(x => x.IsBlocked));
            _blockedRooms.AddRange(items);
        }
    }

    /// <summary>
    /// Occurs when the unblock rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the blocked rooms view.</returns>
    private async Task OnUnhideRoomsAsync()
    {
        _listView = ListView.Hidden;
        _hiddenRooms.Clear();

        if (ItemsProvider is not null)
        {
            var items = await ItemsProvider(new(x => x.IsHidden));
            _hiddenRooms.AddRange(items);
        }
    }

    /// <summary>
    /// Occurs when the room search action is triggered.
    /// </summary>
    /// <param name="e">Events args associated to the method.</param>
    /// <returns>Returns a task which displays the rooms based on the predicate <see cref="RoomSearchFunction"/> when completed.</returns>
    private async Task OnChatRoomSearchAsync(OptionsSearchEventArgs<ChatRoom> e)
    {
        if (RoomSearchFunction is not null)
        {
            e.Items = await RoomSearchFunction(e.Text, RoomNameComparison);
        }
    }

    /// <summary>
    /// Occurs when the new chat group action is triggered.
    /// </summary>
    /// <returns>Returns a task which creates a new chat group when completed.</returns>
    /// <exception cref="ChatRoomListException">Occurs when the <see cref="ChatGroupCreateResult.GroupId"/> is below to 1.</exception>
    private async Task OnNewChatGroupAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<ChatUserGroupSelectorDialog>(
            a =>
            {
                a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogOk];
                a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogCancel];
                a.Parameters.Add(nameof(ChatUserGroupSelectorDialog.OnSearchFunction), UserSearchFunction);
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
            var result = await OnNewChatGroup(new ChatGroupCreateRequest()
            {
                Users = [.. users, Owner],
            });

            if (!result.GroupId.HasValue ||
                result.GroupId < 1)
            {
                throw new ChatRoomListException("The chat group must have an id greater than or equal to 1.");
            }

            await LoadChatRoomsAsync(result.GroupId.GetValueOrDefault());
        }
    }

    /// <summary>
    /// Formats the chat message for display in the chat room list view.
    /// </summary>
    /// <param name="message">Message to format.</param>
    /// <returns>The formatted message into a <see cref="MarkupString"/>.</returns>
    private MarkupString Format(IChatMessage message)
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

/*    /// <summary>
    /// Occurs when the selected chat room changes.
    /// </summary>
    /// <returns>Returns a task which change the chat room when completed.</returns>
    private async Task OnSelectedChatRoomChangedAsync()
    {
        if (string.IsNullOrEmpty(_selectedRoom))
        {
            ChatState.Room = null;
            _selectedRoom = "-1";
        }
        else
        {
            var roomId = long.Parse(_selectedRoom, CultureInfo.InvariantCulture);
            ChatState.Room = _chatRooms.FirstOrDefault(x => x.Id == roomId);
        }

        if (IsMobile && OnMobileNavigation.HasDelegate)
        {
            await OnMobileNavigation.InvokeAsync();
        }
    }*/

    /// <summary>
    /// Loads the chat rooms asynchronously based on the provided ID.
    /// </summary>
    /// <param name="id">Identifier of the room.</param>
    /// <returns>Returns a task which loads the room when completed.</returns>
    private async Task LoadChatRoomsAsync(long id = -1)
    {
        ChatState.IsLoading = true;
        await InvokeAsync(StateHasChanged);

        _chatRooms.Clear();

        if (ItemsProvider is not null)
        {
            var predicate = PredicateBuilder<ChatRoom>.True;

            if (CanUnblock)
            {
                predicate = PredicateBuilder<ChatRoom>.Or(x => !x.IsBlocked);
            }

            if (CanUnhide)
            {
                predicate = PredicateBuilder<ChatRoom>.Or(x => !x.IsHidden);
            }

            var items = await ItemsProvider(new(predicate));
            _chatRooms.AddRange(items);
        }

        if (id != -1)
        {
            ChatState.Room = _chatRooms.FirstOrDefault(x => x.Id == id);
        }

        _selectedRoom = ChatState.Room is null ? "-1" : ChatState.Room.Id.ToString(CultureInfo.InvariantCulture);

        //if (_chatRooms.Count > 0 &&
        //    ChatState.Room is null)
        //{
        //    ChatState.Room = _chatRooms[0];
        //    _selectedRoom = _chatRooms[0].Id.ToString();
        //}

        ChatState.IsLoading = false;

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Occurs when the rename action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which renames the chat room when completed.</returns>
    private async Task OnRenameAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<ChatRoomRenameDialog>(a =>
        {
            a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogOk];
            a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogCancel];
            a.Parameters.Add(nameof(ChatRoomRenameDialog.Value), ChatState.Room?.Name);
        });

        if (dialog.Cancelled)
        {
            return;
        }

        ChatState.IsLoading = true;

        if (ChatState.Room is not null)
        {
            ChatState.Room.Name = dialog.Value as string;
            await LoadChatRoomsAsync(ChatState.Room.Id);

            if (OnRename.HasDelegate)
            {
                await OnRename.InvokeAsync(ChatState.Room);
            }
        }

        ChatState.IsLoading = false;
    }

    /// <summary>
    /// Occurs when the delete action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which deletes the chat room when completed.</returns>
    private async Task OnDeleteAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            Localizer[LanguageResource.CX_Chat_Room_DeleteRoomMessage],
            Localizer[LanguageResource.CX_Chat_Room_DialogYes],
            Localizer[LanguageResource.CX_Chat_Room_DialogNo],
            Localizer[LanguageResource.CX_Chat_Room_DeleteRoomTitle]);

        if (dialog.Cancelled)
        {
            return;
        }

        ChatState.IsLoading = true;

        if (OnDelete.HasDelegate)
        {
            await OnDelete.InvokeAsync(ChatState.Room);
        }

        ChatState.Room = null;

        await LoadChatRoomsAsync();
        ChatState.IsLoading = false;
    }

    /// <summary>
    /// Occurs when the hide action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which hides the chat room when completed.</returns>
    private async Task OnHideAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            Localizer[LanguageResource.CX_Chat_Room_HideRoomMessage],
            Localizer[LanguageResource.CX_Chat_Room_DialogYes],
            Localizer[LanguageResource.CX_Chat_Room_DialogNo],
            Localizer[LanguageResource.CX_Chat_Room_HideRoomTitle]);

        if (dialog.Cancelled)
        {
            return;
        }

        ChatState.IsLoading = true;

        ChatState.Room?.IsHidden = true;

        if (OnHide.HasDelegate)
        {
            await OnHide.InvokeAsync(ChatState.Room);
        }

        await LoadChatRoomsAsync();
        ChatState.IsLoading = false;
    }

    /// <summary>
    /// Occurs when the block action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which blocks the room when completed.</returns>
    private async Task OnBlockAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            Localizer[LanguageResource.CX_Chat_Room_BlockRoomMessage],
            Localizer[LanguageResource.CX_Chat_Room_DialogYes],
            Localizer[LanguageResource.CX_Chat_Room_DialogNo],
            Localizer[LanguageResource.CX_Chat_Room_BlockRoomTitle]);

        if (dialog.Cancelled)
        {
            return;
        }

        ChatState.IsLoading = true;

        ChatState.Room?.IsBlocked = true;

        if (OnBlock.HasDelegate)
        {
            await OnBlock.InvokeAsync(ChatState.Room);
        }

        ChatState.IsLoading = false;
    }

    /// <summary>
    /// Occurs when the unblock action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which unblocks the room when completed.</returns>
    private async Task OnUnblockAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            Localizer[LanguageResource.CX_Chat_Room_UnblockRoomMessage],
            Localizer[LanguageResource.CX_Chat_Room_DialogYes],
            Localizer[LanguageResource.CX_Chat_Room_DialogNo],
            Localizer[LanguageResource.CX_Chat_Room_UnblockRoomTitle]);

        if (dialog.Cancelled)
        {
            return;
        }

        ChatState.IsLoading = true;

        ChatState.Room?.IsBlocked = false;

        if (OnUnblock.HasDelegate)
        {
            await OnUnblock.InvokeAsync(ChatState.Room);
        }

        ChatState.IsLoading = false;
    }

    /// <summary>
    /// Occurs when the unhide action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which unblocks the room when completed.</returns>
    private async Task OnUnhideAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            Localizer[LanguageResource.CX_Chat_Room_UnhideRoomMessage],
            Localizer[LanguageResource.CX_Chat_Room_DialogYes],
            Localizer[LanguageResource.CX_Chat_Room_DialogNo],
            Localizer[LanguageResource.CX_Chat_Room_UnhideRoomTitle]);

        if (dialog.Cancelled)
        {
            return;
        }

        ChatState.IsLoading = true;

        ChatState.Room?.IsHidden = false;

        if (OnUnhide.HasDelegate)
        {
            await OnUnhide.InvokeAsync(ChatState.Room);
        }

        ChatState.IsLoading = false;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await LoadChatRoomsAsync();
        }
    }

    private IEnumerable<(string Title, Icon Icon, Func<Task> Action)> GetSleekDialActions()
    {
        if (IsNewGroupChatEnabled)
        {
            yield return (Localizer[LanguageResource.CX_Chat_Room_NewGroup], new Size24.Add(), OnNewChatGroupAsync);
        }

        if (CanUnblock)
        {
            yield return (Localizer[LanguageResource.CX_Chat_Room_ShowBlockedRooms], new Size24.PresenceBlocked(), OnUnblockRoomsAsync);
        }

        if (CanUnhide)
        {
            yield return (Localizer[LanguageResource.CX_Chat_Room_ShowHiddenRooms], new Size24.EyeOff(), OnUnhideRoomsAsync);
        }
    }

    private IEnumerable<(string Label, Icon Icon, Func<Task> Action)> GetPopoverActions()
    {
        if (_listView == ListView.Normal)
        {
            if (CanRename)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Rename], new Size24.Edit(), OnRenameAsync);
            }

            if (CanBlock)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Block], new Size24.PresenceBlocked(), OnBlockAsync);
            }

            if (CanHide)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Hide], new Size24.EyeOff(), OnHideAsync);
            }

            if (CanDelete)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Delete], new Size24.Delete(), OnDeleteAsync);
            }
        }
        else if (_listView == ListView.Blocked)
        {
            if (CanUnblock)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Unblock], new Size24.PresenceAvailable(), OnUnblockAsync);
            }

            if (CanHide)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Hide], new Size24.Eye(), OnHideAsync);
            }

            if (CanDelete)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Delete], new Size24.Delete(), OnDeleteAsync);
            }
        }
        else if (_listView == ListView.Hidden)
        {
            if (CanUnhide)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Unhide], new Size24.Eye(), OnUnhideAsync);
            }

            if (CanBlock)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Block], new Size24.PresenceBlocked(), OnBlockAsync);
            }

            if (CanDelete)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_Delete], new Size24.Delete(), OnDeleteAsync);
            }
        }
    }
}
