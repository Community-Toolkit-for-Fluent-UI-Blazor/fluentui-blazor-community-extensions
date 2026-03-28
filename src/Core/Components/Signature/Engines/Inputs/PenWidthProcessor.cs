namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes input related to pen width adjustments.
/// </summary>
/// <remarks>Implements logic for handling changes in pen width, typically in drawing or inking scenarios. This
/// processor can be used in conjunction with other input processors to enable dynamic pen width control based on user
/// input.</remarks>
public sealed class PenWidthProcessor
    : IInputProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignaturePoint> Process(
        IEnumerable<SignaturePoint> points,
        PointerSample sample,
        SignatureStrokeEngineStyle style)
    {
        var pen = style.Pen;

        foreach (var p in points)
        {
            var  width = ComputeWidth(p, pen);
            p.Width = width;

            yield return p;
        }
    }

    /// <summary>
    /// Calculates the stroke width for a signature point based on pressure, velocity, and pen configuration options.
    /// </summary>
    /// <remarks>The stroke width is influenced by the pressure and velocity of the signature point, as well
    /// as the pen's width calculation mode. High velocity reduces the stroke width, and the result is always clamped
    /// within the configured minimum and maximum width.</remarks>
    /// <param name="p">The signature point containing pressure and velocity information used to determine the stroke width.</param>
    /// <param name="pen">The pen configuration options that specify stroke width calculation mode, minimum and maximum width, and
    /// velocity thresholds.</param>
    /// <returns>The computed stroke width as a double value, constrained by the pen's minimum and maximum width settings.</returns>
    private static double ComputeWidth(SignaturePoint p, SignaturePenEngineOptions pen)
    {
        double w;

        switch (pen.WidthMode)
        {
            case StrokeWidthMode.Linear:
                w = SurfaceMathUtils.Lerp(pen.MinWidth, pen.BaseWidth, p.Pressure);
                break;

            case StrokeWidthMode.Power:
                var t = Math.Pow(p.Pressure, pen.WidthPower);
                w = SurfaceMathUtils.Lerp(pen.MinWidth, pen.BaseWidth, t);
                break;

            default:
                w = pen.BaseWidth;
                break;
        }

        if (p.Velocity > pen.MaxVelocityForWidth)
        {
            var factor = SurfaceMathUtils.Clamp(
                1.0 - (p.Velocity - pen.MaxVelocityForWidth) / pen.MaxVelocityForWidth,
                0.0, 1.0);

            w *= factor;
        }

        w = SurfaceMathUtils.Clamp(w, pen.MinWidth, pen.MaxWidth);

        return w;
    }
}
