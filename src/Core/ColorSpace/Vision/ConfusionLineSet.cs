namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Represents the slope-intercept of possible visual impairments.
/// </summary>
internal static class ConfusionLineSet
{
    /// <summary>
    /// Gets the slope-intercept of the visual impairment <c>Protanopia</c>.
    /// </summary>
    public static ConfusionLine Protanopia { get; } = new(0.7465, 0.2535, 1.273463, -0.073894);

    /// <summary>
    /// Gets the slope-intercept of the visual impairment <c>Deutanopia</c>.
    /// </summary>
    public static ConfusionLine Deutanopia { get; } = new(1.4, -0.4, 0.968437, 0.003331);

    /// <summary>
    /// Gets the slope-intercept of the visual impairment <c>Tritanopia</c>.
    /// </summary>
    public static ConfusionLine Tritanopia { get; } = new(0.1748, 0, 0.062921, 0.292119);

    /// <summary>
    /// Gets the slope-intercept of an unknown visual impairment.
    /// </summary>
    public static ConfusionLine Unknown { get; } = new(0, 0, 0, 0);
}
