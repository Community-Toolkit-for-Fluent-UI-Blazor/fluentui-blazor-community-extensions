using FluentUI.Blazor.Community.Components.Extensions;
using FluentUI.Blazor.Community.Components.States;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item for the <see cref="FluentCxSlideshow{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class SlideshowItem<TItem>
    : FluentComponentBase, IDisposable
{
    private SlideshowImage<TItem>? _imageChild;
    private SlideshowContentRatio _contentRatio;
    private SlideshowRatioMode _ratioMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="SlideshowItem{TItem}"/> class.
    /// </summary>
    public SlideshowItem(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the aria label of the component.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the interval to show the image.
    /// </summary>
    [Parameter]
    public TimeSpan? Interval { get; set; }

    /// <summary>
    /// Gets or sets the parent of the component.
    /// </summary>
    [CascadingParameter]
    private FluentCxSlideshow<TItem> Parent { get; set; } = default!;

    [Inject]
    private SlideshowState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the width of the element, measured in units appropriate to the context.
    /// </summary>
    private double Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the object in units specified by the context.
    /// </summary>
    private double Height { get; set; }

    /// <summary>
    /// Gets the css of the component.
    /// </summary>
    private string? Css => DefaultClassBuilder
        .AddClass("slideshow-item-vertical", Parent?.InternalOrientation == Orientation.Vertical)
        .Build();

    /// <summary>
    /// Adds the specified child image to the slideshow.
    /// </summary>
    /// <param name="child">The child image to be added to the slideshow. This parameter cannot be null.</param>
    internal void Add(SlideshowImage<TItem> child)
    {
        _imageChild = child;
    }

    /// <summary>
    /// Removes the specified child image from the slideshow if it is currently set as the active image.
    /// </summary>
    /// <param name="child">The child image to be removed from the slideshow. If this image is the currently active image, it will be set to
    /// null.</param>
    internal void Remove(SlideshowImage<TItem> child)
    {
        if (_imageChild == child)
        {
            _imageChild = null;
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{GetType().Name} must be used within a {typeof(FluentCxSlideshow<TItem>).Name} component.");
        }

        State.SizeChanged += OnSizeChanged;
        Parent?.Add(this);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Parent is not null)
        {
            if (_contentRatio != Parent.ContentRatio)
            {
                _contentRatio = Parent.ContentRatio;
                ComputeRatioSize();
            }

            if (_ratioMode != Parent.RatioMode)
            {
                _ratioMode = Parent.RatioMode;
                ComputeRatioSize();
            }
        }
    }

    /// <summary>
    /// Calculates the optimal width and height for content to fit within a specified container while maintaining a
    /// given aspect ratio.
    /// </summary>
    /// <remarks>If the specified aspect ratio is not valid, the method defaults to using either the
    /// container's or the original content's aspect ratio, depending on the value of <paramref
    /// name="ratio"/>.</remarks>
    /// <param name="containerWidth">The width of the container in which the content will be displayed. Must be a positive value.</param>
    /// <param name="containerHeight">The height of the container in which the content will be displayed. Must be a positive value.</param>
    /// <param name="originalWidth">The original width of the content before resizing. Must be a positive value.</param>
    /// <param name="originalHeight">The original height of the content before resizing. Must be a positive value.</param>
    /// <param name="ratio">The desired aspect ratio for the content, specified as a value of the SlideshowContentRatio enumeration.</param>
    /// <param name="mode">The mode that determines how the aspect ratio is applied, specified as a value of the SlideshowRatioMode enumeration.</param>
    /// <returns>A tuple containing the calculated width and height for the content, ensuring it fits within the container
    /// dimensions and preserves the specified aspect ratio.</returns>
    private static (double width, double height) ComputeRatioSize(
        double containerWidth,
        double containerHeight,
        double originalWidth,
        double originalHeight,
        SlideshowContentRatio ratio,
        SlideshowRatioMode mode)
    {
        if (ratio == SlideshowContentRatio.FullContainer)
        {
            return (containerWidth, containerHeight);
        }

        if (ratio == SlideshowContentRatio.Original)
        {
            return (originalWidth, originalHeight);
        }

        var targetRatio = ratio.ToFormula();
        double height;
        double width;

        if (mode == SlideshowRatioMode.Image)
        {
            height = originalHeight;
            width = height * targetRatio;

            return (width, height);
        }

        height = containerHeight;
        width = height * targetRatio;

        return (width, height);
    }

    /// <summary>
    /// Handles the event triggered when the component's size changes, updating its dimensions to maintain the intended
    /// aspect ratio.
    /// </summary>
    /// <remarks>This method recalculates the component's width and height based on the new dimensions and the
    /// parent's content ratio, ensuring the component preserves its aspect ratio after resizing.</remarks>
    /// <param name="sender">The source object that initiated the size change event.</param>
    /// <param name="e">A tuple containing the new width as a string and the new height as doubles, representing the updated dimensions
    /// for the component.</param>
    private void OnSizeChanged(object? sender, string e)
    {
        if (Parent is null)
        {
            return;
        }

        ComputeRatioSize();
    }

    private void ComputeRatioSize()
    {
        if (_imageChild is null)
        {
            return;
        }

        var (width, height) = State.GetSize(_imageChild.Id);
        var (containerWidth, containerHeight) = State.GetSize(Parent!.Id);
        var (w, h) = ComputeRatioSize(containerWidth, containerHeight, width, height, _contentRatio, Parent.RatioMode);

        Width = Math.Round(w, 0, MidpointRounding.AwayFromZero);
        Height = Math.Round(h, 0, MidpointRounding.AwayFromZero);
        StateHasChanged();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        State.SizeChanged -= OnSizeChanged;
        Parent?.Remove(this);

        GC.SuppressFinalize(this);
    }

    internal void Add(SlideshowVideo<TItem> slideshowVideo)
    {
        throw new NotImplementedException();
    }

    internal void Remove(SlideshowVideo<TItem> slideshowVideo)
    {
        throw new NotImplementedException();
    }
}
