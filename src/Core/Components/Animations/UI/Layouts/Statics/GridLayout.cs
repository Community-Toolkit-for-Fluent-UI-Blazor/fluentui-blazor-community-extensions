using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child items in a uniform grid layout with a specified number of columns and cell dimensions.
/// </summary>
/// <remarks>Use this layout to position items in rows and columns, where each cell has the same width and height.
/// The number of columns and the size of each cell can be customized using the corresponding properties. Items are
/// placed in row-major order, filling each row before starting a new one.</remarks>
public sealed class GridLayout : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GridLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public GridLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the number of columns to display in the layout.
    /// </summary>
    /// <remarks>The value determines how many columns are rendered. Adjust this property to control the
    /// distribution of content across the available space.</remarks>
    [Parameter]
    public int Columns { get; set; } = 4;

    /// <summary>
    /// Gets or sets the width of each cell, in pixels.
    /// </summary>
    [Parameter]
    public double CellWidth { get; set; } = 100;

    /// <summary>
    /// Gets or sets the height of each cell, in pixels.
    /// </summary>
    [Parameter]
    public double CellHeight { get; set; } = 100;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            var row = i / Columns;
            var col = i % Columns;

            var t = items[i].LayoutTransition.Target;
            t.Set("x", col * CellWidth);
            t.Set("y", row * CellHeight);
        }
    }
}
