using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Engine;

/// <summary>
/// Represents a utility class responsible for calculating the read state of a chat message based on user states and room users.
/// </summary>
internal static class ChatMessageReadStateCalculator
{
    /// <summary>
    /// Calculates the read state of a chat message for a given owner based on the states of other users in the chat room.
    /// </summary>
    /// <param name="ownerId">The ID of the owner of the chat message.</param>
    /// <param name="userStates">A list of user states indicating whether each user has read the message.</param>
    /// <param name="roomUsers">A list of users in the chat room.</param>
    /// <returns>Returns the read state of the chat message.</returns>
    public static ChatMessageReadState Compute(
        long ownerId,
        IReadOnlyList<ChatMessageUserState> userStates,
        IReadOnlyList<ChatUser> roomUsers)
    {
        var totalOthers = roomUsers.Count - 1;

        if (totalOthers <= 0)
        {
            return ChatMessageReadState.FullyRead;
        }

        var stateMap = new Dictionary<long, bool>(userStates.Count);

        for (var i = 0; i < userStates.Count; i++)
        {
            var s = userStates[i];
            stateMap[s.UserId] = s.IsRead;
        }

        var readCount = 0;

        for (var i = 0; i < roomUsers.Count; i++)
        {
            var user = roomUsers[i];

            if (user.Id == ownerId)
            {
                continue;
            }

            if (stateMap.TryGetValue(user.Id, out var isRead) && isRead)
            {
                readCount++;

                if (readCount == totalOthers)
                {
                    return ChatMessageReadState.FullyRead;
                }
            }
        }

        if (readCount == 0)
        {
            return ChatMessageReadState.Unread;
        }

        if (readCount == totalOthers)
        {
            return ChatMessageReadState.FullyRead;
        }

        return ChatMessageReadState.PartiallyRead;
    }
}
