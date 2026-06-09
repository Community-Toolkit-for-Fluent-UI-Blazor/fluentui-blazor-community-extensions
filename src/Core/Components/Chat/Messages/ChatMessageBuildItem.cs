using FluentUI.Blazor.Community.Components.Chat.Files;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

internal sealed record ChatMessageBuildItem(ChatMessage Message, IReadOnlyList<IBinaryChatFile> Files)
{
}
