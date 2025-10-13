using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a snake-like pattern,  typically used for animated transitions in UI
/// components.
/// </summary>
/// <remarks>This layout is designed to provide a visually dynamic arrangement of elements,  where items are
/// positioned in a winding or serpentine sequence. It is particularly  useful for creating engaging animations or
/// unique visual effects in user interfaces.</remarks>
public sealed class SnakeLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the number of columns in the snake layout.
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 5;

    /// <summary>
    /// Gets or sets the cell width of the layout.
    /// </summary>
    [Parameter]
    public double CellWidth { get; set; } = 60;

    /// <summary>
    /// Gets or sets the cell height of the layout.
    /// </summary>
    [Parameter]
    public double CellHeight { get; set; } = 60;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var row = index / Columns;
        var col = (row% 2 == 0) ? (index % Columns) : (Columns - 1 - (index % Columns));

        animatedElement.OffsetXState = CreateState(col * CellWidth);
        animatedElement.OffsetYState = CreateState(row * CellHeight);
    }
}
