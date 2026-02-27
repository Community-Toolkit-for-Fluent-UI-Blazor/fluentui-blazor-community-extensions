namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for events related to the rendering of a stroke segment in a signature component.
/// </summary>
/// <remarks>Use this event argument to access information about the segment being rendered, including its
/// endpoints and visual style. This can be useful for customizing stroke rendering or handling signature
/// input.</remarks>
/// <param name="P1">The starting point of the stroke segment.</param>
/// <param name="P2">The ending point of the stroke segment.</param>
/// <param name="Style">The rendering style applied to the stroke segment.</param>
public sealed record StrokeSegmentEventArgs(
    SignaturePoint P1,
    SignaturePoint P2,
    SignatureStrokeStyle Style);
