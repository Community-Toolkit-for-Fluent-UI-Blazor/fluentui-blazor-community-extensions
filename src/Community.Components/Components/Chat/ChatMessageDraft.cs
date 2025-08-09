namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a draf for a chat message.
/// </summary>
public sealed class ChatMessageDraft
{
    /// <summary>
    /// Represents the message to edit.
    /// </summary>
    private IChatMessage? _editMessage;

    /// <summary>
    /// Represents the replied message.
    /// </summary>
    private IChatMessage? _replyMessage;

    /// <summary>
    /// Represents all cultures to convert a message.
    /// </summary>
    private Dictionary<string, IEnumerable<string>> _textCultures = [];

    /// <summary>
    /// Gets or sets the text of the message.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the files to send with the message.
    /// </summary>
    public List<ChatFileEventArgs> SelectedChatFiles { get; set; } = [];

    /// <summary>
    /// Gets the replied message.
    /// </summary>
    public IChatMessage? Reply => _replyMessage;

    /// <summary>
    /// Adds a culture of the translated texts.
    /// </summary>
    /// <param name="culture">Culture of the message.</param>
    /// <param name="texts">Translated texts into the culture.</param>
    internal void AddCultureText(string culture, IEnumerable<string> texts)
    {
        _textCultures.TryAdd(culture, texts);
    }

    /// <summary>
    /// Gets all texts ordered by cultures.
    /// </summary>
    /// <returns>Returns all texts in their own culture.</returns>
    public IReadOnlyDictionary<string, IEnumerable<string>> GetTranslatedTexts()
    {
        return _textCultures;
    }

    /// <summary>
    /// Clear the draft.
    /// </summary>
    internal void Clear()
    {
        _textCultures.Clear();
        Text = string.Empty;
        SelectedChatFiles.Clear();
        ClearEditMessage();
        ClearReplyMessage();
    }

    /// <summary>
    /// Remove the replied message.
    /// </summary>
    internal void ClearReplyMessage()
    {
        _replyMessage = null;
    }

    /// <summary>
    /// Clear the edited message.
    /// </summary>
    internal void ClearEditMessage()
    {
        _editMessage = null;
        Text = string.Empty;
    }

    /// <summary>
    /// Gets the edited message.
    /// </summary>
    /// <returns>Returns the edited message.</returns>
    internal IChatMessage? GetEditMessage()
    {
        return _editMessage;
    }

    /// <summary>
    /// Gets the replied message from the culture of the owner.
    /// </summary>
    /// <param name="owner">Owner of the message to reply.</param>
    /// <returns>Returns the reply message of the owner.</returns>
    internal string? GetReplyText(ChatUser? owner)
    {
        var section = _replyMessage?.Sections.Find(x => x.CultureId == owner?.CultureId);

        if (section is null)
        {
            section = _replyMessage?.Sections[0];
        }

        return section?.Content;
    }

    /// <summary>
    /// Sets the message to edit from the culture of the owner.
    /// </summary>
    /// <param name="owner">Owner of the message.</param>
    /// <param name="message">Message to edit.</param>
    internal void SetEditMessage(ChatUser owner, IChatMessage message)
    {
        _editMessage = message;
        Text = message.Sections.FirstOrDefault(s => s.CultureId == owner.CultureId)?.Content;
    }

    /// <summary>
    /// Sets the reply message.
    /// </summary>
    /// <param name="message">Message to reply.</param>
    internal void SetReplyMessage(IChatMessage message)
    {
        _replyMessage = message;
    }
}
