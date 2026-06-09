namespace FluentUI.Blazor.Community.Components.Chat.Messages;

internal sealed record ChatMessageBuildResult(IReadOnlyList<ChatMessageBuildItem> Items)
{
    public static ChatMessageCreationItem ToCreationItem(ChatMessageBuildItem item)
    {
        return new ChatMessageCreationItem(item.Message, item.Files);
    }
}
