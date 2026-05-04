using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a semi-donut component to render a semi-donut chart series.
/// </summary>
public sealed class SemiDonutSerie : DonutSerie
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SemiDonutSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public SemiDonutSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.SemiDonut;

    /// <inheritdoc />
    protected internal override Charts.Series.ChartSerie Create()
    {
        var semiDonutSerie = new Charts.Series.SemiDonutSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            AlternateAnimation = AlternateAnimation
        };

        semiDonutSerie.UpdateItems(Items);

        return semiDonutSerie;
    }
}
