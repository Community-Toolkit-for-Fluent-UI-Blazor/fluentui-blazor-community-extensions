namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item within a tile grid layout, providing properties to specify its position and the number of rows
/// and columns it occupies.
/// </summary>
/// <remarks>Implement this interface to enable flexible arrangement of items in a grid where each item can span
/// multiple rows and columns. The Index property determines the item's position in the grid, while RowSpan and
/// ColumnSpan define how much space the item occupies. This is useful for creating dynamic, resizable grid layouts in
/// user interfaces.</remarks>
public interface ITileGridItem
{
    /// <summary>
    /// Gets the key for the item.
    /// </summary>
    /// <remarks>This key is assigned upon creation and cannot be modified thereafter. It is used inside Blazor rendering engine.</remarks>
    public string Key { get; }

    /// <summary>
    /// Gets or sets the zero-based index of the item within its parent collection.
    /// </summary>
    /// <remarks>Ensure that the value assigned to this property is within the valid range of the collection
    /// to avoid out-of-range errors.</remarks>
    long Index { get; set; }

    /// <summary>
    /// Gets or sets the number of rows that the element spans within a table layout.
    /// </summary>
    /// <remarks>A value of 1 indicates that the element occupies a single row. Setting a value greater than 1
    /// allows the element to extend across multiple rows. Ensure that the specified value does not exceed the total
    /// number of available rows in the layout.</remarks>
    int RowSpan { get; set; }

    /// <summary>
    /// Gets or sets the number of columns that the control spans within a grid layout.
    /// </summary>
    /// <remarks>A value of 1 indicates that the control occupies a single column. Setting a higher value
    /// allows the control to span multiple adjacent columns. Ensure that the layout container supports the specified
    /// column span to avoid layout issues.</remarks>
    int ColumnSpan { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an item is currently being dragged by the user.
    /// </summary>
    /// <remarks>This property is typically used in user interface scenarios to determine if a drag-and-drop
    /// operation is in progress. It can be useful for enabling or disabling certain UI elements based on the dragging
    /// state.</remarks>
    bool IsDragging { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target is a preview version.
    /// </summary>
    /// <remarks>This property is useful for determining if the current target is intended for preview use,
    /// which may include features that are not yet finalized or fully supported.</remarks>
    bool IsPreviewTarget { get; set; }

    /// <summary>
    /// Gets or sets the zero-based index of the row that contains the current item.
    /// </summary>
    int Row { get; set; }

    /// <summary>
    /// Gets or sets the zero-based column index of the current element.
    /// </summary>
    int Column { get; set; }
}
