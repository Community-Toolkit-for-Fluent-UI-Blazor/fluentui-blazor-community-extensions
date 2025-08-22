namespace FluentUI.Blazor.Community.Components;

[Flags]
public enum ChatViews
{
    None = 0,
    Messages = 1,
    PinnedMessages = 2,
    Images = 4,
    Video = 8,
    Audio = 16,
    Other = 32
}
