using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents the base class for all chart series.
/// </summary>
public abstract class ChartSerie
{
    /// <summary>
    /// Gets the unique identifier for the instance.
    /// </summary>
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    public required string? Name { get; set; }

    /// <summary>
    /// Gets or sets an arbitrary object value that can be used to associate custom data with this instance.
    /// </summary>
    public object? Tag { get; set; }

    /// <summary>
    /// Gets a value indicating whether the component is visible.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets the type of chart represented by the current instance.
    /// </summary>
    public abstract ChartType ChartType { get; }

    /// <summary>
    /// Gets the style settings applied to the chart series.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the chart series, such as colors, line
    /// styles, or markers. If not set, the default style for the chart series is used.</remarks>
    public ChartItemStyle? Style { get; set; }

    /// <summary>
    /// Gets the interaction behavior for the chart series, such as how user input is handled.
    /// </summary>
    /// <remarks>Set this property to specify how the chart series responds to user interactions, such as
    /// selection, highlighting, or tooltips. If not set, the default interaction behavior is used.</remarks>
    public ChartSerieInteraction? Interaction { get; set; }

    /// <summary>
    /// Gets the animation settings applied to the chart item.
    /// </summary>
    /// <remarks>Use this property to configure how the chart item animates when it is rendered or updated. If
    /// not set, the chart item will use the default animation behavior defined by the chart component.</remarks>
    public ChartAnimationOptions? Animation { get; set; }

    /// <summary>
    /// Gets a value indicating whether animation is enabled for the chart series.
    /// </summary>
    public bool AnimationEnabled { get; set; }

    /// <summary>
    /// Gets the collection of items that belong to this chart series.
    /// </summary>
    protected internal abstract int ItemsCount { get; }

    /// <summary>
    /// Gets a collection of numeric values represented by the current instance.
    /// </summary>
    protected internal abstract IEnumerable<double> Values { get; }

    /// <summary>
    /// Gets the collection of raw items.
    /// </summary>
    internal abstract IReadOnlyList<ChartItem> RawItems { get; }
}
