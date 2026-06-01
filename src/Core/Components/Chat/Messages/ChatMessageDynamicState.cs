using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

internal sealed class ChatMessageDynamicState
{
    private readonly Dictionary<long, Dictionary<long, ChatMessageReadState>> _readStates = [];
    private readonly Dictionary<long, Dictionary<long, IReadOnlyList<ChatMessageReaction>>> _reactions = [];
    private readonly Dictionary<long, Dictionary<long, List<IChatFile>>> _files = [];
    private readonly Dictionary<long, bool> _pinStates = [];

    public event EventHandler? ReadStateUpdated;
    public event EventHandler? ReactionsUpdated;
    public event EventHandler? FilesUpdated;

    public ChatMessageReadState GetReadState(ChatRoom room, long messageId)
    {
        if (_readStates.TryGetValue(room.Id, out var dict) && dict.TryGetValue(messageId, out var state))
        {
            return state;
        }

        return ChatMessageReadState.Unread;
    }

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

        dict[messageId] = reactions;
        ReactionsUpdated?.Invoke(this, System.EventArgs.Empty);
    }

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

    public void SetPinState(long id, bool pin)
    {
        _pinStates[id] = pin;
    }

    public bool GetPinState(long id)
    {
        if (_pinStates.TryGetValue(id, out var pin))
        {
            return pin;
        }

        return false;
    }
}
