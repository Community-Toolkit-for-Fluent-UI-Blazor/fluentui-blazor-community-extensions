namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Represents a helper class for mathematical conversions and calculations.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Represents the conversion factor from degrees to radians.
    /// </summary>
    public const double Radians = Math.PI / 180;

    /// <summary>
    /// Represents the conversion factor from radians to degrees.
    /// </summary>
    public const double Degrees = 180 / Math.PI;

    /// <summary>
    /// Represents the value of 2π (two times pi).
    /// </summary>
    public const double TwoPi = 2 * Math.PI;

    /// <summary>
    /// Converts an angle from degrees to radians.
    /// </summary>
    /// <param name="degrees">The angle in degrees to be converted.</param>
    /// <returns>The equivalent angle in radians.</returns>
    public static double ToRadians(double degrees) => degrees * Radians;

    /// <summary>
    /// Converts an angle from radians to degrees.
    /// </summary>
    /// <param name="radians">The angle in radians to convert.</param>
    /// <returns>The equivalent angle in degrees.</returns>
    public static double ToDegrees(double radians) => radians * Degrees;
}
