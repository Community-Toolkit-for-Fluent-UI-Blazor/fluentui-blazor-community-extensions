namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content of the group selector.
/// </summary>
/// <param name="Labels">Labels to use on the dialog.</param>
/// <param name="OnSearchFunction">Search function to use.</param>
public record ChatUserGroupSelectorContent(
    ChatRoomLabels Labels, 
    Func<string, Task<IEnumerable<ChatUser>>> OnSearchFunction)
{
}
