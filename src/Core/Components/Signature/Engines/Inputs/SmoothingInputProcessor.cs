namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes input signature points by applying a smoothing algorithm to reduce noise and create smoother stroke paths.
/// </summary>
/// <remarks>This processor uses a moving average over a configurable window size to smooth the X and Y
/// coordinates of input points. Smoothing is only applied if enabled in the provided stroke style. The processor is
/// typically used to enhance the visual quality of drawn strokes by minimizing jitter from input devices.</remarks>
public sealed class SmoothingInputProcessor : IInputProcessor
{
    /// <summary>
    /// Represents the queue of signature points used as a sliding window for processing or analysis.
    /// </summary>
    /// <remarks>This queue maintains the current set of signature points within the active window. The
    /// specific usage and window size depend on the context in which it is used.</remarks>
    private readonly Queue<SignaturePoint> _window = new();

    /// <inheritdoc />
    public IEnumerable<SignaturePoint> Process(
        IEnumerable<SignaturePoint> points,
        PointerSample sample,
        SignatureStrokeEngineStyle style)
    {
        var options = style.Smoothing;

        if (!options.Enabled)
        {
            foreach (var p in points)
            {
                yield return p;
            }

            yield break;
        }

        foreach (var p in points)
        {
            _window.Enqueue(p.Clone());

            while (_window.Count > options.WindowSize)
            {
                _window.Dequeue();
            }

            var x = _window.Average(pt => pt.X);
            var y = _window.Average(pt => pt.Y);

            yield return new SignaturePoint(
                x, y,
                p.Pressure,
                p.Velocity,
                p.Width,
                p.Timestamp
            );
        }
    }
}

