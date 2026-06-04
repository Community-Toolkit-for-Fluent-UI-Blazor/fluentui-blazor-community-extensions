using FluentUI.Blazor.Community.Components.Chat.Engine;
using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

internal sealed class ChatMessageDynamicState
{
    private readonly Dictionary<long, Dictionary<long, ChatMessageReadState>> _readStates = [];
    private readonly Dictionary<long, Dictionary<long, List<ChatMessageReaction>>> _reactions = [];
    private readonly Dictionary<long, Dictionary<long, List<IChatFile>>> _files = [];
    private readonly Dictionary<long, bool> _pinStates = [];

    public event EventHandler? ReadStateUpdated;
    public event EventHandler? ReactionsUpdated;
    public event EventHandler? FilesUpdated;

    /// <summary>
    /// Gets the read state of a message in a chat room. If the read state is not found, it returns Unread by default.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <returns>The read state of the message.</returns>
    public ChatMessageReadState GetReadState(
        ChatRoom room,
        long messageId)
    {
        if (_readStates.TryGetValue(room.Id, out var dict) &&
            dict.TryGetValue(messageId, out var state))
        {
            return state;
        }

        return ChatMessageReadState.Unread;
    }

    /// <summary>
    /// Gets the reactions of a message in a chat room.
    /// If the reactions are not found, it returns an empty list by default.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <returns>The reactions of the message.</returns>
    public IReadOnlyList<ChatMessageReaction> GetReactions(
        ChatRoom room,
        long messageId)
    {
        if (_reactions.TryGetValue(room.Id, out var dict) &&
            dict.TryGetValue(messageId, out var reactions))
        {
            return reactions;
        }

        return [];
    }

    /// <summary>
    /// Gets the files of a message in a chat room.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <returns>The files of the message.</returns>
    public IReadOnlyList<IChatFile> GetFiles(
        ChatRoom room,
        long messageId)
    {
        if (_files.TryGetValue(room.Id, out var dict) &&
            dict.TryGetValue(messageId, out var files))
        {
            return files;
        }

        return [];
    }

    /// <summary>
    /// Sets the read state of a message in a chat room. If the read state is not found, it adds a new entry.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="state">The read state to set.</param>
    public void SetReadState(
        ChatRoom room,
        long messageId,
        ChatMessageReadState state)
    {
        if (!_readStates.TryGetValue(room.Id, out var dict))
        {
            dict = [];
            _readStates[room.Id] = dict;
        }

        dict[messageId] = state;
        ReadStateUpdated?.Invoke(this, System.EventArgs.Empty);
    }

    /// <summary>
    /// Sets the reactions of a message in a chat room. If the reactions are not found, it adds a new entry.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="reactions">The reactions to set.</param>
    public void SetReactions(
        ChatRoom room,
        long messageId,
        IReadOnlyList<ChatMessageReaction> reactions)
    {
        if (!_reactions.TryGetValue(room.Id, out var dict))
        {
            dict = [];
            _reactions[room.Id] = dict;
        }

        dict[messageId] = [.. reactions];
        ReactionsUpdated?.Invoke(this, System.EventArgs.Empty);
    }
    
    /// <summary>
    /// Sets the files of a message in a chat room. If the files are not found, it adds a new entry.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="files">The files to set.</param>
    public void SetFiles(
        ChatRoom room,
        long messageId,
        IReadOnlyList<IChatFile> files)
    {
        if (!_files.TryGetValue(room.Id, out var dict))
        {
            dict = [];
            _files[room.Id] = dict;
        }

        dict[messageId] = [.. files];
        FilesUpdated?.Invoke(this, System.EventArgs.Empty);
    }

    /// <summary>
    /// Appends a file to the existing files of a message in a chat room. If the files are not found, it adds a new entry.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="file">The file to append.</param>
    public void AppendFile(ChatRoom room, long messageId, IChatFile file)
    {
        if (!_files.TryGetValue(room.Id, out var dict))
        {
            dict = [];
            _files[room.Id] = dict;
        }

        if (!dict.TryGetValue(messageId, out _))
        {
            dict.Add(messageId, []);
            dict[messageId].Add(file);
        }
        else
        {
            dict[messageId].Add(file);
        }
    }
    
