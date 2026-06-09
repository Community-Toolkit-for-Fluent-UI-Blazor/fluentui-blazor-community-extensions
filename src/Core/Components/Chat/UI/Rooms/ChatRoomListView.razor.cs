using System.Globalization;
using FluentUI.Blazor.Community.Components.Chat;
using FluentUI.Blazor.Community.Components.Chat.Engine;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
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
    private List<ChatRoomView> _selectedRooms = [];

    /// <summary>
    /// Represents the selected chat room.
    /// </summary>
    private string? _selectedRoom;

    /// <summary>
    /// Represents the list view type.
    /// </summary>
    private RoomListView _listView;

    /// <summary>
    /// Represents the virtualized component for rendering the chat rooms efficiently when there are a large number of rooms to display.
    /// </summary>
    private Virtualize<ChatRoomView>? _virtualizeRooms;

    /// <summary>
    /// Represents the chat orchestrator for managing the loading state and refreshing of the chat room list, ensuring that the UI remains responsive and up-to-date with the latest chat room data.
    /// </summary>
    private readonly ChatOrchestrator<ChatRoomView> _chatOrchestrator = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomListView"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatRoomListView(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    private bool IsLoading { get; set; }

    /// <summary>
    /// Gets or sets the dialog service for showing dialogs.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

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
    private ChatRoomViewState RoomState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the items provider for fetching chat rooms.
    /// </summary>
    [Parameter]
    public ChatRoomItemsProvider? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the item template fragment to render a chat room option.
    /// </summary>
    [Parameter]
    public RenderFragment<ChatRoomView>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the function to search for users in the chat room.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, CancellationToken, Task<IEnumerable<ChatUser>>>? UserSearchProvider { get; set; }

    /// <summary>
    /// Gets or sets the function to search for chat rooms.
    /// </summary>
    [Parameter]
    public Func<string?, StringComparison, CancellationToken, Task<IEnumerable<ChatRoomView>>>? RoomSearchProvider { get; set; }

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
    /// Gets or sets the event callback for when a chat room is deleted.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoom> OnDelete { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is blocked.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoomEventArgs> OnBlockChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is hidden.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoomEventArgs> OnHideChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is archived or unarchived.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoomEventArgs> OnArchiveChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is pinned.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoomEventArgs> OnPinChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback for when a chat room is muted.
    /// </summary>
    [Parameter]
    public EventCallback<ChatRoomEventArgs> OnMuteChanged { get; set; }

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
    /// Gets or sets the chat engine, which is used for managing chat rooms and messages in real-time.
    /// </summary>
    [Inject]
    private ChatEngine ChatEngine { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    private bool ShowMoreButton => OnBlockChanged.HasDelegate ||
                                   OnHideChanged.HasDelegate ||
                                   OnDelete.HasDelegate ||
                                   OnPinChanged.HasDelegate ||
                                   OnMuteChanged.HasDelegate ||
                                   OnRename.HasDelegate;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ItemsProvider is null)
        {
            throw new InvalidOperationException("The ItemsProvider parameter must be set to a valid ChatRoomItemsProvider function.");
        }

        RoomState.RoomsChanged += OnRoomsChanged;
        RoomState.RoomUpdated += OnRoomUpdated;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        RoomState.RoomsChanged -= OnRoomsChanged;
        RoomState.RoomUpdated -= OnRoomUpdated;

        GC.SuppressFinalize(this);

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
    private void OnSelectedRoomsChanged(IEnumerable<ChatRoomView> value)
    {
        _selectedRooms = [.. value];
        StateHasChanged();
    }

    /// <summary>
    /// Sorts chat rooms by pinned status and last message date.
    /// </summary>
    /// <remarks>The input list is sorted in-place.</remarks>
    /// <param name="value">The list of chat rooms to sort.</param>
    /// <returns>The sorted collection with pinned rooms first, ordered by most recent message date.</returns>
    private static List<ChatRoomView> OrderRooms(List<ChatRoomView> value)
    {
        value.Sort((a, b) =>
        {
            var aPinned = a.UserState?.IsPinned ?? false;
            var bPinned = b.UserState?.IsPinned ?? false;

            if (aPinned && !bPinned)
            {
                return -1;
            }
            else if (!aPinned && bPinned)
            {
                return 1;
            }

            var aDate = a.LastMessage?.CreatedDate ?? a.Room.CreatedDate;
            var bDate = b.LastMessage?.CreatedDate ?? b.Room.CreatedDate;

            return bDate.CompareTo(aDate);
        });

        return value;
    }

    /// <summary>
    /// Refreshes the chat room data by invoking the chat orchestrator to manage the loading state and calling the RefreshDataAsync method on the virtualized component to update the displayed chat rooms based on the current view and filters.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of refreshing the chat room data.</returns>
    private async Task RefreshDataAsync()
    {
        if (_virtualizeRooms is not null)
        {
            await _virtualizeRooms.RefreshDataAsync();
            await Task.Yield();
        }
    }

    /// <summary>
    /// Displays all non-blocked and non-hidden chat rooms in the normal list view.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of updating the room list view.</returns>
    private async Task OnShowRoomsAsync()
    {
        _listView = RoomListView.Normal;
        await RefreshDataAsync();
    }

    /// <summary>
    /// Occurs when the unblock rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the blocked rooms view.</returns>
    private async Task OnUnblockRoomsAsync()
    {
        _listView = RoomListView.Blocked;
        await RefreshDataAsync();
    }

    /// <summary>
    /// Occurs when the unhide rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the hidden rooms view.</returns>
    private async Task OnUnhideRoomsAsync()
    {
        _listView = RoomListView.Hidden;
        await RefreshDataAsync();
    }

    /// <summary>
    /// Occurs when the unarchive rooms action is triggered.
    /// </summary>
    /// <returns>Returns a task which displays the archived rooms view.</returns>
    private async Task OnUnarchiveRoomsAsync()
    {
        _listView = RoomListView.Archived;
        await RefreshDataAsync();
    }

    /// <summary>
    /// Occurs when the room search action is triggered.
    /// </summary>
    /// <param name="e">Events args associated to the method.</param>
    /// <returns>Returns a task which displays the rooms based on the predicate <see cref="RoomSearchProvider"/> when completed.</returns>
    private async Task OnChatRoomSearchAsync(OptionsSearchEventArgs<ChatRoomView> e)
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
                Name = string.Join(" & ", full),
                IsEmpty = true
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

            ChatState.RoomView = new ChatRoomView()
            {
                Room = room,
                Users = [Owner, .. users],
            };

            _selectedRoom = room.Id.ToString(CultureInfo.InvariantCulture);

            ChatEngine.SetOwner(Owner!.Id);

            if (_cts is not null)
            {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            var roomView = ChatState.RoomView;
            await ChatEngine.RegisterRoomAsync(roomView, _cts.Token);
            await ChatEngine.SendCreatedRoomAsync(roomView, _cts.Token);

            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Loads the chat rooms asynchronously.
    /// </summary>
    /// <returns>Returns a task which loads the room when completed.</returns>
    private async ValueTask<ItemsProviderResult<ChatRoomView>> LoadRoomsAsync(
        ItemsProviderRequest request,
        CancellationToken token)
    {
        var roomFilter = PredicateBuilder<ChatRoom>.True;
        var userFilter = PredicateBuilder<ChatRoomUser>.True;

        if (!ShowDeletedRoom)
        {
            roomFilter = PredicateBuilder<ChatRoom>.And(roomFilter, x => !x.IsDeleted);
        }

        if (!OnBlockChanged.HasDelegate)
        {
            userFilter = PredicateBuilder<ChatRoomUser>.And(userFilter, x => !x.IsBlocked);
        }

        if (!OnHideChanged.HasDelegate)
        {
            userFilter = PredicateBuilder<ChatRoomUser>.And(userFilter, x => !x.IsHidden);
        }

        if (!OnArchiveChanged.HasDelegate)
        {
            userFilter = PredicateBuilder<ChatRoomUser>.And(userFilter, x => !x.IsArchived);
        }

        userFilter = _listView switch
        {
            RoomListView.Normal => PredicateBuilder<ChatRoomUser>.And(userFilter, x => !x.IsBlocked && !x.IsHidden && !x.IsArchived),
            RoomListView.Blocked => PredicateBuilder<ChatRoomUser>.And(userFilter, x => x.IsBlocked),
            RoomListView.Hidden => PredicateBuilder<ChatRoomUser>.And(userFilter, x => x.IsHidden),
            RoomListView.Archived => PredicateBuilder<ChatRoomUser>.And(userFilter, x => x.IsArchived),
            _ => userFilter
        };

        var ownerId = Owner?.Id ?? 0;

        var result = await ItemsProvider!(
            new ChatRoomItemsRequest(
                roomFilter,
                userFilter,
                ownerId,
                request.StartIndex,
                request.Count),
            token);

        foreach (var item in result.Items)
        {
            await ChatEngine.RegisterRoomAsync(item, token);
        }

        var ordered = OrderRooms([.. result.Items]);

        return new ItemsProviderResult<ChatRoomView>(ordered, result.TotalItemCount);
    }

    /// <summary>
    /// Occurs when the rename action is triggered for a chat room.
    /// </summary>
    /// <returns>Returns a task which renames the chat room when completed.</returns>
    private async Task OnRenameAsync()
    {
        await ExecuteRoomActionAsync<ChatRoom>(
            showDialog: () => DialogService.ShowDialogAsync<ChatRoomRenameDialog>(a =>
            {
                a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogOk];
                a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Room_DialogCancel];
                a.Parameters.Add(nameof(ChatRoomRenameDialog.Value), ChatState.RoomView?.Room.Name);
            }),
            applyChange: async (value) =>
            {
                if (ChatState.RoomView is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        Room = ChatState.RoomView.Room with
                        {
                            Name = value as string
                        }
                    };

                    await ChatEngine.SendUpdatedRoomAsync(ChatState.RoomView.Room);
                }
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
        await ExecuteRoomActionAsync<ChatRoom>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_DeleteRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_DeleteRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (value) =>
            {
                return ChatEngine.UnregisterRoomAsync(ChatState.RoomView!, _cts.Token);
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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_HideRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_HideRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsHidden = true
                        }
                    };
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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_ArchiveRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_ArchiveRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsArchived = true
                        }
                    };
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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_BlockRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_BlockRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsBlocked = true
                        }
                    };

                    if (ChatState.RoomView.UserState.UserId == Owner?.Id)
                    {
                        ChatState.RoomView = ChatState.RoomView with
                        {
                            Room = ChatState.RoomView.Room with
                            {
                                IsLocked = true
                            }
                        };
                        return ChatEngine.SendUpdatedRoomAsync(ChatState.RoomView.Room);
                    }
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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            applyChange: () =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsPinned = true
                        }
                    };
                }

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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            applyChange: () =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsPinned = false
                        }
                    };
                }

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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            applyChange: () =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsMuted = true
                        }
                    };
                }

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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            applyChange: () =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsMuted = false
                        }
                    };
                }

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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_UnblockRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_UnblockRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsBlocked = false
                        }
                    };

                    if (ChatState.RoomView.UserState.UserId == Owner?.Id)
                    {
                        ChatState.RoomView = ChatState.RoomView with
                        {
                            Room = ChatState.RoomView.Room with
                            {
                                IsLocked = false
                            }
                        };

                        return ChatEngine.SendUpdatedRoomAsync(ChatState.RoomView.Room);
                    }
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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_UnhideRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_UnhideRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsHidden = false
                        }
                    };
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
        await ExecuteRoomActionAsync<ChatRoomEventArgs>(
            showDialog: () => DialogService.ShowConfirmationAsync(
                Localizer[LanguageResource.CX_Chat_Room_UnarchiveRoomMessage],
                Localizer[LanguageResource.CX_Chat_Room_UnarchiveRoomTitle],
                Localizer[LanguageResource.CX_Chat_Room_DialogYes],
                Localizer[LanguageResource.CX_Chat_Room_DialogNo]),
            applyChange: (e) =>
            {
                if (ChatState.RoomView is not null &&
                    ChatState.RoomView.UserState is not null)
                {
                    ChatState.RoomView = ChatState.RoomView with
                    {
                        UserState = ChatState.RoomView.UserState with
                        {
                            IsArchived = false
                        }
                    };
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
    /// <param name="callback">A callback to invoke with the affected chat room after the action is applied.</param>
    /// <param name="clearRoom">Indicates whether to clear the selected chat room after the action.</param>
    /// <returns>Returns a task representing the asynchronous operation.</returns>
    private Task ExecuteRoomActionAsync<T>(
        Func<Task<DialogResult>> showDialog,
        Func<object?, Task>? applyChange = null,
        EventCallback<T>? callback = null,
        bool clearRoom = true)
    {
        return ExecuteRoomActionCoreAsync<T>(showDialog, applyChange, callback, clearRoom);
    }

    /// <summary>
    /// Executes a chat room action by applying changes to the chat room, invoking a callback, and optionally reloading the chat rooms.
    /// </summary>
    /// <param name="applyChange">A function that applies changes to the chat room based on the dialog result.</param>
    /// <param name="callback">A callback to invoke with the affected chat room view after the action is applied.</param>
    /// <param name="clearRoom">Indicates whether to clear the selected chat room after the action.</param>
    /// <returns>Returns a task representing the asynchronous operation.</returns>
    private Task ExecuteRoomActionAsync<T>(
        Func<Task> applyChange,
        EventCallback<T>? callback = null,
        bool clearRoom = false)
    {
        return ExecuteRoomActionCoreAsync<T>(
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
    /// <param name="callback">Optional event callback to invoke with the affected chat room view.</param>
    /// <param name="clearRoom">Indicates whether to clear the room during execution.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ExecuteRoomActionCoreAsync<T>(
        Func<Task<DialogResult>>? showDialog,
        Func<object?, Task>? applyChange,
        EventCallback<T>? callback = null,
        bool clearRoom = false)
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
    /// <param name="callback">A callback to invoke with the affected chat room view after the action is applied.</param>
    /// <param name="clearRoom">Indicates whether to clear the chat room after execution.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ExecuteInternalAsync<T>(
        object? value,
        Func<object?, Task>? applyChange,
        EventCallback<T>? callback = null,
        bool clearRoom = false)
    {
        if (applyChange is not null)
        {
            await applyChange(value);
        }

        if (ChatState.RoomView is not null &&
            callback.HasValue &&
            callback.Value.HasDelegate)
        {
            if (typeof(T) == typeof(ChatRoom))
            {
                await callback.Value.InvokeAsync((T)(object)ChatState.RoomView.Room);
            }
            else if (typeof(T) == typeof(ChatRoomEventArgs))
            {
                var args = new ChatRoomEventArgs(ChatState.RoomView.UserState!);
                await callback.Value.InvokeAsync((T)(object)args);
            }
        }

        if (clearRoom)
        {
            ChatState.RoomView = null;
            _selectedRoom = null;
        }

        await RefreshDataAsync();
    }

    /// <summary>
    /// Retrieves the available actions for the sleek dial based on current view state and permissions.
    /// </summary>
    /// <returns>A collection of tuples containing the localized title, icon, and asynchronous action handler for each available
    /// dial action.</returns>
    private IEnumerable<(string Title, Icon Icon, Func<Task> Action)> GetSleekDialActions()
    {
        if (OnNewChatGroup is not null &&
            _listView == RoomListView.Normal)
        {
            yield return (Localizer[LanguageResource.CX_Chat_Room_NewGroup], new Size24.Add(), OnNewChatGroupAsync);
        }

        if (OnBlockChanged.HasDelegate)
        {
            if (_listView != RoomListView.Blocked)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowBlockedRooms], new Size24.PresenceBlocked(), OnUnblockRoomsAsync);
            }
            else
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowAllRooms], new Size24.PresenceAvailable(), OnShowRoomsAsync);
            }
        }

        if (OnHideChanged.HasDelegate)
        {
            if (_listView != RoomListView.Hidden)
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowHiddenRooms], new Size24.Eye(), OnUnhideRoomsAsync);
            }
            else
            {
                yield return (Localizer[LanguageResource.CX_Chat_Room_ShowAllRooms], new Size24.EyeOff(), OnShowRoomsAsync);
            }
        }

        if (OnArchiveChanged.HasDelegate)
        {
            if (_listView != RoomListView.Archived)
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
        if (ChatState.RoomView is null)
        {
            yield break;
        }

        var room = ChatState.RoomView;

        if (_listView == RoomListView.Normal)
        {
            if (OnRename.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Rename_Menu],
                    Icon = new Size24.Edit(),
                    Action = OnRenameAsync
                };
            }

            if (OnPinChanged.HasDelegate)
            {
                var state = ChatState.RoomView.UserState;
                var isPinned = state?.IsPinned ?? false;

                yield return new ChatRoomAction
                {
                    Label = isPinned ? Localizer[LanguageResource.CX_Chat_Room_Unpin] : Localizer[LanguageResource.CX_Chat_Room_Pin],
                    Icon = isPinned ? new Size24.PinOff() : new Size24.Pin(),
                    Action = isPinned ? OnUnpinAsync : OnPinAsync
                };
            }

            if (OnMuteChanged.HasDelegate)
            {
                var state = ChatState.RoomView.UserState;
                var isMuted = state?.IsMuted ?? false;

                yield return new ChatRoomAction
                {
                    Label = isMuted ? Localizer[LanguageResource.CX_Chat_Room_Unmute] : Localizer[LanguageResource.CX_Chat_Room_Mute],
                    Icon = isMuted ? new Size24.Speaker2() : new Size24.SpeakerMute(),
                    Action = isMuted ? OnUnmuteAsync : OnMuteAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (OnArchiveChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Archive],
                    Icon = new Size24.Archive(),
                    Action = OnArchiveAsync
                };
            }

            if (OnBlockChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Block],
                    Icon = new Size24.PresenceBlocked(),
                    Action = OnBlockAsync
                };
            }

            if (OnHideChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Hide],
                    Icon = new Size24.EyeOff(),
                    Action = OnHideAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (OnDelete.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
        else if (_listView == RoomListView.Blocked)
        {
            if (OnArchiveChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Archive],
                    Icon = new Size24.Archive(),
                    Action = OnArchiveAsync
                };
            }

            if (OnBlockChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Unblock],
                    Icon = new Size24.PresenceAvailable(),
                    Action = OnUnblockAsync
                };
            }

            if (OnHideChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Hide],
                    Icon = new Size24.Eye(),
                    Action = OnHideAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (OnDelete.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
        else if (_listView == RoomListView.Hidden)
        {
            if (OnArchiveChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Archive],
                    Icon = new Size24.Archive(),
                    Action = OnArchiveAsync
                };
            }

            if (OnHideChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Unhide],
                    Icon = new Size24.Eye(),
                    Action = OnUnhideAsync
                };
            }

            if (OnBlockChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Block],
                    Icon = new Size24.PresenceBlocked(),
                    Action = OnBlockAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (OnDelete.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Delete],
                    Icon = new Size24.Delete(),
                    Action = OnDeleteAsync
                };
            }
        }
        else if (_listView == RoomListView.Archived)
        {
            if (OnArchiveChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Unarchive],
                    Icon = new Size24.ArchiveArrowBack(),
                    Action = OnUnarchiveAsync
                };
            }

            if (OnBlockChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Block],
                    Icon = new Size24.PresenceBlocked(),
                    Action = OnBlockAsync
                };
            }

            if (OnHideChanged.HasDelegate)
            {
                yield return new ChatRoomAction
                {
                    Label = Localizer[LanguageResource.CX_Chat_Room_Hide],
                    Icon = new Size24.EyeOff(),
                    Action = OnHideAsync
                };
            }

            yield return ChatRoomAction.Separator;

            if (OnDelete.HasDelegate)
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
    /// <param name="roomView">The chat room that was clicked.</param>
    private void HandleRoomClick(ChatRoomView roomView)
    {
        ChatState.RoomView = roomView;

        _selectedRoom = Invariant.ToString(roomView.Room.Id);
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
            RoomListView.Normal => Localizer[LanguageResource.CX_Chat_Room_Normal],
            RoomListView.Blocked => Localizer[LanguageResource.CX_Chat_Room_Blocked],
            RoomListView.Archived => Localizer[LanguageResource.CX_Chat_Room_Archived],
            RoomListView.Hidden => Localizer[LanguageResource.CX_Chat_Room_Hidden],
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
            ChatState.RoomView = null;
            _selectedRoom = null;
            _selectedRooms.Clear();

            if (OwnerChanged.HasDelegate)
            {
                await OwnerChanged.InvokeAsync(Owner);
            }

            await RefreshDataAsync();
        }
    }

    private async Task SetLoadingAsync(bool loading)
    {
        IsLoading = loading;
        await InvokeAsync(StateHasChanged);
        await Task.Delay(50);
    }

    private async ValueTask<ItemsProviderResult<ChatRoomView>> GetRoomsAsync(ItemsProviderRequest request)
    {
        await SetLoadingAsync(true);

        var result = await _chatOrchestrator!.ProvideAsync(LoadRoomsAsync, request);

        await SetLoadingAsync(false);

        return result;
    }
}
