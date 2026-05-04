namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the parameters used to configure an eraser tool, including position, size, shape, and behavior options.
/// </summary>
/// <remarks>This class is typically used to encapsulate all relevant settings for an eraser operation in a
/// drawing or graphics application. The properties allow customization of the eraser's appearance and effect, such as
/// its position, size, shape, softness, and tolerance. The meaning of the integer properties, such as Shape and Mode,
/// depends on the context in which the eraser is used and may correspond to specific predefined values or modes
/// supported by the application.</remarks>
public class EraserPayload : ILayerPayload
{
    /// <summary>
    /// Gets or sets the X-coordinate value.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate value.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the size value of the eraser.
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the radius of the shape.
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// Gets or sets the shape of the eraser.
    /// </summary>
    public int Shape { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether soft edges are applied to the eraser.
    /// </summary>
    public bool SoftEdges { get; set; }

    /// <summary>
    /// Gets or sets the radius, in pixels, of the soft edge effect applied to the component.
    /// </summary>
    public double SoftEdgeRadius { get; set; }

    /// <summary>
    /// Gets or sets the mode value for the current instance.
    /// </summary>
    public int Mode { get; set; }

    /// <summary>
    /// Gets or sets the acceptable margin of error for calculations or comparisons.
    /// </summary>
    /// <remarks>Use this property to specify how much deviation is allowed when determining equality or
    /// accuracy in operations that involve floating-point values. Adjusting the tolerance can help account for rounding
    /// errors or imprecise measurements.</remarks>
    public double Tolerance { get; set; }
}

