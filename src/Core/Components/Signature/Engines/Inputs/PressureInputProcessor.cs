namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes a sequence of signature points by applying pressure simulation and adjustment based on the provided
/// pointer sample and stroke style.
/// </summary>
/// <remarks>This processor modifies the pressure and width of each signature point according to the pressure
/// options defined in the stroke style. It supports both stylus-based and velocity-based pressure simulation, and
/// applies configurable pressure curves such as linear, power, sigmoid, or Bezier. The resulting points reflect the
/// intended pressure dynamics for rendering or analysis. This class is sealed and intended for use as an input
/// processor within signature or drawing workflows.</remarks>
public sealed class PressureInputProcessor : IInputProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignaturePoint> Process(
        IEnumerable<SignaturePoint> points,
        PointerSample sample,
        SignatureStrokeEngineStyle style)
    {
        var options = style.Pressure;
        var hasPressure = options.StylusPressureEnabled && sample.Pressure.HasValue;
        var pressureValue = sample.Pressure.GetValueOrDefault();

        foreach (var point in points)
        {
            var p = 1.0;

            if (hasPressure)
            {
                p = pressureValue;
            }

            p = Math.Clamp(p, 0.0, 1.0);

            p = options.CurveMode switch
            {
                PressureCurveMode.Linear => p,
                PressureCurveMode.Power => Math.Pow(p, options.PowerExponent),
                PressureCurveMode.Sigmoid => SignatureMathUtils.Sigmoid(p, options.SigmoidSteepness),
                PressureCurveMode.Bezier => SignatureMathUtils.Bezier(p, options.BezierControl),
                PressureCurveMode.Velocity => SimulateFromVelocity(point.Velocity, options),
                PressureCurveMode.Hybrid => (pressureValue * SimulateFromVelocity(point.Velocity, options)) * 0.5,
                _ => p
            };

            p = SignatureMathUtils.Clamp(p, options.MinPressure, options.MaxPressure);

            point.Pressure = p;

            yield return point;
        }
    }

    /// <summary>
    /// Calculates a simulated pressure value based on the velocity of the pointer movement, using the specified pressure options.
    /// </summary>
    /// <param name="options">The pressure options that may influence the simulation, such as maximum velocity.</param>
    /// <param name="velocity">The velocity of the pointer movement, which is used to determine the simulated pressure.</param>
    /// <returns>A simulated pressure value between 0.0 and 1.0, where higher velocity results in lower pressure.</returns>
    private static double SimulateFromVelocity(
        double velocity,
        SignaturePressureOptions options)
    {
        var t = SignatureMathUtils.Clamp(velocity / options.MaxVelocity, 0.0, 1.0);

        return 1.0 - t;
    }
}

