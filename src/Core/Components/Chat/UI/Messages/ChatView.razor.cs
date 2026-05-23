using System.Linq.Expressions;
using System.Text;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;
using FluentUI.Blazor.Community.Components.Chat.UI.Writers;
using FluentUI.Blazor.Community.Components.Clipboard;
using FluentUI.Blazor.Community.Components.Components.Chat;
using FluentUI.Blazor.Community.Components.Emojis;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

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
    private Virtualize<IChatMessage>? _virtualizeMessageList;

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
    private ChatState ChatState { get; set; } = default!;

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
    private ITranslationClient? TranslationClient { get; set; }

    /// <summary>
    /// Gets or sets the clipboard module.
    /// </summary>
    [Inject]
    private IClipboard Clipboard { get; set; } = default!;

    /// <summary>
    /// Gets or sets the owner of the view.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

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
    public RenderFragment<IChatMessage>? MessageTemplate { get; set; }

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
    public EmojiDialogSettings EmojiSettings { get; set; } = new();

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
    /// Gets or sets a value indicating if the <see cref="ChatMessageWriter"/> is visible or not.
    /// </summary>
    [Parameter]
    public bool IsMessageWriterVisible { get; set; } = true;

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
                        a.Footer.PrimaryAction.Label = Localizer[LanguageResource.CX_Chat_Message_Import_DialogCancel];
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
    private async void OnRoomChanged(object? sender, ChatRoom? e)
    {
        _chatDraft = ChatState.GetDraft();
        await RefreshDataAsync();
    }

    /// <summary>
    /// Occurs when the sending message button is clicked.
    /// </summary>
    /// <returns>Returns a task which build and send the message at all other users.</returns>
    private async Task OnAddMessageAsync()
    {
        _isSending = true;
        await InvokeAsync(StateHasChanged);

        List<IChatMessage> messages = [];

        if (IsTranslationEnabled)
        {
            await TranslateTextAsync();
        }

        if (ChatState.Room is not null &&
            _chatDraft is not null &&
            Owner is not null)
        {
            if (!string.IsNullOrEmpty(_chatDraft.Text))
            {
                _chatDraft.AddCultureText(Owner.CultureName!, [_chatDraft.Text]);
            }

            messages.AddRange(await ChatMessageService.CreateMessagesAsync(new(
                ChatState.Room.Id,
                Owner,
                _chatDraft,
                MessageSplitOption,
                IsTranslationEnabled
            )));

            _refreshTotalMessageCount = true;

            _chatDraft.Clear();
            _isReply = false;
            await RefreshDataAsync();
        }

        if (messages.Count == 1)
        {
            await SendMessageAsync(messages[0]);
        }
        else
        {
            await SendMessageAsync(messages);
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
        if (TranslationClient is null ||
            ChatState.Room is null ||
            Owner is null ||
            _chatDraft is null)
        {
            return;
        }

        var cultures = ChatState.Room.Users.Select(x => x.CultureName)
                                           .Except([Owner.CultureName])
                                           .Distinct()
                                           .ToList();

        if (cultures.Count > 0 &&
            !string.IsNullOrEmpty(_chatDraft.Text) &&
            TranslationClient.IsConfigurationValid)
        {
            var result = await TranslationClient.TranslateAsync(
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
    private async Task SendMessageAsync(IEnumerable<IChatMessage> messages)
    {
        if (_messageService is not null &&
            ChatState.Room is not null &&
            messages.Any())
        {
            await _messageService.SendAsync(ChatMessageListViewConstants.SendMessagesAsync, ChatState.Room.Id, messages.Select(x => x.Id), ChatState.Room.GetUsersBut(Owner));
        }
    }

    /// <summary>
    /// Send the <paramref name="message"/> in an asynchronous way.    
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns>Returns a task which send the message when completed.</returns>
    private async Task SendMessageAsync(IChatMessage message)
    {
        if (_messageService is not null &&
            ChatState.Room is not null)
        {
            await _messageService.SendAsync(ChatMessageListViewConstants.SendMessageAsync, ChatState.Room.Id, message.Id, ChatState.Room.GetUsersBut(Owner));
        }
    }

    /// <summary>
    /// Send the fact that the message was read in an asynchronous way.
    /// </summary>
    /// <param name="roomId">Identifier of the room.</param>
    /// <param name="messageId">Identifier of the message.</param>
    /// <param name="userIdCollection">List of users which read the message.</param>
    /// <returns>Returns a task which send the fact that the message was read by <paramref name="userIdCollection"/> users.</returns>
    private async Task SendMessageReadAsync(
        long roomId,
        long messageId,
        IEnumerable<long> userIdCollection)
    {
        if (_messageService is not null)
        {
            await _messageService.SendAsync(ChatMessageListViewConstants.MessageReadAsync, roomId, messageId, userIdCollection);
        }
    }

    /// <summary>
    /// Occurs when a message was received.
    /// </summary>
    /// <param name="roomId">Identifier of the room.</param>
    /// <param name="messageIdCollection">Identifier of the message.</param>
    /// <returns>Returns a task which updates the view when completed.</returns>
    private async Task OnReceivedMessagesAsync(long roomId, IEnumerable<long> messageIdCollection)
    {
        var room = ChatState.Room;

        if (room?.Id == roomId &&
            Owner is not null)
        {
            foreach (var messageId in messageIdCollection)
            {
                var chatMessage = await ChatMessageService.GetMessageAsync(roomId, messageId);

                if (chatMessage is not null)
                {
                    _refreshTotalMessageCount = true;
                    await ChatMessageService.SetReadStateAsync(roomId, chatMessage.Id, Owner, true);
                    await SendMessageReadAsync(room.Id, chatMessage.Id, room.GetUsersBut(Owner).Select(x => x.Id));
                }
            }

            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs when a message was received.
    /// </summary>
    /// <param name="roomId">Identifier of the room.</param>
    /// <param name="messageId">Identifier of the message.</param>
    /// <returns>Returns a task which updates the view when completed.</returns>
    private async Task OnReceivedMessageAsync(long roomId, long messageId)
    {
        var room = ChatState.Room;

        if (room?.Id == roomId &&
            Owner is not null)
        {
            var chatMessage = await ChatMessageService.GetMessageAsync(roomId, messageId);

            if (chatMessage is not null)
            {
                _refreshTotalMessageCount = true;
                await ChatMessageService.SetReadStateAsync(roomId, chatMessage.Id, Owner, true);
                await SendMessageReadAsync(room.Id, chatMessage.Id, room.GetUsersBut(Owner).Select(x => x.Id));
            }

            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs when a message was deleted.
    /// </summary>
    /// <param name="roomId">Identifier of the room.</param>
    /// <returns>Returns a task which refresh the view when completed.</returns>
    private async Task OnDeletedMessageAsync(long roomId)
    {
        if (ChatState.Room?.Id == roomId)
        {
            await RefreshDataAsync();
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
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Gets the items to render in the view.
    /// </summary>
    /// <param name="request">Request to use to retrieve items.</param>
    /// <returns>Returns an <see cref="ItemsProviderResult{TItem}"/> which contains the messages to render.</returns>
    private async ValueTask<ItemsProviderResult<IChatMessage>> GetItemsAsync(ItemsProviderRequest request)
    {
        while (ChatState.IsLoading)
        {
            await Task.Delay(10);
        }

        ChatState.IsLoading = true;

        if (Owner is null ||
            ChatState.Room is null ||
            ChatMessageService is null)
        {
            ChatState.IsLoading = false;
            return new();
        }

        var filter = PredicateBuilder<IChatMessage>.True;

        if (!ShowDeletedMessages)
        {
            filter = PredicateBuilder<IChatMessage>.And(x => !x.IsDeleted, Filter);
        }

        if (_refreshTotalMessageCount || _totalMessageCount == 0)
        {
            var current = await ChatMessageService.MessageCountAsync(new(ChatState.Room.Id, Owner.Id, filter));
            _refreshTotalMessageCount = false;
            _scrollToBottom = true;

            if (current != _totalMessageCount)
            {
                _totalMessageCount = current;
            }
        }

        if (_totalMessageCount > 0 &&
            request.Count > 0)
        {
            var list = await ChatMessageService.GetMessageListAsync(new(ChatState.Room.Id, Owner.Id, request.StartIndex, request.Count, filter));

            ChatState.IsLoading = false;
            return new(list, _totalMessageCount);
        }

        ChatState.IsLoading = false;
        return new();
    }

    /// <summary>
    /// Pin or unpin the message specified by <paramref name="id"/> in an asynchronous way.
    /// </summary>
    /// <param name="id">Identifier of the message.</param>
    /// <returns>Returns a task which pin or unpin the message.</returns>
    private async Task OnPinOrUnpinAsync(long id)
    {
        if (ChatState.Room?.Id == id)
        {
            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs after a react was received.
    /// </summary>
    /// <param name="roomId">Identifier of the room.</param>
    /// <returns>Returns a task which refresh the view when completed.</returns>
    private async Task OnReactedMessageAsync(long roomId)
    {
        if (Owner is not null &&
            ChatState.Room?.Id == roomId)
        {
            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs after a message was read.
    /// </summary>
    /// <param name="roomId">Identifier of the room.</param>
    /// <param name="messageId">Identifier of the message.</param>
    /// <returns>Returns a task which refresh the view when completed.</returns>
    private async Task OnMessageReadAsync(long roomId, long messageId)
    {
        if (Owner is not null &&
            ChatState.Room?.Id == roomId)
        {
            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs when the edit button is clicked.
    /// </summary>
    /// <param name="message">Message to edit.</param>
    private void OnEdit(IChatMessage message)
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
        var message = _chatDraft?.GetEditMessage();

        if (Owner is not null &&
            ChatState.Room is not null &&
            _chatDraft is not null &&
            message is not null &&
            !string.IsNullOrEmpty(_chatDraft.Text))
        {
            _isEdit = false;
            await ChatMessageService.EditMessageAsync(new(ChatState.Room.Id, message, Owner, _chatDraft.Text));
            _chatDraft.ClearEditMessage();
            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Occurs when the message is tapped.
    /// </summary>
    /// <param name="message">Tapped message.</param>
    /// <returns>Returns a task which show the message in bigger view.</returns>
    private async Task OnTappedAsync(IChatMessage message)
    {
        var dialog = await DialogService.ShowDialogAsync<ChatMessageViewer>(new ChatMessageViewerContent(Owner!, message, ChatMessageListLabels.LoadingLabel), new DialogParameters()
        {
            Title = ChatMessageListLabels.MessageViewer,
            PreventDismissOnOverlayClick = true,
            DismissTitle = ChatMessageListLabels.DialogCancel,
            PrimaryAction = ChatMessageListLabels.DialogCancel,
            Width = "90%",
            Height = "90%"
        });

        await dialog.Result;
    }

    /// <summary>
    /// Occurs when a message must be deleted.
    /// </summary>
    /// <param name="message">Message to delete.</param>
    /// <returns>Returns a task which deletes the message when completed.</returns>
    private async Task OnDeleteAsync(IChatMessage message)
    {
        if (_messageService is not null &&
            ChatState.Room is not null)
        {
            await ChatMessageService.DeleteAsync(ChatState.Room.Id, message);
            await RefreshDataAsync();
            await _messageService.SendAsync(ChatMessageListViewConstants.DeleteMessageAsync, ChatState.Room.Id, ChatState.Room.GetUsersBut(Owner));
        }
    }

    /// <summary>
    /// Occurs when a message must be copied.
    /// </summary>
    /// <param name="message">Message to copy.</param>
    /// <returns>Returns a task which copy the content of the message when completed.</returns>
    private async Task OnCopyAsync(IChatMessage message)
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
    private void OnReply(IChatMessage message)
    {
        _isReply = true;
        _chatDraft?.SetReplyMessage(message);
    }

    /// <summary>
    /// Occurs when a message is pined or unpined.
    /// </summary>
    /// <param name="e">Event args associated to the method.</param>
    /// <returns>Returns a task which pin or unpin a message.</returns>
    private async Task OnPinOrUnpinAsync(PinMessageEventArgs e)
    {
        if (_messageService is not null &&
            ChatState.Room is not null)
        {
            await ChatMessageService.PinOrUnpinAsync(new(ChatState.Room.Id, e.Message, e.Pin));
            await RefreshDataAsync();
            await _messageService.SendAsync(ChatMessageListViewConstants.PinOrUnpinAsync, ChatState.Room.Id, ChatState.Room.GetUsersBut(Owner));
        }
    }

    /// <summary>
    /// Occurs when a message was reacted by a user.
    /// </summary>
    /// <param name="e">Event args associated to the method.</param>
    /// <returns>Returns a task which reacts to a message when completed.</returns>
    private async Task OnReactAsync(ChatMessageReactEventArgs e)
    {
        if (ChatState.Room is not null &&
            Owner is not null &&
            !string.IsNullOrEmpty(e.Reaction) &&
            _messageService is not null)
        {
            await ChatMessageService.AddReactionAsync(new(ChatState.Room.Id, Owner, e.Message, e.Reaction));
            await RefreshDataAsync();
            await _messageService.SendAsync(ChatMessageListViewConstants.SendReactOnMessageAsync, ChatState.Room.Id, ChatState.Room.GetUsersBut(Owner));
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

        StringBuilder s = new();
        s.Append(_chatDraft.Text);
        s.Append(emoji.Unicode);

        _chatDraft.Text = s.ToString();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ChatState.RoomChanged += OnRoomChanged;
        _chatDraft = ChatState.GetDraft();

        _messageService = MessageServiceFactory.Create(HubName);
        _messageService.ListenOn<long, IEnumerable<long>>(ChatMessageListViewConstants.ReceiveMessages, OnReceivedMessagesAsync)
                       .ListenOn<long, long>(ChatMessageListViewConstants.ReceiveMessage, OnReceivedMessageAsync)
                       .ListenOn<long>(ChatMessageListViewConstants.MessageDeleted, OnDeletedMessageAsync)
                       .ListenOn<long>(ChatMessageListViewConstants.ReactOnMessage, OnReactedMessageAsync)
                       .ListenOn<long>(ChatMessageListViewConstants.PinOrUnpin, OnPinOrUnpinAsync)
                       .ListenOn<long, long>(ChatMessageListViewConstants.MessageRead, OnMessageReadAsync);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (_messageService is not null)
            {
                await _messageService.StartAsync();
            }

            _dotNetReference ??= DotNetObjectReference.Create(this);
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _module.InvokeVoidAsync("initialize", Id, _dotNetReference, ChunkSize);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        ChatState.RoomChanged -= OnRoomChanged;

        if (_messageService is not null)
        {
            _messageService.ListenOff(ChatMessageListViewConstants.ReceiveMessages)
                           .ListenOff(ChatMessageListViewConstants.ReceiveMessage)
                           .ListenOff(ChatMessageListViewConstants.MessageDeleted)
                           .ListenOff(ChatMessageListViewConstants.ReactOnMessage)
                           .ListenOff(ChatMessageListViewConstants.PinOrUnpin);

            await _messageService.DisposeAsync();
            _messageService = null;
        }

        GC.SuppressFinalize(this);
    }

    private void OnAudioReady(byte[] data)
    {
        var fileName = Path.GetRandomFileName();
        fileName = Path.ChangeExtension(fileName, "webm");
        _chatDraft?.SelectedChatFiles.Add(new(fileName, "audio/webm", data, true));
    }
}
