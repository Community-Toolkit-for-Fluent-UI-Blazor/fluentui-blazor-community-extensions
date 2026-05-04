namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render the current dynamic signature stroke on a surface render target.
/// </summary>
internal sealed class DynamicStrokeRenderer : ISignatureStrokeRenderer
{
    private readonly StrokeManager _strokeManager;

    /// <summary>
    /// Initializes a new instance of the StrokeRenderer class with the specified selection manager and selection
    /// rectangle provider.
    /// </summary>
    /// <param name="strokeManager">The stroke manager used to track the current stroke.</param>
    public DynamicStrokeRenderer(
        StrokeManager strokeManager)
    {
        _strokeManager = strokeManager;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        return ValueTask.FromResult(Compose(target, strokes, options));
    }

    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        var current = _strokeManager.CurrentStroke;

        if (current is null || current.Points.Count == 0)
        {
            return false;
        }

        var payload = PayloadFactory.CreateDynamicStroke(current, [], null);

        target.AddLayer(new DynamicStrokeLayer(payload));

        return true;
    }
}

