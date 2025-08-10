using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the card of a chat message.
/// </summary>
public partial class ChatMessageCard
    : FluentComponentBase, IDisposable
{
    #region Fields

    /// <summary>
    /// Represents the fragment to render the avatar.
    /// </summary>
    private readonly RenderFragment _renderAvatar;

    /// <summary>
    /// Represents the fragment to render the card.
    /// </summary>
    private readonly RenderFragment _renderCard;

    /// <summary>
    /// Represents the fragment to render the action buttons.
    /// </summary>
    private readonly RenderFragment _renderActionButtons;

    /// <summary>
    /// Represents the fragment to render the message.
    /// </summary>
    private readonly RenderFragment _renderMessage;

    /// <summary>
    /// Represents the fragment to render the emoji button.
    /// </summary>
    private readonly RenderFragment _renderEmojiButton;

    /// <summary>
    /// Represents the fragment to render the read state.
    /// </summary>
    private readonly RenderFragment _renderReadState;

    /// <summary>
    /// Represents the fragment to render the react on the message.
    /// </summary>
    private readonly RenderFragment<IEnumerable<IChatMessageReaction>> _renderReactions;

    /// <summary>
    /// Represents the fragment to render a footer.
    /// </summary>
    private readonly RenderFragment _renderFooter;

    /// <summary>
    /// Represents the fragment to render the text.
    /// </summary>
    private readonly RenderFragment _renderText;

    /// <summary>
    /// Represents the fragment to render the document.
    /// </summary>
    private readonly RenderFragment<ImageGroupLayout> _renderDocument;

    /// <summary>
    /// Represents the fragment to render a gift.
    /// </summary>
    private readonly RenderFragment _renderGift;

    /// <summary>
    /// Represents the fragment to render a deleted message.
    /// </summary>
    private readonly RenderFragment _renderDeletedMessage;

    /// <summary>
    /// Represents a value indicating if the popover is visible or not.
    /// </summary>
    private bool _showActionsPopover;

    /// <summary>
    /// Represents a value indicating if a click on the card is prevented if the click is on a button inside the card.
    /// </summary>
    private bool _preventTapped;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the message to render.
    /// </summary>
    [Parameter]
    public IChatMessage? Message { get; set; }

    /// <summary>
    /// Gets or sets the provider of emojis.
    /// </summary>
    [Parameter]
    public GEmojiProviderDelegate? GEmojiItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DeviceInfoState"/>.
    /// </summary>
    [Inject]
    private DeviceInfoState DeviceInfoState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the owner of the message.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the card is tapped.
    /// </summary>
    [Parameter]
    public EventCallback<IChatMessage> Tapped { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is deleted.
    /// </summary>
    [Parameter]
    public EventCallback<IChatMessage> Delete { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when a react occurs on the message.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessageReactEventArgs> React { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is edited.
    /// </summary>
    [Parameter]
    public EventCallback<IChatMessage> Edit { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is copied.
    /// </summary>
    [Parameter]
    public EventCallback<IChatMessage> Copy { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is replied.
    /// </summary>
    [Parameter]
    public EventCallback<IChatMessage> Reply { get; set; }

    /// <summary>
    /// Gets or sets the labels to use for the component.
    /// </summary>
    [Parameter]
    public ChatMessageListLabels ChatMessageListLabels { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback to raise when the message is pinned or unpined.
    /// </summary>
    [Parameter]
    public EventCallback<PinMessageEventArgs> PinOrUnpin { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the deleted message is shown or not.
    /// </summary>
    [Parameter]
    public bool ShowDeleted { get; set; }

    /// <summary>
    /// Gets or sets the template of the message.
    /// </summary>
    [Parameter]
    public RenderFragment<IChatMessage>? MessageTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template of a deleted message.
    /// </summary>
    /// <remarks>The template is used when a message is deleted and when <see cref="ShowDeleted"/> is set to true.</remarks>
    [Parameter]
    public RenderFragment? DeletedMessageTemplate { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Occurs when the message is pinned or unpinned.
    /// </summary>
    /// <param name="pin">Value indicating if the message is pinned or not.</param>
    /// <returns>Returns a task which pin or unpin the message when completed.</returns>
    private async Task OnPinOrUnpinAsync(bool pin)
    {
        _showActionsPopover = false;

        if (PinOrUnpin.HasDelegate)
        {
            await PinOrUnpin.InvokeAsync(new(Message!, pin));
        }
    }

    /// <summary>
    /// Shows the <see cref="GEmojiExplorer"/> in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which show the explorer and react on the message if not cancelled.</returns>
    private async Task OnShowEmojiExplorerAsync()
    {
        _preventTapped = true;

        var dialog = await DialogService.ShowDialogAsync<GEmojiDialog>(
            new GEmojiContent(GEmojiItemsProvider),
            new DialogParameters()
            {
                ShowDismiss = false,
                Height = "70%",
                PrimaryAction = string.Empty,
                SecondaryAction = string.Empty
            });

        var result = await dialog.Result;

        if (!result.Cancelled &&
            result.Data is string s &&
            Message is not null &&
            React.HasDelegate)
        {
            await React.InvokeAsync(new(Message, s));
        }
    }

    /// <summary>
    /// Occurs when the card is clicked.
    /// </summary>
    /// <returns>Returns a task which raise the <see cref="Tapped"/> callback if not prevented.</returns>
    private async Task OnTappedAsync()
    {
        if (_preventTapped)
        {
            _preventTapped = false;
            return;
        }

        if (Tapped.HasDelegate)
        {
            await Tapped.InvokeAsync(Message);
        }
    }

    /// <summary>
    /// Occurs when the reply button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes <see cref="Reply"/> when completed.</returns>
    private async Task OnReplyAsync()
    {
        _showActionsPopover = false;

        if (Reply.HasDelegate)
        {
            await Reply.InvokeAsync(Message);
        }
    }

    /// <summary>
    /// Occurs when the copy button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes <see cref="Copy"/> when completed.</returns>
    private async Task OnCopyAsync()
    {
        _showActionsPopover = false;

        if (Copy.HasDelegate)
        {
            await Copy.InvokeAsync(Message);
        }
    }

    /// <summary>
    /// Occurs when the edit button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes <see cref="Edit"/> when completed.</returns>
    private async Task OnEditAsync()
    {
        _showActionsPopover = false;

        if (Edit.HasDelegate)
        {
            await Edit.InvokeAsync(Message);
        }
    }

    /// <summary>
    /// Gets the source image of the <paramref name="chatFile"/>.
    /// </summary>
    /// <param name="chatFile">Chat file to get the source image.</param>
    /// <returns>Returns the source image of the <paramref name="chatFile"/>.</returns>
    private static string? GetSourceImage(IChatFile chatFile)
    {
        if (chatFile.ContentType.StartsWith("image"))
        {
            if (chatFile is IUrlChatFile urlChatFile)
            {
                return urlChatFile.Url;
            }
            else if (chatFile is BinaryChatFile binaryChatFile)
            {
                return Base64ContentHelper.GetBase64Content(binaryChatFile.Data, binaryChatFile.ContentType);
            }
        }

        return FileIcons.ToImageSource(FileIcons.FromExtension(Path.GetExtension(chatFile.Name)));
    }

    /// <summary>
    /// Occurs when the message is deleted.
    /// </summary>
    /// <returns>Returns a task which deletes the message when completed.</returns>
    private async Task OnDeleteAsync()
    {
        _showActionsPopover = false;

        var dialog = await DialogService.ShowConfirmationAsync(
            ChatMessageListLabels.DeleteMessage,
            ChatMessageListLabels.DialogYes,
            ChatMessageListLabels.DialogNo,
            ChatMessageListLabels.DeleteTitle
        );

        var result = await dialog.Result;

        if (!result.Cancelled && Delete.HasDelegate)
        {
            await Delete.InvokeAsync(Message);
        }
    }

    /// <summary>
    /// Gets the reply text.
    /// </summary>
    /// <returns>Returns the reply text.</returns>
    private string? GetReplyText()
    {
        var section = Message?.ReplyMessage?.Sections.FirstOrDefault(x => x.CultureId == Owner?.CultureId);

        section ??= Message?.ReplyMessage?.Sections[0];

        return section?.Content;
    }

    /// <summary>
    /// Gets the number of documents visible on the message.
    /// </summary>
    /// <returns></returns>
    private int GetDocumentVisibleCount()
    {
        if (DeviceInfoState is null ||
            DeviceInfoState.DeviceInfo is null)
        {
            return 4;
        }

        return DeviceInfoState.DeviceInfo.Mobile switch
        {
            Mobile.UnknownMobileDevice or Mobile.NotMobileDevice => 5,
            _ => 4,
        };
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _showActionsPopover = false;

        GC.SuppressFinalize(this);
    }

    #endregion Methods
}
