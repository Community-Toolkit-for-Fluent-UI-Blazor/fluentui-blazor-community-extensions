namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a signature stroke layer.
/// </summary>
public sealed record SignatureStrokeLayerOptions
{
    /// <summary>
    /// Gets The unique identifier for the stroke layer.
    /// </summary>
    /// <remarks>Used to distinguish between different layers in a signature.</remarks>
    public string LayerId { get; init; } = Guid.NewGuid().ToString();
}
