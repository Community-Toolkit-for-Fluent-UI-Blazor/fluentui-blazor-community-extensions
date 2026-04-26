using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a semi-donut series component.
/// </summary>
public sealed class SemiDonutSerieOptions : ComponentBase, IDisposable
{
    /// <summary>
    /// Value indicating whether the <see cref="StartAngle"/> or <see cref="EndAngle"/> properties have changed since the last render.
    /// </summary>
    private bool _hasAngleChanged;

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

    /// <summary>
    /// Gets or sets the parent chart component that this options instance is associated with.
    /// </summary>
    [CascadingParameter]
    private ChartOptions? Parent { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveSemiDonutSerieOptions();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("SemiDonutSerieOptions must be used within a FluentCxChart component.");
        }

        Parent.AddSemiDonutSerieOptions(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasAngleChanged = parameters.TryGetValue<double>(nameof(StartAngle), out var newStartAngle) && newStartAngle != StartAngle
            || parameters.TryGetValue<double>(nameof(EndAngle), out var newEndAngle) && newEndAngle != EndAngle;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        if (_hasAngleChanged)
        {
            _hasAngleChanged = false;

            Parent?.NotifySemiDonutSerieOptionsChanged();
        }

        return base.OnParametersSetAsync();
    }
}
