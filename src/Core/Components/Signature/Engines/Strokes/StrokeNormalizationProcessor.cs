namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes a collection of signature strokes by normalizing their point properties to ensure valid and consistent
/// values.
/// </summary>
/// <remarks>This processor adjusts each point in the provided strokes to ensure that pressure, width, velocity,
/// and coordinates are within valid ranges or set to default values if invalid. This normalization helps prevent
/// rendering or analysis issues caused by invalid or out-of-range stroke data. The processor does not modify the
/// structure of the strokes or points beyond normalization.</remarks>
public sealed class StrokeNormalizationProcessor : IStrokeProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        foreach (var stroke in strokes)
        {
            var style = stroke.Style.Engine;

            foreach (var p in stroke.Points)
            {
                if (double.IsNaN(p.Pressure))
                {
                    p.Pressure = 1.0;
                }

                p.Pressure = Math.Clamp(p.Pressure, 0.0, 1.0);

                if (double.IsNaN(p.Width) || p.Width <= 0)
                {
                    p.Width = style.BaseWidth;
                }

                if (double.IsNaN(p.Velocity) || p.Velocity < 0)
                {
                    p.Velocity = 0;
                }

                if (double.IsNaN(p.X))
                {
                    p.X = 0;
                }

                if (double.IsNaN(p.Y))
                {
                    p.Y = 0;
                }
            }

            yield return stroke;
        }
    }
}

