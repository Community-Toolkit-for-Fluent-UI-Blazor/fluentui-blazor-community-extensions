using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Extensions;

/// <summary>
/// Provides extension methods for applying motion gestures to motion items asynchronously.
/// </summary>
/// <remarks>These extension methods enable convenient gesture-based animations on motion items by mapping gesture
/// names to their corresponding motion variants. Intended for use with the MotionGestures and MotionItem types to
/// facilitate gesture-driven UI effects.</remarks>
public static class MotionGestureExtensions
{
    /// <summary>
    /// Applies the specified gesture animation to the given motion item asynchronously using the provided transition.
    /// </summary>
    /// <remarks>If the specified gesture name does not correspond to a known gesture variant, no animation is
    /// applied and the method completes immediately.</remarks>
    /// <param name="gestures">The collection of motion gestures containing the gesture variants to apply.</param>
    /// <param name="gestureName">The name of the gesture to apply. Supported values include "hover", "press", "tap", and "drag".</param>
    /// <param name="item">The motion item to which the gesture animation will be applied.</param>
    /// <param name="transition">The transition to use when animating the motion item to the gesture variant.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the gesture animation has been
    /// applied, or immediately if the gesture name is not recognized.</returns>
    public static Task ApplyGestureAsync(
        this MotionGestures gestures,
        MotionGestureName gestureName,
        MotionItem item,
        MotionTransition transition)
    {
        if (item.Actions is null)
        {
            return Task.CompletedTask;
        }

        var variant = gestureName switch
        {
            MotionGestureName.Hover => gestures.Hover,
            MotionGestureName.Press => gestures.Press,
            MotionGestureName.Tap => gestures.Tap,
            MotionGestureName.Drag => gestures.Drag,
            _ => null
        };

        if (variant is null)
        {
            return Task.CompletedTask;
        }

        return item.Actions.ToVariantAsync(variant, transition);
    }
}
