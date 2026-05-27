using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Engine;

internal static class ChatUnreadCalculator
{
    public static int Compute(
        ChatRoom room,
        long currentUserId,
        ChatMessageState messageState,
        ChatMessageDynamicState dynamicState)
    {
        var messages = messageState.GetMessages(room.Id);

        if (messages is null ||
            !messages.Any())
        {
            return 0;
        }

        var count = 0;

        foreach (var msg in messages)
        {
            var state = dynamicState.GetReadState(room, msg.Id);

            if (msg.Sender?.Id != currentUserId &&
                state == ChatMessageReadState.Unread)
            {
                count++;
            }
        }

        return count;
    }
}
