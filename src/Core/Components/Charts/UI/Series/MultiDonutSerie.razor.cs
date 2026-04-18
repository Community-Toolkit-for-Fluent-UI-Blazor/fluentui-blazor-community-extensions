using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a data series for a multi-donut chart visualization.
/// </summary>
/// <remarks>Use this class to define and configure a series of data to be displayed in a multi-donut chart
/// component. This type is typically used in conjunction with charting controls that support multiple concentric donut
/// series.</remarks>
public sealed partial class MultiDonutSerie : SerieBase
{
    /// <summary>
    /// Represents the collection of individual donut series that make up the multi-donut chart. Each item in this list corresponds to a separate donut chart within the overall visualization, allowing for complex data representations with multiple layers of information.
    /// </summary>
    private readonly List<DonutSerie> _series = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiDonutSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public MultiDonutSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.MultiDonut;

    /// <summary>
    /// Gets or sets a value indicating whether to use alternate animation for the multi-donut series.
    /// </summary>
    [Parameter]
    public bool? AlternateAnimation { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    /// <remarks>Use this property to specify the child elements or markup that should appear within the
    /// component. Typically set in Razor markup using child content syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the palette mode for the multi-donut series.
    /// </summary>
    [Parameter]
    public DonutPaletteMode PaletteMode { get; set; } = DonutPaletteMode.ByCategory;

    /// <summary>
    /// Removes the specified donut series from the collection.
    /// </summary>
    /// <param name="value">The donut series to remove from the collection. Cannot be null.</param>
    internal void Remove(DonutSerie value)
    {
        _series.Remove(value);
    }

    /// <summary>
    /// Adds the specified donut series to the collection.
    /// </summary>
    /// <param name="value">The donut series to add to the collection. Cannot be null.</param>
    internal void Add(DonutSerie value)
    {
        _series.Add(value);
    }

    /// <inheritdoc />
    protected internal override Charts.Series.ChartSerie Create()
    {
        return new Charts.Series.MultiDonutSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            PaletteMode = PaletteMode,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            AlternateAnimation = AlternateAnimation,
            Series = [.. _series.Select(s => s.Create()).Cast<Charts.Series.DonutSerie>()]
        };
    }
}
