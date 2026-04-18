using FluentUI.Blazor.Community.Components.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a set of animation strategies used to control the behavior of chart animations for various chart
/// interactions.
/// </summary>
/// <remarks>Use this record to configure how different chart events, such as initial appearance, updates,
/// removals, hover, and selection, are animated. The strategies defined in this record allow for consistent and
/// centralized management of animation behaviors across chart components. All properties are immutable and should be
/// set during initialization.</remarks>
public sealed record ChartStrategiesOverride
{
    /// <summary>
    /// Gets a value indicating whether animations are enabled for the component.
    /// </summary>
    public bool? AnimationsEnabled { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when the chart initially appears.
    /// </summary>
    public ChartAnimationStrategy? InitialAppear { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when updating the chart.
    /// </summary>
    /// <remarks>Use this property to specify how chart updates are animated. The selected strategy determines
    /// the visual transition applied when the chart data changes.</remarks>
    public ChartAnimationStrategy? Update { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when removing chart elements.
    /// </summary>
    /// <remarks>Use this property to specify how chart elements are animated when they are removed from the
    /// chart. The default value disables removal animations.</remarks>
    public ChartAnimationStrategy? Remove { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when a chart element is hovered over.
    /// </summary>
    /// <remarks>Use this property to customize the visual effect applied when the user hovers over chart
    /// elements. The default value provides a standard hover animation, but you can specify a different strategy to
    /// achieve custom behavior.</remarks>
    public ChartAnimationStrategy? HoverIn { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when the pointer leaves a chart element.
    /// </summary>
    public ChartAnimationStrategy? HoverOut { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when selecting chart elements.
    /// </summary>
    /// <remarks>Use this property to specify how chart elements should animate when they are selected. The
    /// default value provides standard selection animation behavior.</remarks>
    public ChartAnimationStrategy? SelectIn { get; init; }

    /// <summary>
    /// Gets the animation strategy to use when deselecting chart elements.
    /// </summary>
    public ChartAnimationStrategy? SelectOut { get; init; }
}
