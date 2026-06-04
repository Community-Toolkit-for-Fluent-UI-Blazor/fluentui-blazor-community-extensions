using System.Linq.Expressions;
using System.Text;
using FluentUI.Blazor.Community.Components.Chat.Engine;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;
using FluentUI.Blazor.Community.Components.Clipboard;
using FluentUI.Blazor.Community.Components.Components.Chat;
using FluentUI.Blazor.Community.Components.Emojis;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Messages;

/// <summary>
/// Represents a component that displays a list of chat messages in a chat interface.
/// </summary>
public partial class ChatView<TItem>
    : FluentComponentBase
{
    /// <summary>
    /// Represents the virtualizer.
    /// </summary>
    private Virtualize<ChatMessage>? _virtualizeMessageList;

    /// <summary>
    /// Value indicating if the owner of the view has changed since the last render.
    /// </summary>
    private bool _hasOwnerChanged;

    /// <summary>
    /// Value indicating if the emoji popover is visible.
    /// </summary>
    private bool _isEmojiPopoverVisible;

    /// <summary>
    /// Represents the message draft.
    /// </summary>
    private ChatMessageDraft? _chatDraft;

    /// <summary>
    /// Value indicating if the message is currently in editing mode.
    /// </summary>
    private bool _isEdit;

    /// <summary>
    /// Value indicating if the message is currently in reply mode.
    /// </summary>
    private bool _isReply;

    /// <summary>
    /// Value indicating if the message is sending.
    /// </summary>
    private bool _isSending;

    /// <summary>
    /// Value indicating whether the total message count should be refreshed.
    /// </summary>
    private bool _refreshTotalMessageCount = true;

    /// <summary>
    /// Value representing the total count of messages in the current chat room.
    /// </summary>
    private int _totalMessageCount;

    /// <summary>
    /// Cancellation token source for canceling asynchronous operations.
    /// </summary>
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatView{TItem}"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatView(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the state of the chat.
    /// </summary>
    [Inject]
    private ChatState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state of the chat room.
    /// </summary>
    [Inject]
    private ChatRoomState RoomState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state of the chat messages.
    /// </summary>
    [Inject]
    private ChatMessageState MessageState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state of the chat message dynamic properties.
    /// </summary>
    [Inject]
    private ChatMessageDynamicState DynamicState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state of the chat room dynamic properties.
    /// </summary>
    [Inject]
    private ChatRoomDynamicState RoomDynamicState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the engine to manage the chat.
    /// </summary>
    [Inject]
    private ChatEngine ChatEngine { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state of the device info.
    /// </summary>
    [Inject]
    private DeviceInfoState DeviceState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the service for toast.
    /// </summary>
    [Inject]
    private IToastService ToastService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the translation client.
    /// </summary>
    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

    /// <summary>
    /// Gets or sets the clipboard module.
    /// </summary>
    [Inject]
    private IClipboard Clipboard { get; set; } = default!;

    /// <summary>
    /// Gets or sets the fragment to render when the room is undefined.
    /// </summary>
    [Parameter]
    public RenderFragment? RoomUndefinedContent { get; set; }

    /// <summary>
    /// Gets or sets the owner of the view.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the callback which is invoked when the owner of the view has changed.
    /// </summary>
    [Parameter]
    public EventCallback<ChatUser?> OwnerChanged { get; set; }

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
    /// Gets or sets a value indicating if the chat allows to react to a message.
    /// </summary>
    [Parameter]
    public bool IsReactEnabled { get; set; } = true;

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
    public Icon? MediaIcon { get; set; } = new Size20.Image();

    /// <summary>
    /// Gets or sets the icon to be used for the emoji button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? EmojiIcon { get; set; } = new Size20.Emoji();

    /// <summary>
    /// Gets or sets the icon to be used for the gift button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? GiftIcon { get; set; } = new Size20.Gift();

    /// <summary>
    /// Gets or sets the icon to be used for the send button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? SendIcon { get; set; } = new Size20.Send();

    /// <summary>
    /// Gets or sets the icon to be used for the cancel edit button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? DismissIcon { get; set; } = new Size20.Dismiss();

    /// <summary>
    /// Gets or sets the icon to be used for the commit edit button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? CheckmarkIcon { get; set; } = new Size20.Checkmark();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when not recording in chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MicroIcon { get; set; } = new Size20.Mic();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when recording in chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MicroOffIcon { get; set; } = new Size20.MicOff();

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
    public EventCallback<ChatMessageReactRequest> OnReactMessage { get; set; }

    /// <summary>
    /// Occurs when the gift button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnGift"/> when completed.</returns>
    private async Task OnAddGiftAsync()
    {
        if (OnGift.HasDelegate)
        {
            await OnGift.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the import media button is clicked.
    /// </summary>
    /// <returns>Returns a task which ask how to import the media when completed.</returns>
    private async Task OnImportMediaAsync()
    {
        switch (ImportOptions)
        {
            case ChatFileImportOptions.Cloud:
                {
                    await ShowCloudDialogAsync();
                }

                break;

            case ChatFileImportOptions.Local:
                {
                    await ShowInputFileDialogAsync();
                }

                break;

            case ChatFileImportOptions.Both:
                {
                    var dialog = await DialogService.ShowDialogAsync<ChatMediaImporterDialog>(a =>
                    {
                        a.Header.Title = Localizer[LanguageResource.CX_Chat_Message_Import_FileSelectorDialogTitle];
                        a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Message_Import_DialogCancel];
                    });

                    if (!dialog.Cancelled &&
                        dialog.Value is ChatMediaImporterDialogResult r)
                    {
                        switch (r)
                        {
                            case ChatMediaImporterDialogResult.Cloud:
                                await ShowCloudDialogAsync();
                                break;

                            case ChatMediaImporterDialogResult.Local:
                                await ShowInputFileDialogAsync();
                                break;
                        }
                    }
                }

                break;
        }
    }

    /// <summary>
    /// Shows the cloud import dialog in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which import the selected cloud files when completed.</returns>
    private async Task ShowCloudDialogAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<CloudFileManagerDialog<TItem>>(a =>
        {
            a.Header.Title = Localizer[LanguageResource.CX_Chat_Message_Import_FileSelectorDialogTitle];
            a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Message_Import_DialogCancel];
        });

        if (!dialog.Cancelled &&
            dialog.Value is IEnumerable<ChatFileEventArgs> e)
        {
            if (_chatDraft is not null)
            {
                if (!AppendFiles)
                {
                    _chatDraft.SelectedChatFiles.Clear();
                }

                _chatDraft.SelectedChatFiles.AddRange(e);
            }
        }
    }

    /// <summary>
    /// Shows an input file dialog in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which import the selected files when completed.</returns>
    private async Task ShowInputFileDialogAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<ChatFileUploaderDialog>(a =>
        {
            a.Header.Title = Localizer[LanguageResource.CX_Chat_Message_Import_FileSelectorDialogTitle];
        });

        if (!dialog.Cancelled &&
            dialog.Value is IEnumerable<ChatFileEventArgs> e)
        {
            if (_chatDraft is not null)
            {
                if (!AppendFiles)
                {
                    _chatDraft.SelectedChatFiles.Clear();
                }

                _chatDraft.SelectedChatFiles.AddRange(e);
            }
        }
    }

    /// <summary>
    /// Occurs when the selected room has changed.
    /// </summary>
    /// <param name="sender">Object which invokes the method.</param>
    /// <param name="e">Event associated to this method.</param>
    private async void OnRoomChanged(object? sender, System.EventArgs e)
    {
        _chatDraft = State.GetDraft();
        _chatDraft?.SenderId = Owner?.Id ?? 0;
        _totalMessageCount = 0;
        _refreshTotalMessageCount = true;
        await RefreshDataAsync();
    }

    /// <summary>
    /// Occurs when the sending message button is clicked.
    /// </summary>
    /// <returns>Returns a task which build and send the message at all other users.</returns>
    private async Task OnAddMessageAsync()
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();
        _isSending = true;
        await InvokeAsync(StateHasChanged);

        List<ChatMessage> messages = [];

        if (IsTranslationEnabled)
        {
            await TranslateTextAsync();
        }

        if (State.Room is not null &&
            _chatDraft is not null &&
            Owner is not null)
        {
            if (!string.IsNullOrEmpty(_chatDraft.Text))
            {
                _chatDraft.AddCultureText(Owner.CultureName!, [_chatDraft.Text]);
            }

            (var Messages, var Files) = await _chatDraft.BuildAsync(State.Room.Id, Owner, MessageSplitOption);

            if (OnCreate is not null)
            {
                var createdMessages = await OnCreate(new(Messages, Files, _cts.Token));

                foreach (var item in createdMessages.Files)
                {
                    DynamicState.AppendFile(State.Room, item.MessageId, item);
                }

                messages.AddRange(createdMessages.Messages);
                _chatDraft.Clear();
                _isReply = false;
                _refreshTotalMessageCount = true;
                await RefreshDataAsync();
                await SendMessagesAsync(messages);
            }
        }

        _isSending = false;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Translates the text in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which translates the text into all users languages when completed.</returns>
    private async Task TranslateTextAsync()
    {
        if (State.Room is null ||
            Owner is null ||
            _chatDraft is null ||
            !IsTranslationEnabled)
        {
            return;
        }

        var translationClient = ServiceProvider.GetRequiredService<ITranslationClient>();
        var cultures = RoomDynamicState.GetUsersBut(State.Room.Id, Owner.Id)
                                       .Select(x => x.CultureName)
                                       .Distinct()
                                       .ToList();

        if (cultures.Count > 0 &&
            !string.IsNullOrEmpty(_chatDraft.Text) &&
            translationClient.IsConfigurationValid)
        {
            var result = await translationClient.TranslateAsync(
                _chatDraft.Text,
                Owner.CultureName,
                cultures
            );

            foreach (var item in result)
            {
                _chatDraft?.AddCultureText(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// Send the <paramref name="messages"/> in an asynchronous way.    
    /// </summary>
    /// <param name="messages">Messages to send.</param>
    /// <returns>Returns a task which send the messages when completed.</returns>
    private async Task SendMessagesAsync(List<ChatMessage> messages)
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();
        var count = messages.Count;

        if (State.Room is not null &&
            count > 0)
        {
            if (count == 1)
            {
                await ChatEngine.SendNewMessageAsync(State.Room, messages[0].Id, _cts.Token);
            }
            else
            {
                await ChatEngine.SendNewMessagesAsync(State.Room, messages.Select(x => x.Id), _cts.Token);
            }
        }
    }

    /// <summary>
    /// Refresh the data in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which refresh the view when completed.</returns>
    private async Task RefreshDataAsync()
    {
        if (_virtualizeMessageList != null)
        {
            await _virtualizeMessageList.RefreshDataAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Gets the items to render in the view.
    /// </summary>
    /// <param name="request">Request to use to retrieve items.</param>
    /// <returns>Returns an <see cref="ItemsProviderResult{TItem}"/> which contains the messages to render.</returns>
    private async ValueTask<ItemsProviderResult<ChatMessage>> GetItemsAsync(ItemsProviderRequest request)
    {
        while (State.IsLoading)
        {
            await Task.Delay(10);
        }

        State.IsLoading = true;

        if (Owner is null ||
            State.Room is null ||
            ItemsProvider is null ||
            CountProvider is null)
        {
            State.IsLoading = false;
            return new();
        }

        var filter = PredicateBuilder<ChatMessage>.True;

        if (!ShowDeletedMessages)
        {
            filter = PredicateBuilder<ChatMessage>.And(x => !x.IsDeleted, Filter);
        }

        filter = PredicateBuilder<ChatMessage>.And(filter, x => x.RoomId == State.Room.Id);

        if (_refreshTotalMessageCount ||
            _totalMessageCount == 0)
        {
            var current = await CountProvider(new ChatMessageCountRequest(filter, request.CancellationToken));
            _refreshTotalMessageCount = false;

            if (current != _totalMessageCount)
            {
                _totalMessageCount = current;
            }
        }

        if (_totalMessageCount > 0 &&
            request.Count > 0)
        {
            var list = await ItemsProvider(new(filter, request.StartIndex, request.Count, request.CancellationToken));

            if (IsReactEnabled && ReactionsProvider is not null)
            {
                var reactions = await ReactionsProvider(new([.. list.Select(x => x.Id)], request.CancellationToken));

                foreach (var item in reactions)
                {
                    DynamicState.AppendReactions(State.Room, item.MessageId, item);
                }
            }

            if (ReadUserStateProvider is not null)
            {
                var readStates = await ReadUserStateProvider(new([.. list.Select(x => x.Id)], request.CancellationToken));
                DynamicState.SetReadStates(State.Room, Owner.Id, readStates, RoomDynamicState.GetUsers(State.Room.Id));
            }

            if (FilesProvider is not null)
            {
                var files = await FilesProvider(new([.. list.Select(x => x.Id)], request.CancellationToken));
                DynamicState.SetFiles(State.Room, files);
            }

            State.IsLoading = false;
            return new(list, _totalMessageCount);
        }

        State.IsLoading = false;
        return new();
    }

    /// <summary>
    /// Occurs when the edit button is clicked.
    /// </summary>
    /// <param name="message">Message to edit.</param>
    private void OnEdit(ChatMessage message)
    {
        _isEdit = true;

        if (Owner is not null)
        {
            _chatDraft?.SetEditMessage(Owner, message);
        }
    }

    /// <summary>
    /// Occurs when an edit is cancelled.
    /// </summary>
    private void OnCancelEdit()
    {
        _isEdit = false;
        _chatDraft?.ClearEditMessage();
    }

    /// <summary>
    /// Occurs when the edited message is validated.
    /// </summary>
    /// <returns>Returns a task which edit the message when completed.</returns>
    private async Task OnEditMessageAsync()
    {
        if (Owner is not null &&
            State.Room is not null &&
            _chatDraft is not null &&
            !string.IsNullOrEmpty(_chatDraft.Text) &&
            OnEditMessage is not null)
        {
            var message = _chatDraft.GetEditMessage();

            if (message is null)
            {
                return;
            }

            if (_cts is not null)
            {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            _isEdit = false;
            message = message with
            {
                EditedDate = DateTimeOffset.UtcNow,
            };

            var section = message.Sections.FirstOrDefault(s => s.CultureId == Owner.CultureId);
            section?.Content = _chatDraft.Text;

            await OnEditMessage(new(
                message,
                _cts.Token
            ));

            _chatDraft.ClearEditMessage();
            await ChatEngine.SendEditedMessageAsync(State.Room, message.Id, _cts.Token);
            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs when the message is tapped.
    /// </summary>
    /// <param name="message">Tapped message.</param>
    /// <returns>Returns a task which show the message in bigger view.</returns>
    private async Task OnTappedAsync(ChatMessage message)
    {
        await DialogService.ShowDialogAsync<ChatMessageDialog>(a =>
        {
            //a.Header.CloseAction.Visible = true;
            a.Width = "90%";
            a.Height = "90%";
            a.Parameters.Add(nameof(ChatMessageDialog.Message), message);
            a.Footer.SecondaryAction.Label = Localizer[LanguageResource.CX_Chat_Cancel];
            a.Footer.PrimaryAction.Visible = false;
        });
    }

    /// <summary>
    /// Occurs when a message must be deleted.
    /// </summary>
    /// <param name="message">Message to delete.</param>
    /// <returns>Returns a task which deletes the message when completed.</returns>
    private async Task OnDeleteAsync(ChatMessage message)
    {
        if (State.Room is not null &&
            OnDelete is not null)
        {
            if (_cts is not null)
            {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            _cts = new CancellationTokenSource();
            _refreshTotalMessageCount = true;
            await OnDelete(message, _cts.Token);
            await RefreshDataAsync();
            await ChatEngine.SendDeletedMessageAsync(State.Room, message.Id, _cts.Token);
        }
    }

    /// <summary>
    /// Occurs when a message must be copied.
    /// </summary>
    /// <param name="message">Message to copy.</param>
    /// <returns>Returns a task which copy the content of the message when completed.</returns>
    private async Task OnCopyAsync(ChatMessage message)
    {
        if (Owner is not null)
        {
            var text = message.Sections.FirstOrDefault(x => x.CultureId == Owner.CultureId)?.Content;

            if (string.IsNullOrEmpty(text))
            {
                text = message.Sections[0].Content;
            }

            var result = await Clipboard.WriteTextAsync(text);
            var bodyText = result ? Localizer[LanguageResource.CX_Chat_Message_Text_Copied] : Localizer[LanguageResource.CX_Chat_Message_Text_Copy_Failed];
            var intent = result ? ToastIntent.Success : ToastIntent.Error;
            var title = result ? Localizer[LanguageResource.CX_Chat_Message_Text_Copied_Title] : Localizer[LanguageResource.CX_Chat_Message_Text_Copy_Failed_Title];

            await ToastService.ShowToastAsync(a =>
            {
                a.Intent = intent;
                a.Title = title;
                a.Body = bodyText;
            });
        }
    }

    /// <summary>
    /// Occurs when the reply message is dismissed.
    /// </summary>
    private void OnDismiss()
    {
        _isReply = false;
        _chatDraft?.ClearReplyMessage();
    }

    /// <summary>
    /// Occurs when a message is replied.
    /// </summary>
    /// <param name="message">Replied message.</param>
    private void OnReply(ChatMessage message)
    {
        _isReply = true;
        _chatDraft?.SetReplyMessage(message);
    }

    /// <summary>
    /// Occurs when a message was reacted by a user.
    /// </summary>
    /// <param name="e">Event args associated to the method.</param>
    /// <returns>Returns a task which reacts to a message when completed.</returns>
    private async Task OnReactAsync(ChatMessageReactEventArgs e)
    {
        if (State.Room is not null &&
            Owner is not null &&
            !string.IsNullOrEmpty(e.Reaction))
        {
            if (_cts is not null)
            {
                await _cts.CancelAsync();
                _cts.Dispose();
            }

            if (OnReactMessage.HasDelegate)
            {
                var request = new ChatMessageReactRequest(Owner.Id, e.Message.Id, e.Reaction);
                await OnReactMessage.InvokeAsync(request);

                if (request.Reaction is null)
                {
                    return;
                }

                _cts = new CancellationTokenSource();
                var allReaactions = DynamicState.GetReactions(State.Room, e.Message.Id).ToList();
                var reactToReplace = allReaactions.Find(x => x.UserReactedById == Owner.Id);

                if (reactToReplace is null)
                {
                    allReaactions.Add(request.Reaction);
                }
                else
                {
                    reactToReplace.Emoji = request.Reaction.Emoji;
                }

                DynamicState.SetReactions(State.Room, e.Message.Id, allReaactions);
                await ChatEngine.SendReactedMessageAsync(State.Room, e.Message.Id, e.Reaction, _cts.Token);
                await InvokeAsync(StateHasChanged);
            }
        }
    }

    /// <summary>
    /// Dismisses the file specified by <paramref name="e"/>.
    /// </summary>
    /// <param name="e">File to remove.</param>
    private void OnDismissFile(ChatFileEventArgs e)
    {
        _chatDraft?.SelectedChatFiles.Remove(e);
    }

    /// <summary>
    /// Adds the <paramref name="emoji"/> at the cursor in the text area.
    /// </summary>
    /// <param name="emoji">Emoji to add to the text.</param>
    private void OnAddEmoji(FluentCxEmoji emoji)
    {
        if (_chatDraft is null)
        {
            return;
        }

        var builder = new StringBuilder();
        builder.Append(_chatDraft.Text);
        builder.Append(emoji.Unicode);

        _chatDraft.Text = builder.ToString();
    }

    private void OnUpdated(object? sender, System.EventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        State.RoomChanged += OnRoomChanged;

        RoomState.RoomsChanged += OnUpdated;
        RoomState.RoomUpdated += OnUpdated;

        MessageState.MessageUpdated += OnUpdated;
        MessageState.MessageRemoved += OnUpdated;

        DynamicState.ReadStateUpdated += OnUpdated;
        DynamicState.ReactionsUpdated += OnUpdated;
        DynamicState.FilesUpdated += OnUpdated;

        _chatDraft = State.GetDraft();

        ChatEngine.SetFilesProvider(FilesProvider, IsMediaInsertionAllowed)
                  .SetMessageItemProvider(ItemRetrieveProvider)
                  .SetReactionsProvider(ReactionsProvider, IsReactEnabled)
                  .SetUserStateProvider(ReadUserStateProvider);
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        State.RoomChanged -= OnRoomChanged;
        RoomState.RoomsChanged -= OnUpdated;
        RoomState.RoomUpdated -= OnUpdated;

        MessageState.MessageUpdated -= OnUpdated;
        MessageState.MessageRemoved -= OnUpdated;

        DynamicState.ReadStateUpdated -= OnUpdated;
        DynamicState.ReactionsUpdated -= OnUpdated;
        DynamicState.FilesUpdated -= OnUpdated;

        GC.SuppressFinalize(this);

        return base.DisposeAsync();
    }

    private void OnAudioReady(byte[] data)
    {
        var fileName = Path.GetRandomFileName();
        fileName = Path.ChangeExtension(fileName, "webm");
        _chatDraft?.SelectedChatFiles.Add(new(fileName, "audio/webm", data, true));
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasOwnerChanged)
        {
            _hasOwnerChanged = false;

            if (OwnerChanged.HasDelegate)
            {
                await OwnerChanged.InvokeAsync(Owner);
            }

            await RefreshDataAsync();
        }
    }
}
