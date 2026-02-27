namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Manages the selection and manipulation of signature strokes within a signature input surface.
/// </summary>
/// <remarks>The SelectionManager provides methods to select, deselect, and move signature strokes based on user
/// interactions such as point or rectangle selection. It maintains the current selection state and supports additive
/// selection operations. This class is intended for internal use within the signature input engine and is not
/// thread-safe.</remarks>
public sealed class SelectionManager
{
    /// <summary>
    /// Provides hit testing functionality for stroke-based input or drawing operations.
    /// </summary>
    private readonly StrokeHitTester _strokeHitTester;

    /// <summary>
    /// Provides hit testing functionality for rectangular regions.
    /// </summary>
    private readonly RectangleHitTester _rectangleHitTester;

    /// <summary>
    /// Represents the configuration options used to control signature selection behavior, such as tolerance for hit testing.
    /// </summary>
    private readonly SignatureSelectionEngineOptions _options;

    /// <summary>
    /// Represents the collection of signature strokes that are currently selected.
    /// </summary>
    private readonly List<SignatureStroke> _selected = [];

    /// <summary>
    /// Gets the collection of currently selected signature strokes.
    /// </summary>
    /// <remarks>The returned collection is read-only and reflects the current selection state. Modifying the
    /// selection must be done through the appropriate selection methods.</remarks>
    public IReadOnlyList<SignatureStroke> SelectedStrokes => _selected;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionManager"/> class with the specified
    ///  stroke hit tester, rectangle hit tester, and selection options.
    /// </summary>
    /// <param name="strokeHitTester">Hit tester used for point-based selection of strokes. Cannot be null.</param>
    /// <param name="rectangleHitTester">Hit tester used for rectangle-based selection of strokes. Cannot be null.</param>
    /// <param name="options">Options used to configure the behavior of the selection engine, such as hit testing tolerance. Cannot be null.</param>
    public SelectionManager(
        StrokeHitTester strokeHitTester,
        RectangleHitTester rectangleHitTester,
        SignatureSelectionEngineOptions options)
    {
        _strokeHitTester = strokeHitTester;
        _rectangleHitTester = rectangleHitTester;
        _options = options;
    }

    /// <summary>
    /// Removes all items from the current selection.
    /// </summary>
    /// <remarks>Call this method to clear the selection and reset the state to empty. After calling this
    /// method, no items will be selected.</remarks>
    public void Clear()
    {
        _selected.Clear();
    }

    /// <summary>
    /// Selects the stroke at the specified coordinates, optionally adding to the current selection.
    /// </summary>
    /// <remarks>If no stroke is found at the specified coordinates, the selection remains unchanged. When
    /// additive is true and the stroke is already selected, it will be deselected.</remarks>
    /// <param name="x">The x-coordinate, in device-independent units, of the point to test for stroke selection.</param>
    /// <param name="y">The y-coordinate, in device-independent units, of the point to test for stroke selection.</param>
    /// <param name="additive">true to add or remove the stroke from the current selection; false to clear the selection before selecting the
    /// stroke.</param>
    public void SelectStrokeAt(double x, double y, bool additive)
    {
        var stroke = _strokeHitTester.HitTestPoint(x, y, _options.Tolerance);

        if (!additive)
        {
            _selected.Clear();
        }

        if (stroke is null)
        {
            return;
        }

        if (!_selected.Contains(stroke))
        {
            _selected.Add(stroke);
        }
        else if (additive)
        {
            _selected.Remove(stroke);
        }
    }

    /// <summary>
    /// Selects all strokes that intersect the specified rectangle, optionally adding to the current selection.
    /// </summary>
    /// <param name="rect">The rectangle, in document coordinates, used to determine which strokes to select.</param>
    /// <param name="additive">If <see langword="true"/>, adds the intersecting strokes to the current selection; otherwise, replaces the
    /// current selection.</param>
    public void SelectRectangle(RectD rect, bool additive)
    {
        var hits = _rectangleHitTester.HitTestRectangle(rect);

        if (!additive)
        {
            _selected.Clear();
        }

        foreach (var stroke in hits)
        {
            if (!_selected.Contains(stroke))
            {
                _selected.Add(stroke);
            }
        }
    }

    /// <summary>
    /// Moves all selected strokes by the specified horizontal and vertical offsets.
    /// </summary>
    /// <param name="dx">The distance to move the selected strokes along the X-axis.</param>
    /// <param name="dy">The distance to move the selected strokes along the Y-axis.</param>
    public void Move(double dx, double dy)
    {
        foreach (var stroke in _selected)
        {
            stroke.Translate(dx, dy);
        }
    }
}
