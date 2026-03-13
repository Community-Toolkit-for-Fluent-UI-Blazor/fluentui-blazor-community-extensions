namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of a motion transition, including both the source and target motion states.
/// </summary>
/// <remarks>Use this class to encapsulate the starting and ending states involved in a motion transition, such as
/// for animations or UI state changes. Both the source and target states are accessible as read-only
/// properties.</remarks>
public sealed class MotionTransitionState
{
    /// <summary>
    /// Gets the source motion state for the current component.
    /// </summary>
    public MotionState Source { get; } = new();

    /// <summary>
    /// Gets the target motion state for the current operation.
    /// </summary>
    public MotionState Target { get; } = new();

    /// <summary>
    /// Resets the state of the current object and its associated components to their initial values.
    /// </summary>
    internal void Reset()
    {
        Target.Reset();
        Source.Reset();
    }
}
