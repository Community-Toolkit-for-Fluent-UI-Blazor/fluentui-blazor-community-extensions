using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a configurable text component for displaying descriptive content within a slideshow, allowing
/// customization of text, color, font, and layout styles.
/// </summary>
/// <remarks>Use this component to present descriptive or explanatory text in a slideshow interface. The
/// appearance of the text can be customized using standard CSS style values for color, font family, font size, font
/// weight, letter spacing, line height, and margin. All style-related properties accept valid CSS values as strings. If
/// a property is not set, a default value is applied as specified in each property's documentation.</remarks>
public partial class SlideshowCaptionText : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the text to display in the description area of the slideshow.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the CSS color value to apply to the component's content.
    /// </summary>
    /// <remarks>If not set, the default color is "white". Accepts any valid CSS color string, such as a named
    /// color (e.g., "red"), a hex code (e.g., "#FF0000"), or an RGB/RGBA value.</remarks>
    [Parameter]
    public string? Color { get; set; } = "white";

    /// <summary>
    /// Gets or sets the CSS font family to apply to the component's text.
    /// </summary>
    [Parameter]
    public string? FontFamily { get; set; } = "sans-serif";

    /// <summary>
    /// Gets or sets the CSS font size to apply to the component's content.
    /// </summary>
    /// <remarks>The value should be a valid CSS font-size string, such as "16px", "1.5em", or "large". If not
    /// set, the default value is "24px".</remarks>
    [Parameter]
    public string? FontSize { get; set; } = "24px";

    /// <summary>
    /// Gets or sets the font weight to apply to the text content.
    /// </summary>
    /// <remarks>Accepts standard CSS font-weight values, such as numeric values (e.g., "400", "700") or
    /// keywords (e.g., "normal", "bold"). The default value is "400".</remarks>
    [Parameter]
    public string? FontWeight { get; set; } = "400";

    /// <summary>
    /// Gets or sets the rotation angle (in degrees) to apply to the text content.
    /// </summary>
    [Parameter]
    public int Rotation { get; set; } = 0;

    /// <summary>
    /// Gets or sets the text shadow effect to apply to the text content.
    /// </summary>
    [Parameter]
    public string? Shadow { get; set; }

    /// <summary>
    /// Gets or sets the highlight background color to apply to the text content.
    /// </summary>
    [Parameter]
    public string? Highlight { get; set; }

    /// <summary>
    /// Gets or sets the CSS letter-spacing value to apply to the element's text content.
    /// </summary>
    [Parameter]
    public string? LetterSpacing { get; set; } = "0";

    /// <summary>
    /// Gets or sets the line height value for the element.
    /// </summary>
    public string? LineHeight { get; set; } = "64px";

    /// <summary>
    /// Gets or sets the margin value to apply, typically used to define spacing around an element.
    /// </summary>
    public string? Margin { get; set; } = "0";

    /// <summary>
    /// Gets the computed CSS style string based on the current style-related property values.
    /// </summary>
    /// <remarks>This property combines individual style settings, such as color, font, and spacing, into a
    /// single CSS style string. It is intended for internal use when rendering elements with dynamic styles.</remarks>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("color", Color, !string.IsNullOrEmpty(Color))
        .AddStyle("font-family", FontFamily, !string.IsNullOrEmpty(FontFamily))
        .AddStyle("font-size", FontSize, !string.IsNullOrEmpty(FontSize))
        .AddStyle("font-weight", FontWeight, !string.IsNullOrEmpty(FontWeight))
        .AddStyle("letter-spacing", LetterSpacing, !string.IsNullOrEmpty(LetterSpacing))
        .AddStyle("line-height", LineHeight, !string.IsNullOrEmpty(LineHeight))
        .AddStyle("margin", Margin, !string.IsNullOrEmpty(Margin))
        .AddStyle("transform", $"rotate({Rotation}deg)", Rotation != 0)
        .AddStyle("text-shadow", Shadow, !string.IsNullOrEmpty(Shadow))
        .AddStyle("background", Highlight, !string.IsNullOrEmpty(Highlight))
        .Build();
}
