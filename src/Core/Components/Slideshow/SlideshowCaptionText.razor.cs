using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a configurable text component for displaying descriptive content within a slideshow, allowing
/// customization of text, color, font, and layout styles.
/// </summary>
public partial class SlideshowCaptionText : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the SlideshowCaptionText class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine how slideshow captions are managed and displayed.</param>
    public SlideshowCaptionText(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the text to display in the description area of the slideshow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the CSS color value to apply to the component's content.
    /// </summary>
    /// <remarks>If not set, the default color is "var(--colorNeutralForeground1)".</remarks>
    [Parameter]
    public string? Color { get; set; } = "var(--colorNeutralForeground1)";

    /// <summary>
    /// Gets or sets the font family to apply to the component's text.
    /// </summary>
    [Parameter]
    public TextFont Font { get; set; } = TextFont.Base;

    /// <summary>
    /// Gets or sets the font size to apply to the component's content.
    /// </summary>
    [Parameter]
    public TextSize? Size { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the text is displayed in italics.
    /// </summary>
    [Parameter]
    public bool Italic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the text is underlined.
    /// </summary>
    [Parameter]
    public bool Underline { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the text is displayed with a strikethrough.
    /// </summary>
    [Parameter]
    public bool Strikethrough { get; set; }

    /// <summary>
    /// Gets or sets the text alignment for the component content.
    /// </summary>
    [Parameter]
    public TextAlign? Align { get; set; }

    /// <summary>
    /// Gets or sets the font weight to apply to the text content, using predefined weight options.
    /// </summary>
    [Parameter]
    public TextWeight? Weight { get; set; }

    /// <summary>
    /// Gets or sets the HTML tag used to render the text content.
    /// </summary>
    [Parameter]
    public TextTag As { get; set; } = TextTag.Span;

    /// <summary>
    /// Gets or sets a value indicating whether the content should be truncated.
    /// </summary>
    [Parameter]
    public bool Truncate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the content should not wrap within its container.
    /// </summary>
    [Parameter]
    public bool Nowrap { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the text is set to block.
    /// </summary>
    [Parameter]
    public bool Block { get; set; }
}