    /// <summary>
    /// Clears the dynamic state of a message in a chat room.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    public void Clear(ChatRoom room, long messageId)
    {
        if (_readStates.TryGetValue(room.Id, out var readStatesDict))
        {
            readStatesDict.Remove(messageId);
            ReadStateUpdated?.Invoke(this, System.EventArgs.Empty);
        }

        if (_reactions.TryGetValue(room.Id, out var reactionsDict))
        {
            reactionsDict.Remove(messageId);
            ReactionsUpdated?.Invoke(this, System.EventArgs.Empty);
        }

        if (_files.TryGetValue(room.Id, out var filesDict))
        {
            filesDict.Remove(messageId);
            FilesUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }

    /// <summary>
    /// Sets the pin state of a message. If the pin state is not found, it adds a new entry.
    /// </summary>
    /// <param name="id">The ID of the message.</param>
    /// <param name="pin">The pin state to set.</param>
    public void SetPinState(long id, bool pin)
    {
        _pinStates[id] = pin;
    }

    /// <summary>
    /// Gets the pin state of a message. If the pin state is not found, it returns false by default.
    /// </summary>
    /// <param name="id">The ID of the message.</param>
    /// <returns>The pin state of the message.</returns>
    public bool GetPinState(long id)
    {
        if (_pinStates.TryGetValue(id, out var pin))
        {
            return pin;
        }

        return false;
    }

    /// <summary>
    /// Appends a reaction to the existing reactions of a message in a chat room.
    /// If the reactions are not found, it adds a new entry.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="item">The reaction to append.</param>
    public void AppendReactions(
        ChatRoom room,
        long messageId,
        ChatMessageReaction item)
    {
        if (!_reactions.TryGetValue(room.Id, out var dict))
        {
            dict = [];
            _reactions[room.Id] = dict;
        }

        if (!dict.TryGetValue(messageId, out _))
        {
            dict.Add(messageId, []);
            dict[messageId].Add(item);
        }
        else
        {
            dict[messageId].Add(item);
        }
    }

    /// <summary>
    /// Sets the read states of messages in a chat room based on the provided user states and users.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="ownerId">The ID of the owner.</param>
    /// <param name="readStates">The read states of the messages.</param>
    /// <param name="users">The users in the chat room.</param>
    internal void SetReadStates(
        ChatRoom room,
        long ownerId,
        IReadOnlyList<ChatMessageUserState> readStates,
        IReadOnlyList<ChatUser> users)
    {
        var dict = new Dictionary<long, List<ChatMessageUserState>>();

        foreach (var item in readStates)
        {
            if (!dict.TryGetValue(item.MessageId, out var listForMessage))
            {
                listForMessage = [];
                dict[item.MessageId] = listForMessage;
            }

            listForMessage.Add(item);
        }

        foreach (var item in dict)
        {
            var state = ChatMessageReadStateCalculator.Compute(ownerId, item.Value, users);
            SetReadState(room, item.Key, state);
        }
    }

    /// <summary>
    /// Sets the reactions of messages in a chat room based on the provided reactions.
    /// </summary>
    /// <param name="room">The chat room.</param>
    /// <param name="files">The files to set for the messages.</param>
    internal void SetFiles(ChatRoom room, IReadOnlyList<IChatFile> files)
    {
        var dict = new Dictionary<long, List<IChatFile>>();

        foreach (var item in files)
        {
            if (!dict.TryGetValue(item.MessageId, out var listForMessage))
            {
                listForMessage = [];
                dict[item.MessageId] = listForMessage;
            }

            listForMessage.Add(item);
        }

        foreach (var item in dict)
        {
            SetFiles(room, item.Key, item.Value);
        }
    }
}
