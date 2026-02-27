namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for stroke interpolation when processing signature input.
/// </summary>
/// <remarks>Use this class to control how signature strokes are interpolated, including enabling or disabling
/// interpolation, setting the spacing between resampled points, and determining whether a final resampling pass is
/// applied at the end of a stroke. These options can be adjusted to achieve smoother or more precise signature
/// rendering based on application requirements.</remarks>
public sealed class SignatureInterpolationOptions
{
    /// <summary>
    /// Enables or disables stroke interpolation.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Distance in pixels between resampled points.
    /// </summary>
    public double Spacing { get; set; } = 2.0;

    /// <summary>
    /// Whether to apply a final resampling pass when the stroke ends.
    /// </summary>
    public bool FinalResample { get; set; } = true;

    /// <summary>
    /// Gets or sets the stroke interpolation mode to apply when processing signature strokes.
    ///  The mode determines how the stroke points are modified during interpolation.
    /// </summary>
    public StrokeInterpolationMode Mode { get; set; }

    /// <summary>
    /// Resets the object's properties to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the initial state of the object if its properties have been
    /// modified. This can be useful when reusing the object for a new operation or scenario.</remarks>
    public void Reset()
    {
        Enabled = false;
        Spacing = 2.0;
        FinalResample = true;
    }

    /// <summary>
    /// Creates a new instance of the SignatureInterpolationOptions class that is a copy of the current instance.
    /// </summary>
    /// <remarks>The cloned instance is independent of the original. Changes to the properties of the cloned
    /// object do not affect the original object.</remarks>
    /// <returns>A new SignatureInterpolationOptions object with the same property values as the current instance.</returns>
    public SignatureInterpolationOptions Clone()
    {
        return new SignatureInterpolationOptions
        {
            Enabled = Enabled,
            Spacing = Spacing,
            FinalResample = FinalResample,
            Mode = Mode
        };
    }
}
