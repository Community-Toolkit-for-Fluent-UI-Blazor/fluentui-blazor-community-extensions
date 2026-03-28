namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes a sequence of signature points and calculates the velocity for each point based on its position and
/// timestamp.
/// </summary>
/// <remarks>This processor is typically used to augment signature or stroke data with velocity information, which
/// can be useful for rendering effects or gesture analysis. The velocity is computed as the distance between
/// consecutive points divided by the elapsed time in seconds. The first point in the sequence is assigned a velocity of
/// zero.</remarks>
public sealed class VelocityInputProcessor : IInputProcessor
{
    /// <summary>
    /// Represents the last processed signature point, used to calculate velocity for the next point.
    /// </summary>
    private SignaturePoint? _lastPoint;

    /// <inheritdoc />
    public IEnumerable<SignaturePoint> Process(
        IEnumerable<SignaturePoint> points,
        PointerSample sample,
        SignatureStrokeEngineStyle style)
    {
        foreach (var point in points)
        {
            point.Velocity = _lastPoint is null ? 0 : SurfaceMathUtils.Velocity(_lastPoint, point);

            _lastPoint = point;

            yield return point;
        }
    }
}

