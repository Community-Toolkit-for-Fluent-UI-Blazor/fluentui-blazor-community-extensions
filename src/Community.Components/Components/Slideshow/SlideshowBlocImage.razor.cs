using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an image component for use within a slideshow, supporting customizable layout, appearance, and content.
/// </summary>
/// <remarks>This component allows you to display an image with configurable properties such as alternative text,
/// source URL, title, background color, dimensions, border radius, and image position. Additional content can be
/// provided using the ChildContent parameter. The component is designed for use in Blazor applications and can be
/// integrated into a larger slideshow or carousel UI.</remarks>
/// <typeparam name="TItem">The type of the data item associated with the slideshow image.</typeparam>
public partial class SlideshowBlocImage<TItem> : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the alternative text to display when the image cannot be rendered.
    /// </summary>
    /// <remarks>Alternative text is important for accessibility, as it is read by screen readers and
    /// displayed if the image fails to load. Provide a descriptive value to improve the user experience for all
    /// users.</remarks>
    [Parameter]
    public string? AltText { get; set; }

    /// <summary>
    /// Gets or sets the source identifier or URI for the component's data or content.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the title to display for the component.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the background color of the component.
    /// </summary>
    /// <remarks>Set this property to specify a custom background color using a valid CSS color value, such as
    /// a color name, hexadecimal, RGB, or other supported CSS formats. If not set, the component uses its default
    /// background color.</remarks>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the width, in pixels, of the image to be rendered.
    /// </summary>
    [Parameter]
    public int? ImageWidth { get; set; }

    /// <summary>
    /// Gets or sets the height, in pixels, of the image to be rendered.
    /// </summary>
    [Parameter]
    public int? ImageHeight { get; set; }

    /// <summary>
    /// Gets or sets the position of the image within the slideshow component.
    /// </summary>
    /// <remarks>Use this property to control where the image appears relative to other content in the
    /// slideshow. The default value is SlideshowImagePosition.Left.</remarks>
    [Parameter]
    public SlideshowImagePosition ImagePosition { get; set; } = SlideshowImagePosition.Left;

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    /// <remarks>Use this property to specify the child elements or markup that will be rendered within the
    /// component. Typically set implicitly by placing content between the component's opening and closing tags in Razor
    /// syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the border radius, in pixels, to apply to the component's corners.
    /// </summary>
    [Parameter]
    public int? BorderRadius { get; set; }

    /// <summary>
    /// Gets or sets the CSS border-radius value to apply to the image.
    /// </summary>
    /// <remarks>Set this property to control the roundness of the image's corners using standard CSS
    /// border-radius syntax (e.g., "8px", "50%", or "1em"). If not set, the image will use the default border radius
    /// defined by the component or stylesheet.</remarks>
    [Parameter]
    public string? ImageBorderRadius { get; set; }

    /// <summary>
    /// Gets the computed CSS style string based on the current style settings.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("background-color", BackgroundColor, !string.IsNullOrWhiteSpace(BackgroundColor))
        .AddStyle("border-radius", $"{BorderRadius}px", BorderRadius.HasValue)
        .Build();

    /// <summary>
    /// Gets the inline CSS style string for the image based on the current image settings.
    /// </summary>
    private string? InternalImageStyle => new StyleBuilder()
        .AddStyle("width", $"{ImageWidth}px", ImageWidth.HasValue)
        .AddStyle("height", $"{ImageHeight}px", ImageHeight.HasValue)
        .AddStyle("border-radius", ImageBorderRadius, !string.IsNullOrEmpty(ImageBorderRadius))
        .Build();
}
