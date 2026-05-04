using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a polar area chart series component that can be used to visualize data in a polar area chart.
/// </summary>
public sealed class PolarAreaSerie : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolarAreaSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public PolarAreaSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.PolarArea;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<PolarItem> Items { get; set; } = [];

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var polarSerie = new PolarSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            PolarType = PolarChartType.Area
        };

        polarSerie.UpdateItems(Items);

        return polarSerie;
    }
}
