using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents an event related to a pointer interaction with a chart element.
/// </summary>
public sealed record ChartPointerEvent
{
    /// <summary>
    /// Gets the unique identifier for the group.
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier for the element.
    /// </summary>

    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the coordinates of the pointer event relative to the chart's coordinate system.
    /// </summary>
    public ChartPoint Position { get; init; }
}
