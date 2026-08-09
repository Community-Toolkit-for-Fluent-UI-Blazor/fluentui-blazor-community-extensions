using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Extensions;

/// <summary>
/// Provides extension methods for applying motion variants to objects implementing the IMotionActions interface.
/// </summary>
/// <remarks>This static class contains helper methods that enable batch application of motion variants using
/// asynchronous operations. It is intended to simplify the process of animating multiple properties defined in a
/// MotionVariant with a specified transition.</remarks>
public static class MotionActionsVariantExtensions
{
    /// <summary>
    /// Animates all double-valued properties in the specified motion variant using the provided transition.
    /// </summary>
    /// <remarks>Only properties in the variant with double values are animated. Animations are performed
    /// concurrently and the method awaits their completion.</remarks>
    /// <param name="actions">The motion actions interface used to perform the animations.</param>
    /// <param name="variant">The motion variant containing the target property values to animate.</param>
    /// <param name="transition">The transition to apply to each animated property.</param>
    /// <returns>A task that represents the asynchronous animation operation. The task completes when all animations have
    /// finished.</returns>
    public static async Task ToVariantAsync(
        this IMotionActions actions,
        MotionVariant variant,
        MotionTransition transition)
    {
        var tasks = new List<Task>();

        foreach (var kvp in variant.Values)
        {
            if (kvp.Value is double d)
            {
                tasks.Add(actions.AnimateDoubleAsync(kvp.Key, d, transition));
            }
        }

        var buckets = tasks.Interleaved();
        var last = buckets[^1];

        var t = await last.ConfigureAwait(false);
        await t.ConfigureAwait(false);
    }
}
