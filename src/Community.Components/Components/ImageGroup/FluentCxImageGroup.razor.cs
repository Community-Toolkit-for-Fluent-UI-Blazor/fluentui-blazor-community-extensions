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
    private readonly List<FluentCxImageGroupItem> _children = [];

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
    [Parameter, EditorRequired]
    public int MaxVisibleItems { get; set; } = 1;

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
    /// Gets or sets the border style of each image.
    /// </summary>
    /// <returns></returns>
    [Parameter]
    public string? BorderStyle { get; set; }

    /// <summary>
    /// Gets or sets the background style of each image.
    /// </summary>
    [Parameter]
    public string? BackgroundStyle { get; set; }

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
    /// <param name="imageGroupItem">Image to add.</param>
    internal void Add(FluentCxImageGroupItem imageGroupItem)
    {
        imageGroupItem.SetGroupSize((int)Size);
        _children.Add(imageGroupItem);
    }

    /// <summary>
    /// Remove an image from the group.
    /// </summary>
    /// <param name="imageGroupItem"></param>
    internal void Remove(FluentCxImageGroupItem imageGroupItem)
    {
        _children.Remove(imageGroupItem);
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
            ImageShape.Circle => "100000px",
            _ => throw new InvalidOperationException("Invalid image shape.")
        };
    }

    /// <summary>
    /// Gets the margin left for the spread layout.
    /// </summary>
    /// <param name="imageGroupItem">Image where the margin left will be set.</param>
    /// <returns>Returns the margin left.</returns>
    internal int GetSpreadMarginLeft(FluentCxImageGroupItem imageGroupItem)
    {
        return _children.IndexOf(imageGroupItem) <= 0 ? 0 : 16;
    }

    /// <summary>
    /// Gets the margin left for the stack layout.
    /// </summary>
    /// <param name="imageGroupItem">Image where the margin left will be set.</param>
    /// <returns>Returns the margin left.</returns>
    internal string GetStackMarginLeft(FluentCxImageGroupItem imageGroupItem)
    {
        return _children.IndexOf(imageGroupItem) <= 0 ? "0" : (-6 * ((int)Size / 16f)).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets a value indicating if the current image is inside the popover.
    /// </summary>
    /// <param name="imageGroupItem">Image to check if it is inside a popover.</param>
    /// <returns>Returns <see langword="true" /> if the image is inside a popover, <see langword="false" /> otherwise.</returns>
    internal bool IsInPopover(FluentCxImageGroupItem imageGroupItem)
    {
        return _children.IndexOf(imageGroupItem) >= MaxVisibleItems;
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
    /// Occurs when a parameter of a <see cref="FluentCxImageGroupItem"/> changed.
    /// </summary>
    /// <param name="imageGroupItem">Image which the parameters have changed.</param>
    protected internal virtual void OnItemParemetersChanged(FluentCxImageGroupItem imageGroupItem)
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
