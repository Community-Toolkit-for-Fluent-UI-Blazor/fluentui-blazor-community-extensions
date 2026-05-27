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

    #endregion Properties

    #region Methods

    /// <summary>
    /// Occurs when the message is pinned or unpinned.
    /// </summary>
    /// <param name="pin">Value indicating if the message is pinned or not.</param>
    /// <returns>Returns a task which pin or unpin the message when completed.</returns>
    private async Task OnPinOrUnpinAsync(bool pin)
    {
        if (PinOrUnpin.HasDelegate)
        {
            await PinOrUnpin.InvokeAsync(new(Message!, pin));
        }
    }

    /// <summary>
    /// Shows the <see cref="EmojiPickerDialog"/> in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which show the explorer and react on the message if not cancelled.</returns>
    private async Task OnShowEmojiExplorerAsync()
    {
        var panelResult = await DialogService.ShowDrawerAsync<EmojiPickerDialog>(a =>
        {
            a.Header.Title = Localizer[LanguageResource.CX_Chat_EmokiPicker_Dialog_Title];
            a.Size = DialogSize.Small;
            a.Parameters.Add(nameof(EmojiPickerDialog.FontProvider), EmojiDialogSettings.FontProvider);
            a.Parameters.Add(nameof(EmojiPickerDialog.EmojisPerRow), EmojiDialogSettings.EmojisPerRow);
        });

        if (panelResult.Cancelled)
        {
            return;
        }

        if (panelResult.Value is not FluentCxEmoji emoji ||
            Message is null ||
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

        if (!dialog.Cancelled && Delete.HasDelegate)
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
        var section = Message?.ReplyToMessage?.Sections.FirstOrDefault(x => x.CultureId == Owner?.CultureId);

        section ??= Message?.ReplyToMessage?.Sections[0];

        return section?.Content;
    }

    private IReadOnlyList<ChatMessageReaction> GetReactions()
    {
        if (Message is null ||
            ChatState.Room is null)
        {
            return [];
        }

        return DynamicState.GetReactions(ChatState.Room, Message.Id);
    }

    private ChatMessageReadState GetReadState()
    {
        if (Message is null ||
            ChatState.Room is null)
        {
            return ChatMessageReadState.Unread;
        }

        return DynamicState.GetReadState(ChatState.Room, Message.Id);
    }

    /*  /// <summary>
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
      }*/

    #endregion Methods
}
