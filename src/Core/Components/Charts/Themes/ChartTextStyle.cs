using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the style settings for chart text elements, including font family, size, weight, and color.
/// </summary>
/// <remarks>Use this type to configure the appearance of text in chart components, such as titles, subtitles,
/// axes, legends, labels, and values. Predefined static properties provide common default styles for typical chart
/// elements.</remarks>
public sealed record ChartTextStyle
{
    /// <summary>
    /// Gets the font family used for rendering text in the component.
    /// </summary>
    public string FontFamily { get; init; } = "'Segoe UI', 'Segoe UI Web (West European)', -apple-system, BlinkMacSystemFont, Roboto, 'Helvetica Neue', sans-serif";

    /// <summary>
    /// Gets the font size used to display the text.
    /// </summary>
    public double FontSize { get; init; } = 12;

    /// <summary>
    /// Gets the font weight to apply to the text content.
    /// </summary>
    public TextWeight FontWeight { get; init; }

    /// <summary>
    /// Gets the color value represented in the sRGB color space with 8-bit channels.
    /// </summary>
    public Srgb8 Color { get; init; }

    /// <summary>
    /// Gets the default style settings for chart titles, including font size and weight.
    /// </summary>
    /// <remarks>Use this property to apply a consistent appearance to chart titles across your application.
    /// The default style sets the font size to 18 and the font weight to 600.</remarks>
    public static ChartTextStyle DefaultTitle => new()
    {
        FontSize = 32,
        FontWeight = TextWeight.Semibold
    };

    /// <summary>
    /// Gets the default text style used for chart subtitles.
    /// </summary>
    /// <remarks>The default style sets the font size to 400 and the font weight to Medium. Use this property
    /// to maintain consistency in subtitle appearance across charts.</remarks>
    public static ChartTextStyle DefaultSubtitle => new()
    {
        FontSize = 16,
        FontWeight = TextWeight.Medium
    };

    /// <summary>
    /// Gets the default text style used for chart axes.
    /// </summary>
    /// <remarks>The returned style specifies the default font size and weight for axis labels in charts. Use
    /// this property as a baseline when customizing axis appearance.</remarks>
    public static ChartTextStyle DefaultAxis => new()
    {
        FontSize = 14,
        FontWeight = TextWeight.Regular
    };

    /// <summary>
    /// Gets the default text style used for chart legends.
    /// </summary>
    /// <remarks>The returned style specifies the default font size and weight for legend text. Use this
    /// property as a base when customizing legend appearance, or to ensure consistency across multiple
    /// charts.</remarks>
    public static ChartTextStyle DefaultLegend => new()
    {
        FontSize = 14,
        FontWeight = TextWeight.Regular
    };

    /// <summary>
    /// Gets the default text style used for chart labels.
    /// </summary>
    /// <remarks>The default style specifies a font size of Size200 and a regular font weight. Use this
    /// property to apply a consistent appearance to chart labels when no custom style is specified.</remarks>
    public static ChartTextStyle DefaultLabel => new()
    {
        FontSize = 14,
        FontWeight = TextWeight.Regular
    };

    /// <summary>
    /// Gets the default text style settings for chart elements.
    /// </summary>
    /// <remarks>The default style uses a font size of Size200 and a regular font weight. Use this property to
    /// obtain a baseline style configuration when customizing chart text appearance.</remarks>
    public static ChartTextStyle DefaultValue => new()
    {
        FontSize = 14,
        FontWeight = TextWeight.Regular
    };

    /// <summary>
    /// Gets a predefined text style for small chart labels with regular font weight and a small font size.
    /// </summary>
    /// <remarks>Use this style to ensure consistency for small labels in chart components. The style applies
    /// a regular font weight and a font size suitable for less prominent text elements.</remarks>
    public static ChartTextStyle SmallLabel => new()
    {
        FontSize = 10,
        FontWeight = TextWeight.Regular
    };

    /// <summary>
    /// Gets the default text style used for chart tooltips.
    /// </summary>
    /// <remarks>This style specifies the font size and weight applied to tooltip text in charts. Use this
    /// property to ensure consistent tooltip appearance across chart components.</remarks>
    public static ChartTextStyle Tooltip => new()
    {
        FontSize = 12,
        FontWeight = TextWeight.Medium
    };
}

