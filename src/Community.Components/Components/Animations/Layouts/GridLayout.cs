using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a grid with a fixed number of columns and uniform cell dimensions.
/// </summary>
/// <remarks>The <see cref="GridLayout"/> class organizes elements into rows and columns based on the specified
/// number of columns  and the dimensions of each cell. The layout automatically calculates the position of each element
/// within the grid  and animates their transitions when the layout changes.  This layout is particularly useful for
/// scenarios where elements need to be displayed in a structured grid format,  such as image galleries, dashboards, or
/// data visualizations.</remarks>
public sealed class GridLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the number of columns in the grid. Must be at least 1.
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 4;

    /// <summary>
    /// Gets or sets the width of each cell in the grid.
    /// </summary>
    [Parameter]
    public double CellWidth { get; set; } = 100;

    /// <summary>
    /// Gets or sets the height of each cell in the grid.
    /// </summary>
    [Parameter]
    public double CellHeight { get; set; } = 100;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var row = index / Columns;
        var col = index % Columns;
        var x = col * CellWidth;
        var y = row * CellHeight;

        animatedElement.OffsetXState = CreateState(x);
        animatedElement.OffsetYState = CreateState(y);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.TryGetValue(nameof(Width), out double w))
        {
            Columns = Math.Max(1, (int)(w / CellWidth));
        }

        return base.SetParametersAsync(parameters);
    }
}
