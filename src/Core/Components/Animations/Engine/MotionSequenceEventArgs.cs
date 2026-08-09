namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for events related to a motion sequence.
/// </summary>
/// <param name="Sequence">The motion sequence associated with the event.</param>
public sealed record MotionSequenceEventArgs(MotionSequence Sequence)
{
}
