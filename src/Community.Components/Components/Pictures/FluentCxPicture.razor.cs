using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a picture component with various customization options.
/// </summary>
public partial class FluentCxPicture : FluentComponentBase
{
    /// <summary>
    /// Represents the internal list of responsive image sources.
    /// </summary>
    private readonly List<ResponsiveImage> _internalResponsiveSources = new();

    /// <summary>
    /// Gets or sets the source URL of the image.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the source set for responsive images.
    /// </summary>
    [Parameter]
    public string? SourceSet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to preload the image.
    /// </summary>
    [Parameter]
    public bool Preload { get; set; }

    /// <summary>
    /// Gets or sets the loading behavior of the image.
    /// </summary>
    [Parameter]
    public LoadingBehavior Loading { get; set; } = LoadingBehavior.Auto;

    /// <summary>
    /// Gets or sets the fetch priority of the image.
    /// </summary>
    [Parameter]
    public FetchPriority FetchPriority { get; set; } = FetchPriority.Auto;

    /// <summary>
    /// Gets or sets the image format (e.g., "webp", "jpeg").
    /// </summary>
    /// <remarks>This is used for the image processing middleware.</remarks>
    [Parameter]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the quality of the image (e.g., "80" for 80% quality).
    /// </summary>
    [Parameter]
    public string? Quality { get; set; }

    /// <summary>
    /// Gets or sets the CSS filter effects to apply to the image (e.g., "grayscale(100%)", "blur(5px)").
    /// </summary>
    [Parameter]
    public string? Effects { get; set; }

    /// <summary>
    /// Gets or sets the shadow effect for the image.
    /// </summary>
    [Parameter]
    public Shadow? Shadow { get; set; }

    /// <summary>
    /// Gets or sets the width of the image.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the image.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the border radius type of the image.
    /// </summary>
    [Parameter]
    public BorderRadius BorderRadius { get; set; } = BorderRadius.None;

    /// <summary>
    /// Gets or sets the custom border radius value when <see cref="BorderRadius"/> is set to <see cref="BorderRadius.Custom"/>.
    /// </summary>
    [Parameter]
    public string? BorderRadiusValue { get; set; }

    /// <summary>
    /// Gets or sets the border style of the image.
    /// </summary>
    [Parameter]
    public BorderStyle BorderStyle { get; set; } = BorderStyle.None;

    /// <summary>
    /// Gets or sets the border width of the image.
    /// </summary>
    [Parameter]
    public string? BorderWidth { get; set; }

    /// <summary>
    /// Gets or sets the border color of the image.
    /// </summary>
    [Parameter]
    public string? BorderColor { get; set; }

    /// <summary>
    /// Gets or sets the overlay content to be displayed over the picture.
    /// </summary>
    [Parameter]
    public RenderFragment? OverlayContent { get; set; }

    /// <summary>
    /// Gets or sets how the image should be resized to fit its container.
    /// </summary>
    [Parameter]
    public ObjectFit ObjectFit { get; set; } = ObjectFit.Cover;

    /// <summary>
    /// Gets or sets the alt text for the image.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; }

    /// <summary>
    /// Gets or sets the title for the image.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the caption for the image.
    /// </summary>
    [Parameter]
    public string? Caption { get; set; }

    /// <summary>
    /// Gets or sets the language of the image for accessibility purposes.
    /// </summary>
    [Parameter]
    public string? Language { get; set; }

    /// <summary>
    /// Gets or sets the responsive content to be rendered within the picture element.
    /// </summary>
    [Parameter]
    public RenderFragment? ResponsiveContent { get; set; }

    /// <summary>
    /// Gets the internal CSS class for the picture component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("fluentcx-image-picture")
        .Build();

    /// <summary>
    /// Gets the internal style for the picture component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("border", $"{BorderWidth} {BorderStyle.ToAttributeValue()} {BorderColor}", BorderStyle != BorderStyle.None)
        .AddStyle("border-radius", BorderRadius == BorderRadius.Circle ? "50%" : BorderRadius == BorderRadius.RoundSquare ? "10px" : BorderRadius == BorderRadius.Custom && !string.IsNullOrEmpty(BorderRadiusValue) ? BorderRadiusValue : null, BorderRadius != BorderRadius.None)
        .AddStyle("box-shadow", Shadow?.ToCss(), Shadow is not null)
        .AddStyle("filter", Effects, !string.IsNullOrEmpty(Effects))
        .AddStyle("object-fit", ObjectFit.ToAttributeValue(), ObjectFit != ObjectFit.None)
        .Build();

    /// <summary>
    /// Adds a responsive image source to the internal list.
    /// </summary>
    /// <param name="source">Source to add.</param>
    internal void AddSource(ResponsiveImage source)
    {
        if (!_internalResponsiveSources.Contains(source))
        {
            _internalResponsiveSources.Add(source);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Removes a responsive image source from the internal list.
    /// </summary>
    /// <param name="source">Source to remove.</param>
    internal void RemoveSource(ResponsiveImage source)
    {
        _internalResponsiveSources.Remove(source);
        StateHasChanged();
    }
}
