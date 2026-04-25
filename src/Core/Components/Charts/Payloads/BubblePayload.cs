namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a bubble chart, containing a collection of bubble points to be rendered.
/// </summary>
public sealed record BubblePayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the collection of bubble point.
    /// </summary>
    public required IReadOnlyList<BubblePointPayload> Points { get; init; }
}

