using System.Globalization;
using FluentUI.Blazor.Community.Components.Chat;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Components.Chat;
using FluentUI.Blazor.Community.Components.Emojis;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the card of a chat message.
/// </summary>
public partial class ChatMessageCard
    : FluentComponentBase
{
    #region Fields

    /// <summary>
    /// Value indicating if the emoji popover is visible or not.
    /// </summary>
    private bool _isEmojiPopoverVisible;

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
    private readonly RenderFragment _renderReactions;

    /// <summary>
    /// Represents the fragment to render a footer.
    /// </summary>
    private readonly RenderFragment _renderFooter;

    /// <summary>
    /// Represents the fragment to render a header.
    /// </summary>
    private readonly RenderFragment _renderHeader;

    /// <summary>
    /// Represents the fragment to render the text.
    /// </summary>
    private readonly RenderFragment _renderText;

    /// <summary>
    /// Represents the fragment to render the document.
    /// </summary>
    private readonly RenderFragment _renderDocument;

    /// <summary>
    /// Represents the fragment to render a gift.
    /// </summary>
    private readonly RenderFragment _renderGift;

    /// <summary>
    /// Represents the fragment to render a deleted message.
    /// </summary>
    private readonly RenderFragment _renderDeletedMessage;

    /// <summary>
    /// Represents the culture info of the owner of the message, used for formatting the date of the message.
    /// </summary>
    private CultureInfo _ownerCultureInfo = CultureInfo.CurrentCulture;

    /// <summary>
    /// Value indicating if the owner of the message has changed, used to update the culture info of the owner.
    /// </summary>
    private bool _hasOwnerChanged;

    private readonly EventCallback _emptyCallback = EventCallback.Empty;

    private readonly EventCallback _tappedCallback;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the dynamic state of the chat message.
    /// </summary>
    [Inject]
    private ChatMessageDynamicState DynamicState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the state of the chat.
    /// </summary>
    [Inject]
    private ChatState ChatState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the message to render.
    /// </summary>
    [Parameter]
    public ChatMessage? Message { get; set; }

    /// <summary>
    /// Gets or sets the owner of the message.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the card is tapped.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessage> Tapped { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is deleted.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessage> Delete { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when a react occurs on the message.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessageReactEventArgs> React { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is edited.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessage> Edit { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is copied.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessage> Copy { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the message is replied.
    /// </summary>
    [Parameter]
    public EventCallback<ChatMessage> Reply { get; set; }

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
    public RenderFragment<ChatMessage>? MessageTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template of a deleted message.
    /// </summary>
    /// <remarks>The template is used when a message is deleted and when <see cref="ShowDeleted"/> is set to true.</remarks>
    [Parameter]
    public RenderFragment? DeletedMessageTemplate { get; set; }

    /// <summary>
    /// Gets or sets the settings of the emoji dialog.
    /// </summary>
    [Parameter]
    public EmojiSettings EmojiDialogSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating if the react feature is enabled.
    /// </summary>
    [Parameter]
    public bool ReactEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the provider used to get the font family for rendering emojis.
    /// </summary>
    [Parameter]
    public IEmojiFontProvider EmojiFontProvider { get; set; } = new MicrosoftEmojiProvider();

    /// <summary>
    /// Gets the CSS font-family string to use for rendering emojis, combining the specified emoji font family and fallback font family.
    /// </summary>
    private string FontFamily => $"{EmojiDialogSettings.FontProvider.FontFamily}, {EmojiDialogSettings.FontProvider.FallbackFontFamily}";

    /// <summary>
    /// Gets the internal CSS style string for the component.
    /// </summary>
    private string? InternalMessageReactionStyle => new StyleBuilder()
        .AddStyle("font-family", FontFamily)
        .AddStyle("height", "22px")
        .AddStyle("align-items", "center")
        .AddStyle("display", "flex")
        .AddStyle("font-size", "18px")
        .Build();

    /// <summary>
    /// Gets the formatted date message to show at the bottom of the message.
    /// </summary>
    private string? FormattedDateMessage => Message?.CreatedDate.DateTime.ToString("F", _ownerCultureInfo);

    #endregion Properties

    #region Methods

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Message is not null)
        {
            DynamicState.SetPinState(Message.Id, Message.IsPinned);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasOwnerChanged = parameters.TryGetValue<ChatUser>(nameof(Owner), out var newOwner) && !Equals(newOwner, Owner);
        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasOwnerChanged && !string.IsNullOrEmpty(Owner?.CultureName))
        {
            _ownerCultureInfo = CultureInfo.GetCultureInfo(Owner.CultureName);
        }
    }

    /// <summary>
    /// Occurs when the message is pinned or unpinned.
    /// </summary>
    /// <param name="pin">Value indicating if the message is pinned or not.</param>
    /// <returns>Returns a task which pin or unpin the message when completed.</returns>
    private async Task OnPinOrUnpinAsync(bool pin)
    {
        if (Message is not null && PinOrUnpin.HasDelegate)
        {
            await PinOrUnpin.InvokeAsync(new(Message!, pin));
            DynamicState.SetPinState(Message.Id, pin);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Occurs when a react occurs on the message.
    /// </summary>
    /// <param name="emoji">The emoji that was added as a reaction.</param>
    /// <returns>Returns a task which adds the emoji reaction when completed.</returns>
    private async Task OnAddEmojiAsync(FluentCxEmoji emoji)
    {
        if (Message is null ||
            !React.HasDelegate)
        {
            return;
        }

        await React.InvokeAsync(new(Message!, emoji.Unicode));
    }

    /// <summary>
    /// Occurs when the card is clicked.
    /// </summary>
    /// <returns>Returns a task which raise the <see cref="Tapped"/> callback if not prevented.</returns>
    private EventCallback GetTappedCallback()
    {
        if (Message?.Type == ChatMessageType.Text)
        {
            return _emptyCallback;
        }

        return _tappedCallback;
    }

    private async Task OnTappedAsync()
    {
        if (Message is not null &&
            Tapped.HasDelegate)
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
        if (Edit.HasDelegate)
        {
            await Edit.InvokeAsync(Message);
        }
    }

    /// <summary>
    /// Occurs when the message is deleted.
    /// </summary>
    /// <returns>Returns a task which deletes the message when completed.</returns>
    private async Task OnDeleteAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            Localizer[LanguageResource.CX_Chat_Message_DeleteMessage],
            Localizer[LanguageResource.CX_Chat_Message_DeleteTitle],
            Localizer[LanguageResource.CX_Chat_Message_DialogYes],
            Localizer[LanguageResource.CX_Chat_Message_DialogNo]
        );

        if (!dialog.Cancelled &&
            Delete.HasDelegate)
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
        var msg = Message?.ReplyToMessage;

        if (msg is null)
        {
            return null;
        }

        if (msg.Type == ChatMessageType.Text)
        {
            var section = Message?.ReplyToMessage?.Sections.FirstOrDefault(x => x.CultureId == Owner?.CultureId);

            section ??= Message?.ReplyToMessage?.Sections.Count > 0 ? Message?.ReplyToMessage?.Sections[0] : null;

            return section?.Content;
        }
        else if (msg.Type == ChatMessageType.Files)
        {
            return Localizer[LanguageResource.CX_Chat_Message_ReplyFromDocumentOnly];
        }
        else if (msg.Type == ChatMessageType.Gift)
        {
            return Localizer[LanguageResource.CX_Chat_Message_ReplyFromGiftOnly];
        }
        else
        {
            return Localizer[LanguageResource.CX_Chat_Message_ReplyFromMultipleSources];
        }
    }

    /// <summary>
    /// Retrieves the reactions of the message.
    /// </summary>
    /// <returns>Returns the reactions of the message.</returns>
    private IReadOnlyList<ChatMessageReaction> GetReactions()
    {
        if (Message is null ||
            ChatState.Room is null)
        {
            return [];
        }

        return DynamicState.GetReactions(ChatState.Room, Message.Id);
    }

    /// <summary>
    /// Retrieves the read state of the message.
    /// </summary>
    /// <returns>Returns the read state of the message.</returns>
    private ChatMessageReadState GetReadState()
    {
        if (Message is null ||
            ChatState.Room is null)
        {
            return ChatMessageReadState.Unread;
        }

        return DynamicState.GetReadState(ChatState.Room, Message.Id);
    }

    #endregion Methods
}
