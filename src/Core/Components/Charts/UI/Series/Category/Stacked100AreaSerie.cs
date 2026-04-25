using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a line series component for displaying category data in a Fluent UI Blazor chart.
/// </summary>
/// <remarks>Use this class to configure and render a line series within a category-based chart. The series
/// displays data points connected by lines, with customizable options and data items. This component is intended for
/// use with the Fluent UI Blazor library and follows its configuration patterns.</remarks>
public sealed class Stacked100AreaSerie
    : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Stacked100AreaSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public Stacked100AreaSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Stacked100Area;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<CategoryItem> Items { get; init; } = [];

    /// <summary>
    /// Gets or sets the configuration options for the area series.
    /// </summary>
    [Parameter]
    public CategoryLineOptions? Options { get; init; }

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var cls = new Charts.Series.LineSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            Options = Options,
            LineType = LineChartType.Stacked100Area
        };

        cls.UpdateItems(Items);

        return cls;
    }
}
