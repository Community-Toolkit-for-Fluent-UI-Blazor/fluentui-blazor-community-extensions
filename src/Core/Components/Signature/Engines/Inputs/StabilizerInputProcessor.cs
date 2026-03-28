namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes input signature points to apply stabilization, smoothing the input based on the specified stroke style
/// options.
/// </summary>
/// <remarks>This processor modifies the sequence of input points to reduce jitter and create smoother strokes by
/// interpolating between the previous output and the current input point. Stabilization is only applied if enabled in
/// the provided stroke style. The processor maintains internal state between calls, so it is not thread-safe and should
/// not be shared across concurrent input streams.</remarks>
public sealed class StabilizerInputProcessor : IInputProcessor
{
    /// <summary>
    /// Represents the most recent output signature point, or null if no output has been produced.
    /// </summary>
    private SignaturePoint? _lastOutput;

    /// <inheritdoc />
    public IEnumerable<SignaturePoint> Process(
        IEnumerable<SignaturePoint> points,
        PointerSample sample,
        SignatureStrokeEngineStyle style)
    {
        var options = style.Stabilization;

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
            if (_lastOutput is null)
            {
                _lastOutput = p.Clone();
                yield return p;
                continue;
            }

            var f = options.Factor;
            var x = SurfaceMathUtils.Lerp(_lastOutput.X, p.X, f);
            var y = SurfaceMathUtils.Lerp(_lastOutput.Y, p.Y, f);

            var stabilized = new SignaturePoint(
                x, y,
                p.Pressure,
                p.Velocity,
                p.Width,
                p.Timestamp
            );

            _lastOutput = stabilized;

            yield return stabilized;
        }
    }
}
