using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a pie component to render a pie chart series.
/// </summary>
public sealed class PieSerie : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PieSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public PieSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Pie;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<RadialSlice> Items { get; init; } = [];

    /// <summary>
    /// Gets or sets the configuration options for the bar series.
    /// </summary>
    [Parameter]
    public RadialSerieOptions? Options { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to use alternate animation for the multi-donut series.
    /// </summary>
    [Parameter]
    public bool AlternateAnimation { get; set; }

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var pieSerie = new Charts.Series.PieSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            AlternateAnimation = AlternateAnimation,
            Options = Options
        };

        pieSerie.UpdateItems(Items);

        return pieSerie;
    }
}
