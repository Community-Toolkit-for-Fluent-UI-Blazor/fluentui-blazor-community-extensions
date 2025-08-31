using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents settings for the signature tool.
/// </summary>
public class SignatureSettings
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the current tool being used in the signature component.
    /// </summary>
    [Parameter]
    public SignatureTool Tool { get; set; } = SignatureTool.Pen;

    /// <summary>
    /// Gets or sets the width of the stroke used for drawing the signature.
    /// </summary>
    [Parameter]
    public float StrokeWidth { get; set; } = 2.0f;

    /// <summary>
    /// Gets or sets the color of the stroke used for drawing the signature.
    /// </summary>
    [Parameter]
    public string StrokeColor { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the color of the pen used for drawing the signature.
    /// </summary>
    [Parameter]
    public string PenColor { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the color of the shadow used for drawing the signature.
    /// </summary>
    [Parameter]
    public string ShadowColor { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity of the shadow used for drawing the signature.
    /// Value should be between 0.0 (fully transparent) and 1.0 (fully opaque).
    /// </summary>
    [Parameter]
    public float ShadowOpacity { get; set; } = 0.6f;

    /// <summary>
    /// Gets or sets the opacity of the pen used for drawing the signature.
    /// Value should be between 0.0 (fully transparent) and 1.0 (fully opaque).
    /// </summary>
    [Parameter]
    public float PenOpacity { get; set; } = 1.0f;

    /// <summary>
    /// Gets or sets the style of the stroke used for drawing the signature.
    /// </summary>
    [Parameter]
    public SignatureLineStyle StrokeStyle { get; set; } = SignatureLineStyle.Solid;

    /// <summary>
    /// Gets or sets a value indicating whether the stroke should be smoothed for a more natural appearance.
    /// </summary>
    [Parameter]
    public bool Smooth { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether a shadow effect should be applied to the stroke.
    /// </summary>
    [Parameter]
    public bool UseShadow { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether pointer pressure sensitivity should be used when drawing the signature.
    /// </summary>
    [Parameter]
    public bool UsePointerPressure { get; set; }

    /// <summary>
    /// Gets or sets the background color of the signature area.
    /// </summary>
    [Parameter]
    public string BackgroundColor { get; set; } = "#FFFFFF";

    /// <summary>
    /// Gets or sets the background image of the signature area as a byte array.
    /// This can be used to set a custom background for the signature pad.
    /// </summary>
    /// <remarks>
    /// If <see cref="BackgroundImage"/> is set, it will override the <see cref="BackgroundColor"/>."/>
    /// </remarks>
    [Parameter]
    public byte[]? BackgroundImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a separator line should be displayed in the signature area.
    /// </summary>
    [Parameter]
    public bool ShowSeparatorLine { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether a separator line should be displayed in the signature area.
    /// </summary>
    [Parameter]
    public string SeparatorLineColor{ get; set; } = "#808080";

    /// <summary>
    /// Gets or sets the color of the separator line in the signature area.
    /// </summary>
    [Parameter]
    public float SeparatorY { get; set; } = 0.6f;

    /// <summary>
    /// Gets or sets the color of the separator line in the signature area.
    /// </summary>
    [Parameter]
    public bool ShowGrid { get; set; }

    /// <summary>
    /// Gets or sets the type of grid to be displayed in the signature area.
    /// </summary>
    [Parameter]
    public SignatureGridType GridType { get; set; } = SignatureGridType.Lines;

    /// <summary>
    /// Gets or sets the spacing between grid lines in the signature area, in pixels.
    /// </summary>
    [Parameter]
    public int GridSpacing { get; set; } = 24;

    /// <summary>
    /// Gets or sets the color of the grid lines in the signature area.
    /// </summary>
    [Parameter]
    public string GridColor { get; set; } = "#e0e0e0";

    /// <summary>
    /// Gets or sets the opacity of the grid lines in the signature area. Value should be between 0.0 (fully transparent) and 1.0 (fully opaque).
    /// </summary>
    [Parameter]
    public float GridOpacity { get; set; } = 0.8f;

    /// <summary>
    /// Gets or sets the parent signature component.
    /// </summary>
    [CascadingParameter]
    private FluentCxSignature? Parent { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        SeparatorY = Math.Clamp(SeparatorY, 0.0f, 1.0f);
        PenOpacity = Math.Clamp(PenOpacity, 0.05f, 1.0f);
        GridOpacity = Math.Clamp(GridOpacity, 0.05f, 1.0f);

        base.OnParametersSet();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SignatureSettings)} must be used within a {nameof(FluentCxSignature)} component.");
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.HasValueChanged(nameof(Tool), Tool) ||
            parameters.HasValueChanged(nameof(StrokeWidth), StrokeWidth) ||
            parameters.HasValueChanged(nameof(StrokeColor), StrokeColor) ||
            parameters.HasValueChanged(nameof(PenColor), PenColor) ||
            parameters.HasValueChanged(nameof(ShadowColor), ShadowColor) ||
            parameters.HasValueChanged(nameof(PenOpacity), PenOpacity) ||
            parameters.HasValueChanged(nameof(StrokeStyle), StrokeStyle) ||
            parameters.HasValueChanged(nameof(Smooth), Smooth) ||
            parameters.HasValueChanged(nameof(UseShadow), UseShadow) ||
            parameters.HasValueChanged(nameof(UsePointerPressure), UsePointerPressure) ||
            parameters.HasValueChanged(nameof(BackgroundColor), BackgroundColor) ||
            parameters.HasValueChanged(nameof(BackgroundImage), BackgroundImage) ||
            parameters.HasValueChanged(nameof(ShowSeparatorLine), ShowSeparatorLine) ||
            parameters.HasValueChanged(nameof(SeparatorLineColor), SeparatorLineColor) ||
            parameters.HasValueChanged(nameof(SeparatorY), SeparatorY) ||
            parameters.HasValueChanged(nameof(ShowGrid), ShowGrid) ||
            parameters.HasValueChanged(nameof(GridType), GridType) ||
            parameters.HasValueChanged(nameof(GridSpacing), GridSpacing) ||
            parameters.HasValueChanged(nameof(GridColor), GridColor) ||
            parameters.HasValueChanged(nameof(GridOpacity), GridOpacity))
        {
            Parent?.InvalidateRender();
        }

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Updates the internal values of the signature settings and triggers a re-render of the parent component.
    /// </summary>
    /// <param name="strokeWidth">Width of the stroke.</param>
    /// <param name="penColor">Color of the pen.</param>
    /// <param name="penOpacity">Opacity of the pen.</param>
    /// <param name="strokeStyle">Style of the stroke.</param>
    /// <param name="showSeparatorLine">Show the separator line.</param>
    /// <param name="useSmooth">True to use smooth, false otherwise.</param>
    /// <param name="usePointerPressure">Use the pointer pressure.</param>
    /// <param name="useShadow">Use the shadow.</param>
    /// <param name="shadowOpacity">Opacity of the shadow.</param>
    /// <param name="shadowColor">Color of the shadow.</param>
    /// <param name="showGrid">Shows the grid.</param>
    internal void UpdateInternalValues(
        float strokeWidth,
        string penColor,
        float penOpacity,
        SignatureLineStyle strokeStyle,
        bool showSeparatorLine,
        bool useSmooth,
        bool usePointerPressure,
        bool useShadow,
        float shadowOpacity,
        string shadowColor,
        bool showGrid)
    {
        StrokeWidth = strokeWidth;
        PenColor = penColor;
        PenOpacity = penOpacity;
        StrokeStyle = strokeStyle;
        ShowSeparatorLine = showSeparatorLine;
        Smooth = useSmooth;
        UsePointerPressure = usePointerPressure;
        UseShadow = useShadow;
        ShadowOpacity = shadowOpacity;
        ShadowColor = shadowColor;
        ShowGrid = showGrid;
        Parent?.InvalidateRender();
    }
}
