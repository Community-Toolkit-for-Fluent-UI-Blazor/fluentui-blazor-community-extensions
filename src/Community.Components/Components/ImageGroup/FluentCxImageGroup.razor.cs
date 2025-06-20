using System.Globalization;
using System.Runtime.CompilerServices;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a group of images.
/// </summary>
public partial class FluentCxImageGroup
    : FluentComponentBase
{
    /// <summary>
    /// Represents the images inside this component.
    /// </summary>
    private readonly List<FluentCxImage> _children = [];

    /// <summary>
    /// Represents a value if the popover is opened.
    /// </summary>
    private bool _isPopoverOpen;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxImageGroup"/> component.
    /// </summary>
    public FluentCxImageGroup() : base()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the number of visible items in the group.
    /// </summary>
    [Parameter]
    public int MaxVisibleItems { get; set; }

    /// <summary>
    /// Gets or sets the size of each image.
    /// </summary>
    [Parameter]
    public ImageSize Size { get; set; } = ImageSize.Size40;

    /// <summary>
    /// Gets or sets the shape of each image.
    /// </summary>
    [Parameter]
    public ImageShape Shape { get; set; } = ImageShape.RoundSquare;

    /// <summary>
    /// Gets or sets the layout of the group.
    /// </summary>
    /// <remarks>
    /// By default, the layout is set to <see cref="ImageGroupLayout.Spread"/>.
    /// </remarks>
    [Parameter]
    public ImageGroupLayout GroupType { get; set; } = ImageGroupLayout.Spread;

    /// <summary>
    /// Gets the number of visible items.
    /// </summary>
    private int VisibleCount => MaxVisibleItems < _children.Count ? MaxVisibleItems : _children.Count;

    /// <summary>
    /// Add an image into the group.
    /// </summary>
    /// <param name="image">Image to add.</param>
    internal void Add(FluentCxImage image)
    {
        image.SetGroupSize((int)Size);
        _children.Add(image);
    }

    /// <summary>
    /// Remove an image from the group.
    /// </summary>
    /// <param name="image"></param>
    internal void Remove(FluentCxImage image)
    {
        _children.Remove(image);
    }

    /// <summary>
    /// Gets the border radius of the image from its shape.
    /// </summary>
    /// <returns>Returns the radius of the image from its shape.</returns>
    internal string GetBorderRadius()
    {
        return Shape switch
        {
            ImageShape.Square => "0px",
            ImageShape.RoundSquare => "8px",
            _ => "10000px"
        };
    }

    /// <summary>
    /// Gets the margin left for the spread layout.
    /// </summary>
    /// <param name="image">Image where the margin left will be set.</param>
    /// <returns>Returns the margin left.</returns>
    internal int GetSpreadMarginLeft(FluentCxImage image)
    {
        return _children.IndexOf(image) <= 0 ? 0 : 16;
    }

    /// <summary>
    /// Gets the margin left for the stack layout.
    /// </summary>
    /// <param name="image">Image where the margin left will be set.</param>
    /// <returns>Returns the margin left.</returns>
    internal string GetStackMarginLeft(FluentCxImage image)
    {
        return _children.IndexOf(image) <= 0 ? "0" : (-6 * ((int)Size / 16f)).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets a value indicating if the current image is inside the popover.
    /// </summary>
    /// <param name="image">Image to check if it is inside a popover.</param>
    /// <returns>Returns <see langword="true" /> if the image is inside a popover, <see langword="false" /> otherwise.</returns>
    internal bool IsInPopover(FluentCxImage image)
    {
        return _children.IndexOf(image) >= MaxVisibleItems;
    }

    /// <summary>
    /// Gets the style of the popover button.
    /// </summary>
    /// <returns>Returns the style of the button.</returns>
    private string GetButtonStyle()
    {
        DefaultInterpolatedStringHandler handler = new();
        var size = (int)Size;

        if (GroupType == ImageGroupLayout.Spread)
        {
            handler.AppendLiteral("margin-left: 16px;");
        }
        else
        {
            handler.AppendLiteral("margin-left: ");
            handler.AppendFormatted(-6 * (size / 16f));
            handler.AppendLiteral("px;");
        }

        handler.AppendLiteral("border-radius: ");
        handler.AppendFormatted(GetBorderRadius());
        handler.AppendLiteral(";");

        handler.AppendLiteral("width: ");
        handler.AppendFormatted(size);
        handler.AppendLiteral("px; height: ");
        handler.AppendFormatted(size);
        handler.AppendLiteral("px;");

        return handler.ToString();
    }

    /// <summary>
    /// Occurs when a parameter of a <see cref="FluentCxImage"/> changed.
    /// </summary>
    /// <param name="image">Image which the parameters have changed.</param>
    protected internal virtual void OnItemParemetersChanged(FluentCxImage image)
    {
        StateHasChanged();
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Size), Size))
        {
            foreach (var item in _children)
            {
                item.SetGroupSize((int)Size);
            }
        }
    }
}
