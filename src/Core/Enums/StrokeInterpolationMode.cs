namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the interpolation mode used to render strokes between points.
/// </summary>
/// <remarks>Use this enumeration to control how intermediate points are calculated when drawing or processing
/// strokes. Different modes provide varying levels of smoothness and accuracy, depending on the desired visual effect
/// or performance requirements.</remarks>
public enum StrokeInterpolationMode
{
    /// <summary>
    /// No interpolation is applied.
    /// </summary>
    None,

    /// <summary>
    /// Represents a resampling interpolation mode that generates intermediate points by resampling the original stroke
    /// </summary>
    Resample,

    /// <summary>
    /// Represents the Catmull-Rom spline interpolation method used for generating smooth curves through a set of
    /// points.
    /// </summary>
    CatmullRom,

    /// <summary>
    /// Represents a quadratic value or entity, typically used to model quadratic equations or operations.
    /// </summary>
    Quadratic,

    /// <summary>
    /// Represents a Bézier curve used for modeling smooth, continuous curves in graphical applications.
    /// </summary>
    Bezier
}
