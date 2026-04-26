using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a radar series component.
/// </summary>
public class RadarSerieOptions : ComponentBase, IDisposable
{
    /// <summary>
    /// Value indicating whether the <see cref="FillArea"/> property has changed since the last render.
    /// </summary>
    private bool _hasFillAreaChanged;

    /// <summary>
    /// Gets or sets a value indicating whether the radar series area should be filled.
    /// </summary>
    [Parameter]
    public bool FillArea { get; set; }

    /// <summary>
    /// Gets or sets the parent chart component that this options instance is associated with.
    /// </summary>
    [CascadingParameter]
    private ChartOptions? Parent { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveRadarSerieOptions();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("RadarSerieOptions must be used within a FluentCxChart component.");
        }

        Parent.AddRadarSerieOptions(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasFillAreaChanged = parameters.TryGetValue<bool>(nameof(FillArea), out var newFillArea) && newFillArea != FillArea;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        if (_hasFillAreaChanged)
        {
            _hasFillAreaChanged = false;

            Parent?.NotifyRadarSerieOptionsChanged();
        }

        return base.OnParametersSetAsync();
    }
}
