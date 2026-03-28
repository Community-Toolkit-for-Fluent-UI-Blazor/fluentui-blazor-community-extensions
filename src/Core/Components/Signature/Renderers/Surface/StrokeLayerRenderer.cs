namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render a stroke layer onto a signature surface using the specified rendering options.
/// </summary>
/// <remarks>This class implements the ISignatureSurfaceRenderer interface to support rendering operations for
/// stroke layers. It is intended for use in scenarios where signature surfaces require layered stroke rendering, such
/// as digital signature capture or drawing applications. Instances of this class are sealed and cannot be
/// inherited.</remarks>
internal sealed class StrokeLayerRenderer
    : ISignatureStrokeRenderer
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options)
    {
        if (string.IsNullOrEmpty(options.StrokeLayer.LayerId))
        {
            return;
        }

        var payload = PayloadFactory.CreateStrokeLayer(options.StrokeLayer.LayerId, strokes);

        target.AddLayer(new StrokeLayer(payload));
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
}
