namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for customizing stroke behavior in a signature input component.
/// </summary>
/// <remarks>Use this class to control stroke smoothing, simplification, interpolation, and boundary clipping when
/// capturing or rendering signature input. Adjusting these options can affect the quality, performance, and appearance
/// of signature strokes. All properties are mutable and can be reset to their default values using the Reset
/// method.</remarks>
public class SignatureStrokeOptions
{
    /// <summary>
    /// Minimum distance between two points before adding a new stroke point.
    /// </summary>
    public double MinPointDistance { get; set; } = 0.5;

    /// <summary>
    /// Maximum distance allowed between two points before interpolation kicks in.
    /// </summary>
    public double MaxPointDistance { get; set; } = 3.0;

    /// <summary>
    /// Whether stroke simplification is enabled (Douglas-Peucker).
    /// </summary>
    public bool SimplificationEnabled { get; set; } = true;

    /// <summary>
    /// Tolerance for stroke simplification.
    /// </summary>
    public double SimplificationTolerance { get; set; } = 0.8;

    /// <summary>
    /// Whether interpolation is enabled for smoothing strokes.
    /// </summary>
    public bool InterpolationEnabled { get; set; } = true;

    /// <summary>
    /// Maximum number of points stored per stroke (0 = unlimited).
    /// </summary>
    public int MaxPointsPerStroke { get; set; }

    /// <summary>
    /// Whether strokes should be clipped to the canvas boundaries.
    /// </summary>
    public bool ClipToBounds { get; set; } = true;

    /// <summary>
    /// Resets all stroke configuration properties to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the stroke settings to their initial state. This is useful when
    /// you want to discard customizations and revert to the standard configuration for stroke processing.</remarks>
    public void Reset()
    {
        MinPointDistance = 0.5;
        MaxPointDistance = 3.0;
        SimplificationEnabled = true;
        SimplificationTolerance = 0.8;
        InterpolationEnabled = true;
        MaxPointsPerStroke = 0;
        ClipToBounds = true;
    }
}
