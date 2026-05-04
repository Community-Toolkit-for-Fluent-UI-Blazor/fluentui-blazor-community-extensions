namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single pointer sample captured during the input process,
///  containing information about the pointer's position, pressure, velocity, and timestamp.
/// </summary>
/// <param name="X">X-coordinate of the pointer sample.</param>
/// <param name="Y">Y-coordinate of the pointer sample.</param>
/// <param name="Timestamp">Timestamp of the pointer sample, in milliseconds.</param>
/// <param name="Pressure">Pressure applied at the pointer sample, between 0 and 1.</param>
/// <param name="Velocity">Velocity of the pointer at the sample</param>
/// <param name="AltKey">Value indicating if the Alt key is pressed</param>
/// <param name="CtrlKey">Value indicating if the Ctrl key is pressed.</param>
/// <param name="IsDown">Value indicating if the down key is pressed.</param>
/// <param name="ShiftKey">Value indicating if the shift key is pressed.</param>
public sealed record PointerSample(
    double X,
    double Y,
    long Timestamp,
    double? Pressure = null,
    double? Velocity = null,
    bool IsDown = false,
    bool CtrlKey = false,
    bool ShiftKey = false,
    bool AltKey = false)
{
    /// <summary>
    /// Gets the unique identifier for the sample.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();
}
