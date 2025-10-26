using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item that can be animated within a <see cref="FluentCxAnimation"/> or <see cref="AnimationGroup"/> component.
/// </summary>
public partial class AnimationItem
    : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationItem"/> class.
    /// </summary>
    public AnimationItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxAnimation"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxAnimation? Parent { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="AnimationGroup"/> component, if any.
    /// </summary>
    [CascadingParameter]
    private AnimationGroup? AnimationGroup { get; set; }

    /// <summary>
    /// Gets or sets the child content of the animation item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the background color of the animation item.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the animation item.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset value.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset value.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public double OffsetY { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the animation item.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the rotation angle in degrees.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public double Rotation { get; set; }

    /// <summary>
    /// Gets or sets the horizontal scaling factor.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public double ScaleX { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the vertical scaling factor.
    /// </summary>
    /// <remarks>This parameter is animatable.</remarks>
    [Parameter]
    public double ScaleY { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the value associated with the animation item.
    /// </summary>
    /// <remarks>This parameter is animatable. This parameter should be used for animating an item which represents a chart item.</remarks>
    [Parameter]
    public double Value { get; set; }

    /// <summary>
    /// Gets or sets the width of the animation item in pixels.
    /// </summary>
    /// <remarks>This parameter is not animatable.</remarks>
    [Parameter]
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the animation item in pixels.
    /// </summary>
    /// <remarks>This parameter is not animatable.</remarks>
    [Parameter]
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the left position of the animation item.
    /// </summary>
    /// <remarks>This parameter is not animatable.</remarks>
    [Parameter]
    public string? Left { get; set; }

    /// <summary>
    /// Gets or sets the top position of the animation item.
    /// </summary>
    /// <remarks>This parameter is not animatable.</remarks>
    [Parameter]
    public string? Top { get; set; }

    /// <summary>
    /// Gets or sets the right position of the animation item.
    /// </summary>
    /// <remarks>This parameter is not animatable.</remarks>
    [Parameter]
    public string? Right { get; set; }

    /// <summary>
    /// Gets or sets the bottom position of the animation item.
    /// </summary>
    /// <remarks>This parameter is not animatable.</remarks>
    [Parameter]
    public string? Bottom { get; set; }

    /// <summary>
    /// Gets or sets the z-index of the animation item.
    /// </summary>
    [Parameter]
    public int ZIndex { get; set; }

    /// <summary>
    /// Gets the css class for the animation item.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("animation-item")
        .Build();

    /// <summary>
    /// Gets the inline style for the animation item.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--animation-item-width", $"{Width}px", Width > 0)
        .AddStyle("--animation-item-height", $"{Height}px", Height > 0)
        .AddStyle("--animation-item-left", Left, !string.IsNullOrWhiteSpace(Left))
        .AddStyle("--animation-item-top", Top, !string.IsNullOrWhiteSpace(Top))
        .AddStyle("--animation-item-right", Right, !string.IsNullOrWhiteSpace(Right))
        .AddStyle("--animation-item-bottom", Bottom, !string.IsNullOrWhiteSpace(Bottom))
        .AddStyle("--animation-item-z-index", $"{ZIndex}", ZIndex != 0)
        .AddStyle("--animation-item-opacity", Opacity.ToString(System.Globalization.CultureInfo.InvariantCulture), true)
        .Build();

    /// <summary>
    /// Gets the <see cref="AnimatedElement"/> representation of the current state of the animation item.
    /// </summary>
    internal AnimatedElement AnimatedElement => new()
    {
        BackgroundColor = BackgroundColor,
        Color = Color,
        Id = Id!,
        OffsetX = OffsetX,
        OffsetY = OffsetY,
        Opacity = Opacity,
        Rotation = Rotation,
        ScaleX = ScaleX,
        ScaleY = ScaleY,
        Value = Value,
    };

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (AnimationGroup is not null)
        {
            AnimationGroup.AddElement(this);
        }
        else
        {
            Parent?.AddElement(this);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (AnimationGroup is not null)
        {
            AnimationGroup.RemoveElement(this);
        }
        else
        {
            Parent?.RemoveElement(this);
        }

        GC.SuppressFinalize(this);
    }
}
