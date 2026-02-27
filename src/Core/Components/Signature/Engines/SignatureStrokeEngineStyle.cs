namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration options for customizing the appearance and behavior of signature stroke rendering
/// engines.
/// </summary>
/// <remarks>This class encapsulates various style and processing options, such as base stroke width, pressure
/// sensitivity, smoothing, stabilization, interpolation, and pen characteristics. Use this class to define or clone a
/// set of parameters that control how digital ink strokes are rendered in signature components.</remarks>
public sealed class SignatureStrokeEngineStyle
{
    /// <summary>
    /// Gets or sets the base width value used for layout or rendering calculations.
    /// </summary>
    public double BaseWidth { get; set; }

    /// <summary>
    /// Gets or sets the options that control pressure sensitivity for the signature input.
    /// </summary>
    public SignaturePressureOptions Pressure { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to control signature smoothing behavior.
    /// </summary>
    /// <remarks>Use this property to configure how smoothing is applied to the signature input, such as
    /// adjusting the level of smoothing or enabling specific smoothing algorithms. The effect of these options depends
    /// on the implementation of signature rendering.</remarks>
    public SignatureSmoothingOptions Smoothing { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to control signature stabilization behavior.
    /// </summary>
    /// <remarks>Use this property to configure how signature stabilization is applied during the signing
    /// process. Adjusting these options can affect the reliability and performance of signature generation.</remarks>
    public SignatureStabilizationOptions Stabilization { get; set; } = new();

    /// <summary>
    /// Gets or sets the interpolation options used for signature generation.
    /// </summary>
    public SignatureInterpolationOptions Interpolation { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the signature pen engine.
    /// </summary>
    /// <remarks>Use this property to customize the appearance and behavior of the signature pen, such as
    /// color, thickness, and smoothing options. Changes to these options affect how the signature is
    /// rendered.</remarks>
    public SignaturePenEngineOptions Pen { get; set; } = new();

    /// <summary>
    /// Creates a new instance of the SignatureStrokeEngineStyle class that is a deep copy of the current instance.
    /// </summary>
    /// <remarks>All nested objects referenced by the properties are also cloned, ensuring that modifications
    /// to the returned instance do not affect the original.</remarks>
    /// <returns>A new SignatureStrokeEngineStyle object with the same property values as the current instance.</returns>
    public SignatureStrokeEngineStyle Clone() => new()
    {
        BaseWidth = BaseWidth,
        Pressure = Pressure.Clone(),
        Smoothing = Smoothing.Clone(),
        Stabilization = Stabilization.Clone(),
        Interpolation = Interpolation.Clone(),
        Pen = Pen.Clone()
    };
}
