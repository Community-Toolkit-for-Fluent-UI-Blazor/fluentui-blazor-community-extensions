using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the theme options for a chart, including color settings for various chart elements and an indication
/// of whether the theme is dark or light.
/// </summary>
public sealed record ChartThemeOptions
{
    /// <summary>
    /// Gets a value indicating whether the current theme is dark.
    /// </summary>
    public bool IsDark { get; init; }

    /// <summary>
    /// Gets the background color for the chart.
    /// </summary>
    public Srgb8? Background { get; init; }

    /// <summary>
    /// Gets the foreground color for the chart.
    /// </summary>
    public Srgb8? Foreground { get; init; }

    /// <summary>
    /// Gets the grid color for the chart.
    /// </summary>
    public Srgb8? Grid { get; init; }

    /// <summary>
    /// Gets the axis color for the chart.
    /// </summary>
    public Srgb8? Axis { get; init; }

    /// <summary>
    /// Gets the text color for the chart.
    /// </summary>
    public Srgb8? Text { get; init; }

    /// <summary>
    /// Gets the variable name for the text color in CSS.
    /// </summary>
    public string? TextVar { get; init; }

    /// <summary>
    /// Gets the variable name for the background color in CSS.
    /// </summary>
    public string? BackgroundVar { get; init; }

    /// <summary>
    /// Gets the variable name for the foreground color in CSS.
    /// </summary>
    public string? ForegroundVar { get; init; }

    /// <summary>
    /// Gets the variable name for the grid color in CSS.
    /// </summary>
    public string? GridVar { get; init; }

    /// <summary>
    /// Gets the variable name for the axis color in CSS.
    /// </summary>
    public string? AxisVar { get; init; }

    /// <summary>
    /// Gets the predefined light theme options for charts.
    /// </summary>
    /// <remarks>Use this property to apply a light color scheme to chart components. The returned options are
    /// configured for light backgrounds and standard contrast.</remarks>
    public static ChartThemeOptions Light => new()
    {
        IsDark = false,
        Background = new Srgb8(255, 255, 255),
        Foreground = new Srgb8(0, 0, 0),
        Text = new Srgb8(32, 32, 32),
        Grid = new Srgb8(220, 220, 220),
        Axis = new Srgb8(64, 64, 64),
        BackgroundVar = "var(--colorNeutralBackground1)",
        ForegroundVar = "var(--colorNeutralForeground1)",
        TextVar = "var(--colorNeutralForeground1)",
        GridVar = "var(--colorNeutralStroke2)",
        AxisVar = "var(--colorNeutralStroke1)"
    };

    /// <summary>
    /// Gets a predefined set of chart theme options configured for dark mode charts.
    /// </summary>
    /// <remarks>Use this property to apply a dark color scheme to charts, ensuring optimal visibility and
    /// contrast in dark-themed user interfaces.</remarks>
    public static ChartThemeOptions Dark => new()
    {
        IsDark = true,
        Background = new Srgb8(18, 18, 18),
        Foreground = new Srgb8(240, 240, 240),
        Text = new Srgb8(230, 230, 230),
        Grid = new Srgb8(70, 70, 70),
        Axis = new Srgb8(150, 150, 150),
        BackgroundVar = "var(--colorNeutralBackground1)",
        ForegroundVar = "var(--colorNeutralForeground1)",
        TextVar = "var(--colorNeutralForeground1)",
        GridVar = "var(--colorNeutralStroke2)",
        AxisVar = "var(--colorNeutralStroke1)"
    };
}
