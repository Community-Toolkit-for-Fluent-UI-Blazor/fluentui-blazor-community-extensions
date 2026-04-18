namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for defining a clipping path in a chart rendering context.
/// </summary>
public sealed class ClipPathPayload : ILayerPayload
{
    /// <summary>
    /// Gets the unique identifier for this payload.
    /// </summary>
    public string Id { get; } = $"clip-path-{Guid.NewGuid()}";

    /// <summary>
    /// Gets the X-coordinate value.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y-coordinate value.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the width value for the current instance.
    /// </summary>
    public required double Width { get; init; }

    /// <summary>
    /// Gets the height value.
    /// </summary>
    public required double Height { get; init; }
}
