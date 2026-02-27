namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for handling pressure input in signature capture scenarios, including stylus and
/// mouse-based pressure simulation.
/// </summary>
/// <remarks>Use this class to customize how pressure sensitivity is applied when capturing signatures. It allows
/// enabling stylus pressure, simulating pressure with mouse velocity, and adjusting the minimum, maximum, and curve of
/// simulated pressure. These options affect the appearance and responsiveness of signature strokes.</remarks>
public class SignaturePressureOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether pressure input from stylus is used.
    /// </summary>
    public bool StylusPressureEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to simulate pressure when using a mouse.
    /// </summary>
    public bool SimulateFromVelocity { get; set; } = true;

    /// <summary>
    /// Gets or sets the minimum simulated pressure.
    /// </summary>
    public double MinPressure { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the maximum simulated pressure.
    /// </summary>
    public double MaxPressure { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the curve applied to pressure.
    /// </summary>
    public PressureCurveMode CurveMode { get; set; } = PressureCurveMode.Linear;

    /// <summary>
    /// Gets or sets the maximum velocity allowed for the operation.
    /// </summary>
    /// <remarks>The value determines the upper limit for speed calculations. Adjust this property to control
    /// performance constraints as needed.</remarks>
    public double MaxVelocity { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the exponent used for power curve calculations when the curve mode is set to Power.
    /// </summary>
    public double PowerExponent { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the control point value used for Bezier curve calculations.
    /// </summary>
    public double BezierControl { get; set; } = 0.5;

    /// <summary>
    /// Gets or sets the steepness factor used in the sigmoid function calculation.
    /// </summary>
    public double SigmoidSteepness { get; set; } = 6.0;

    /// <summary>
    /// Resets all pressure-related settings to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the stylus pressure configuration to its initial state. This can
    /// be useful when reverting user customizations or preparing for a new drawing session.</remarks>
    public void Reset()
    {
        StylusPressureEnabled = true;
        SimulateFromVelocity = true;
        MinPressure = 0.1;
        MaxPressure = 1.0;
        CurveMode = PressureCurveMode.Linear;
        PowerExponent = 1.0;
        SigmoidSteepness = 6.0;
        BezierControl = 0.5;
        MaxVelocity = 2.0;
    }

    /// <summary>
    /// Clones the current instance of <see cref="SignaturePressureOptions"/>, creating a new object with the same property values.
    /// </summary>
    /// <returns>Returns the cloned instance.</returns>
    public SignaturePressureOptions Clone()
    {
        return new SignaturePressureOptions()
        {
            StylusPressureEnabled = StylusPressureEnabled,
            SimulateFromVelocity = SimulateFromVelocity,
            MinPressure = MinPressure,
            MaxPressure = MaxPressure,
            CurveMode = CurveMode,
            PowerExponent = PowerExponent,
            SigmoidSteepness = SigmoidSteepness,
            BezierControl = BezierControl,
            MaxVelocity = MaxVelocity
        };
    }
}
