namespace FluentUI.Blazor.Community.Components;

public class SleekDialAnimationSettings
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(400);

    public TimeSpan Delay { get; set; }

    public SleekDialAnimation Animation { get; set; } = SleekDialAnimation.None;
}
