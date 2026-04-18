using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using ChartAxisLabelOptions = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisLabelOptions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for customizing the appearance and behavior of a chart axis, including visibility,
/// labeling, formatting, and rendering properties.
/// </summary>
/// <remarks>Use this class to specify axis-related settings such as whether the axis and its labels or ticks are
/// displayed, how numeric values are formatted, and how the axis is rendered visually. These options allow fine-grained
/// control over the axis presentation in chart components. Some properties, such as color, opacity, and stroke width,
/// affect the visual styling, while others, like label formatting and category labeling, influence the axis content.
/// For advanced customization, you can provide a delegate to generate category labels or configure label appearance
/// using the associated label options.</remarks>
public sealed class ChartAxisOptions
    : ComponentBase
{
    /// <summary>
    /// Value indicating whether the axis options have been modified since the last render. This flag can be used to
    /// </summary>
    private bool _hasChanged;

    /// <summary>
    /// Gets a value indicating whether the axis is displayed on the chart.
    /// </summary>
    [Parameter]
    public bool Show { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether tick marks are displayed on the slider control.
    /// </summary>
    [Parameter]
    public bool ShowTicks { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether labels are displayed alongside the associated elements.
    /// </summary>
    [Parameter]
    public bool ShowLabels { get; init; } = true;

    /// <summary>
    /// Gets the length of each tick mark, in device-independent units (DIPs).
    /// </summary>
    [Parameter]
    public double TickLength { get; init; } = 4.0;

    /// <summary>
    /// Gets the maximum number of ticks allowed for the operation.
    /// </summary>
    [Parameter]
    public int MaxTicks { get; init; } = 6;

    /// <summary>
    /// Gets the offset, in pixels, applied to the label position relative to its default location.
    /// </summary>
    [Parameter]
    public double LabelOffset { get; init; } = 12.0;

    /// <summary>
    /// Gets the standard or custom numeric format string used to format numeric values.
    /// </summary>
    /// <remarks>The format string determines how numeric values are converted to their string representation.
    /// The default value is "G", which specifies the general numeric format. For more information about valid format
    /// strings, see Standard Numeric Format Strings and Custom Numeric Format Strings in the .NET
    /// documentation.</remarks>
    [Parameter]
    public string NumericFormat { get; init; } = "G";

    /// <summary>
    /// Gets a function that returns the display label for a given category index.
    /// </summary>
    /// <remarks>The function takes an integer representing the category index and returns a string to be used
    /// as the label for that category. If not set, default labeling behavior may be used.</remarks>
    [Parameter]
    public Func<int, string>? GetCategoryLabel { get; init; }

    /// <summary>
    /// Gets the options used to configure the appearance and behavior of axis labels on the chart.
    /// </summary>
    [Parameter]
    public ChartAxisLabelOptions? LabelOptions { get; init; } = new();

    /// <summary>
    /// Gets or sets the color value in hexadecimal format.
    /// </summary>
    [Parameter]
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The opacity value determines the transparency of the component, where 1.0 is fully opaque and
    /// 0.0 is fully transparent. Values outside the range of 0.0 to 1.0 may not be supported and could result in
    /// undefined behavior.</remarks>
    [Parameter]
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the width of the stroke used to render the component.
    /// </summary>
    [Parameter]
    public double StrokeWidth { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the dash pattern used to render the outline of a shape or path.
    /// </summary>
    /// <remarks>The dash pattern is specified as a string of comma-separated numbers, where each number
    /// represents the length of dashes and gaps in the pattern. For example, "5,2" creates a pattern of a 5-unit dash
    /// followed by a 2-unit gap. If the value is null or empty, a solid line is rendered.</remarks>
    [Parameter]
    public string? DashArray { get; set; }

    /// <summary>
    /// Gets or sets the grid layer on which the component is rendered.
    /// </summary>
    /// <remarks>Use this property to control whether the component appears above or below other grid
    /// elements, such as strokes or content layers. The default value is <see
    /// cref="AxesLayerOrder.Background"/>.</remarks>
    [Parameter]
    public AxesLayerOrder Layer { get; set; } = AxesLayerOrder.Foreground;

    /// <summary>
    /// Gets or sets the parent chart component that this options instance is associated with.
    /// </summary>
    [CascadingParameter]
    private ChartOptions? Parent { get; set; }

    /// <inheritdoc/>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasChanged = parameters.TryGetValue<bool>(nameof(Show), out var newShow) && newShow != Show ||
                      parameters.TryGetValue<bool>(nameof(ShowTicks), out var newShowTicks) && newShowTicks != ShowTicks ||
                      parameters.TryGetValue<bool>(nameof(ShowLabels), out var newShowLabels) && newShowLabels != ShowLabels ||
                      parameters.TryGetValue<double>(nameof(TickLength), out var newTickLength) && newTickLength != TickLength ||
                      parameters.TryGetValue<int>(nameof(MaxTicks), out var newMaxTicks) && newMaxTicks != MaxTicks ||
                      parameters.TryGetValue<double>(nameof(LabelOffset), out var newLabelOffset) && newLabelOffset != LabelOffset ||
                      parameters.TryGetValue<string>(nameof(NumericFormat), out var newNumericFormat) && newNumericFormat != NumericFormat ||
                      parameters.TryGetValue<Func<int, string>>(nameof(GetCategoryLabel), out var newGetCategoryLabel) && newGetCategoryLabel != GetCategoryLabel ||
                      parameters.TryGetValue<ChartAxisLabelOptions?>(nameof(LabelOptions), out var newLabelOptions) && newLabelOptions != LabelOptions ||
                      parameters.TryGetValue<string>(nameof(Color), out var newColor) && newColor != Color ||
                      parameters.TryGetValue<double>(nameof(Opacity), out var newOpacity) && newOpacity != Opacity ||
                      parameters.TryGetValue<double>(nameof(StrokeWidth), out var newStrokeWidth) && newStrokeWidth != StrokeWidth ||
                      parameters.TryGetValue<string?>(nameof(DashArray), out var newDashArray) && newDashArray != DashArray ||
                      parameters.TryGetValue<AxesLayerOrder>(nameof(Layer), out var newLayer) && newLayer != Layer;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        if (_hasChanged)
        {
            Parent!.NotifyChartAxisOptionsChanged();
            _hasChanged = false;
        }

        base.OnParametersSet();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveChartAxisOptions();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("ChartBarOptions must be used within a FluentCxChart component.");
        }

        Parent.AddChartAxisOptions(this);
    }
}
