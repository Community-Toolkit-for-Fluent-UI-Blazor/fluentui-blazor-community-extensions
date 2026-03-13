using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child elements in a grid layout with a snake-like traversal pattern, alternating the direction of each row.
/// </summary>
/// <remarks>The SnakeLayout component positions items in rows and columns, reversing the order of items in every
/// other row to create a snake-like effect. This layout is useful for visualizations or interfaces where a continuous,
/// back-and-forth arrangement is desired. The number of columns and the size of each cell can be customized using the
/// Columns, CellWidth, and CellHeight parameters.</remarks>
public sealed class SnakeLayout : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnakeLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public SnakeLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the number of columns to display.
    /// </summary>
    /// <remarks>The value determines how many columns are rendered in the component layout. Adjust this
    /// property to control the horizontal arrangement of items.</remarks>
    [Parameter]
    public int Columns { get; set; } = 5;

    /// <summary>
    /// Gets or sets the width, in pixels, of each cell in the component.
    /// </summary>
    [Parameter]
    public double CellWidth { get; set; } = 60;

    /// <summary>
    /// Gets or sets the height, in pixels, of each cell in the component.
    /// </summary>
    [Parameter]
    public double CellHeight { get; set; } = 60;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            var row = i / Columns;
            var col = row % 2 == 0 ? (i % Columns) : (Columns - 1 - (i % Columns));

            var t = items[i].LayoutTransition.Target;
            t.Set("x", col * CellWidth);
            t.Set("y", row * CellHeight);
        }
    }
}

