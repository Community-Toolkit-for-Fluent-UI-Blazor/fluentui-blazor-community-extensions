using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a single slice in a pie chart, containing layout and data information for rendering and interaction.
/// </summary>
/// <remarks>This type encapsulates the geometric and data properties required to display and identify a pie chart
/// segment. It is intended for internal use within chart rendering logic and is not designed for direct instantiation
/// by consumers.</remarks>
internal sealed class ChartPieSlice
{
    /// <summary>
    /// Gets the identifier of the payload.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the start angle of the pie slice.
    /// </summary>
    public required double StartAngle { get; init; }

    /// <summary>
    /// Gets the end angle of the pie slice.
    /// </summary>
    public required double EndAngle { get; init; }

    /// <summary>
    /// Gets the mid angle of the pie slice.
    /// </summary>
    public required double MidAngle { get; init; }

    /// <summary>
    /// Gets the position of the label for the pie slice.
    /// </summary>
    public ChartPoint? LabelPosition { get; init; }

    /// <summary>
    /// Gets the index of the slice.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// Gets the value of the slice.
    /// </summary>
    public required double Value { get; init; }

    /// <summary>
    /// Gets the unique identifier for the group associated with this instance.
    /// </summary>
    public required string GroupId { get; init; }

    /// <summary>
    /// Gets the interaction state of the slice.
    /// </summary>
    public required ChartInteractionState InteractionState { get; init; }
}
