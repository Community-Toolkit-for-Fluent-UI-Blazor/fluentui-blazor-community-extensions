using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a semi-donut component to render a semi-donut chart series.
/// </summary>
public sealed class SemiDonutSerie : DonutSerie
{
    /// <summary>
    /// Represents a flag indicating whether the properties of the semi-donut series have changed.
    /// </summary>
    private bool _hasChanged;

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

    /// <summary>
    /// Gets or sets the start angle of the semi-donut chart series.
    /// </summary>
    [Parameter]
    public double StartAngle { get; set; }

    /// <summary>
    /// Gets or sets the end angle of the semi-donut chart series.
    /// </summary>
    [Parameter]
    public double EndAngle { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasChanged)
        {
            _hasChanged = false;
            Options ??= new Charts.Options.RadialSerieOptions();
            Options.StartAngle = StartAngle;
            Options.EndAngle = EndAngle;
            Refresh();
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasChanged = parameters.TryGetValue<double>(nameof(StartAngle), out var newAngle) && newAngle != StartAngle ||
                      parameters.TryGetValue<double>(nameof(EndAngle), out var endAngle) && endAngle != EndAngle;

        return base.SetParametersAsync(parameters);
    }
}
