namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes signature strokes to ensure that each stroke contains no more than a specified maximum number of points.
/// </summary>
/// <remarks>This processor is typically used to reduce the number of points in signature strokes for performance
/// optimization or data size constraints. Strokes with a number of points less than or equal to the maximum are
/// returned unchanged. Strokes exceeding the maximum have their points reduced by sampling at regular intervals. The
/// maximum number of points per stroke is determined by the engine options provided.</remarks>
public sealed class StrokeMaxPointsProcessor : IStrokeProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        var max = options.Stroke.MaxPointsPerStroke;

        if (max > 0)
        {
            foreach (var stroke in strokes)
            {
                if (stroke.Points.Count <= max)
                {
                    yield return stroke;
                    continue;
                }

                var reduced = stroke.Points
                    .Where((p, i) => i % (stroke.Points.Count / max + 1) == 0)
                    .ToList();

                yield return new SignatureStroke(stroke.Style.Clone(), reduced);
            }
        }
    }
}
