namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render eraser on a drawing surface using the specified signature engine options.
/// </summary>
/// <remarks>This renderer is intended for use with digital ink or signature capture scenarios where erasing
/// strokes or marks is required. It implements the ISurfaceRenderer interface to support both synchronous and
/// asynchronous rendering operations.</remarks>
/// <param name="pointerSample">A delegate that returns the current pointer sample</param>
/// <param name="isEraser">A delegate that returns a boolean value indicating whether the eraser tool is currently active.</param>
public sealed class EraserComposer(
    Func<bool> isEraser,
    Func<PointerSample?> pointerSample): ISurfaceComposer<SignatureEraserOptions>
{
    /// <inheritdoc />
    public bool Compose(ISurfaceRenderTarget target, SignatureEraserOptions options)
    {
        if (!isEraser())
        {
            return false;
        }

        var sample = pointerSample();

        if (sample is null)
        {
            return false;
        }

        var payload = PayloadFactory.CreateEraser(sample, options);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new EraserLayer(payload));

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, SignatureEraserOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}
