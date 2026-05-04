namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available modes for interpreting or applying a pressure curve.
/// </summary>
/// <remarks>Use this enumeration to select how input pressure values are mapped or transformed, such as in
/// drawing or input scenarios where pressure sensitivity affects the output. Each mode represents a different
/// mathematical approach to mapping input pressure to output values.</remarks>
public enum PressureCurveMode
{
    /// <summary>
    /// Specifies a linear mode.
    /// </summary>
    Linear,

    /// <summary>
    /// Specifies a power mode, where the input pressure is raised to a specified exponent to create a non-linear curve.
    /// </summary>
    Power,

    /// <summary>
    /// Specifies a sigmoid mode, where the input pressure is transformed using a sigmoid function to create an S-shaped curve that can provide a more natural response for certain types of input.
    /// </summary>
    Sigmoid,

    /// <summary>
    /// Specifies a Bezier mode, where the input pressure is mapped using a Bezier curve defined by control points, allowing for highly customizable pressure response curves.
    /// </summary>
    Bezier,

    /// <summary>
    /// Gets or sets the velocity value.
    /// </summary>
    Velocity,

    /// <summary>
    /// Represents a hybrid option.
    /// </summary>
    Hybrid
}

