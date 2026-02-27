namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes signature strokes by filtering out points that are too close together based on configurable distance
/// thresholds.
/// </summary>
/// <remarks>This processor is typically used to reduce noise in signature input by removing points that are
/// within a minimum distance of each other. It is designed to be used as part of a signature processing pipeline and
/// adheres to the distance constraints specified in the provided engine options. Points that are too far apart are not
/// interpolated by this processor; such cases are expected to be handled by subsequent processing steps.</remarks>
public sealed class StrokeDistanceFilterProcessor : IStrokeProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        var opt = options.Stroke;

        foreach (var stroke in strokes)
        {
            if (stroke.Points.Count == 0)
            {
                yield return stroke;
                continue;
            }

            var filtered = new List<SignaturePoint>();

            filtered.Add(stroke.Points[0]);

            for (var i = 1; i < stroke.Points.Count; i++)
            {
                var a = filtered[^1];
                var b = stroke.Points[i];

                var dx = b.X - a.X;
                var dy = b.Y - a.Y;
                var dist = Math.Sqrt(dx * dx + dy * dy);

                if (dist < opt.MinPointDistance)
                {
                    continue;
                }

                if (dist > opt.MaxPointDistance)
                {
                    // On laisse l’interpolation stroke-level gérer ça
                }

                filtered.Add(b);
            }

            yield return new SignatureStroke(stroke.Style.Clone(), filtered);
        }
    }
}

