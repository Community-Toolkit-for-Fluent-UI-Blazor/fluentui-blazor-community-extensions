namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the axis along which a wave operation is applied.
/// </summary>
/// <remarks>Use this enumeration to indicate whether the wave effect should be performed along the X axis, Y
/// axis, or as a rotational transformation. The selected value determines the direction or type of transformation
/// applied in wave-related operations.</remarks>
public enum WaveAxis
{
    /// <summary>
    /// Represents the X-axis, indicating that the wave effect should be applied horizontally.
    /// </summary>
    X,

    /// <summary>
    /// Represents the Y-axis, indicating that the wave effect should be applied vertically.
    /// </summary>
    Y,

    /// <summary>
    /// Represents the Z-axis, indicating that the wave effect should be applied as a rotational transformation.
    /// </summary>
    Rotation
}

