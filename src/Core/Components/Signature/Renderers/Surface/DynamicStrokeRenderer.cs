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
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        Render(target, strokes, options);

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        var current = _strokeManager.CurrentStroke;

        if (current is null || current.Points.Count == 0)
        {
            return;
        }

        var payload = PayloadFactory.CreateDynamicStroke(current, [], null);

        target.DynamicLayer.SetDynamicStroke(payload);
    }
}

