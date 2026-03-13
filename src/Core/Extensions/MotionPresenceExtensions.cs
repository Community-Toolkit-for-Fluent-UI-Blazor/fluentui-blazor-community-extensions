namespace FluentUI.Blazor.Community.Components.Extensions;

/// <summary>
/// Provides extension methods for applying enter and exit animations to motion presence components.
/// </summary>
/// <remarks>These extension methods simplify the process of triggering enter and exit animations on components
/// that support motion presence. They check for the presence of required animation variants and transitions before
/// applying the animation, ensuring safe and predictable behavior.</remarks>
public static class MotionPresenceExtensions
{
    /// <summary>
    /// Applies the enter transition asynchronously to the specified motion item using the provided motion presence
    /// configuration.
    /// </summary>
    /// <param name="presence">The motion presence configuration that defines the enter and transition behaviors to apply.</param>
    /// <param name="item">The motion item to which the enter transition will be applied.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the enter transition has been
    /// applied, or immediately if no enter or transition configuration is present.</returns>
    public static Task ApplyEnterAsync(this MotionPresence presence, MotionItem item)
    {
        if (item.Actions is null || presence.Enter is null || presence.Transition is null)
        {
            return Task.CompletedTask;
        }

        return item.Actions.ToVariantAsync(presence.Enter, presence.Transition);
    }

    /// <summary>
    /// Applies the exit animation to the specified motion item asynchronously using the provided motion presence
    /// settings.
    /// </summary>
    /// <param name="presence">The motion presence configuration that defines the exit animation and transition to apply.</param>
    /// <param name="item">The motion item to which the exit animation will be applied.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the exit animation has been applied,
    /// or immediately if no exit animation is defined.</returns>
    public static Task ApplyExitAsync(this MotionPresence presence, MotionItem item)
    {
        if (item.Actions is null ||
            presence.Exit is null ||
            presence.Transition is null)
        {
            return Task.CompletedTask;
        }

        return item.Actions.ToVariantAsync(presence.Exit, presence.Transition);
    }
}
