namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Manages a collection of signature strokes and provides undo and redo functionality for stroke operations.
/// </summary>
/// <remarks>Use this class to add, remove, clear, and manage signature strokes with support for undoing and
/// redoing changes. The stroke collection is exposed as a read-only list. Thread safety is not guaranteed; synchronize
/// access if used concurrently.</remarks>
public sealed class StrokeManager
{
    /// <summary>
    /// Contains the collection of signature strokes recorded during the input process.
    /// </summary>
    /// <remarks>This field stores individual stroke data as users draw their signature. It is typically used
    /// to reconstruct or analyze the signature for display or validation purposes.</remarks>
    private readonly List<SignatureStroke> _strokes = [];

    /// <summary>
    /// Gets the collection of strokes that represent the user's signature input.
    /// </summary>
    public IReadOnlyList<SignatureStroke> Strokes => _strokes;

    /// <summary>
    /// Gets the current stroke being drawn by the user, if any.
    /// </summary>
    public SignatureStroke? CurrentStroke { get; private set; }

    /// <summary>
    /// Occurs when the associated state changes.
    /// </summary>
    public event EventHandler? OnChanged;

    /// <summary>
    /// Begins a new stroke with the specified style and sets it as the current stroke being drawn.
    /// </summary>
    /// <param name="style">Style of the stroke.</param>
    public void BeginStroke(SignatureStrokeStyle style)
    {
        CurrentStroke = new SignatureStroke(style.Clone());
    }

    /// <summary>
    /// Adds a collection of signature points to the current stroke being drawn, if a stroke is active.
    /// </summary>
    /// <param name="points"></param>
    public void AddPoints(IEnumerable<SignaturePoint> points)
    {
        if (CurrentStroke is null)
        {
            return;
        }

        CurrentStroke.Points.AddRange(points);
    }

    /// <summary>
    /// Ends the current stroke being drawn, adds it to the collection of strokes if it contains points, and returns the completed stroke.
    /// </summary>
    /// <returns>Returns the stroke being drawn.</returns>
    public SignatureStroke? EndStroke()
    {
        if (CurrentStroke is null)
        {
            return null;
        }

        var completed = CurrentStroke;
        CurrentStroke = null;

        if (completed.Points.Count > 0)
        {
            return completed;
        }

        return null;
    }

    /// <summary>
    /// Adds a completed stroke to the collection of strokes.
    /// </summary>
    /// <param name="stroke">Signature stroke to add.</param>
    public void AddStroke(SignatureStroke stroke)
    {
        _strokes.Add(stroke);
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Removes a stroke from the collection of strokes.
    /// </summary>
    /// <param name="stroke">Signature stroke to remove.</param>
    /// <returns>Returns true if the stroke was successfully removed; otherwise, false.</returns>
    public bool RemoveStroke(SignatureStroke stroke)
    {
        var removed =  _strokes.RemoveAll(x => x.Id == stroke.Id);

        if (removed > 0)
        {
            OnChanged?.Invoke(this, EventArgs.Empty);
        }

        return removed > 0;
    }

    /// <summary>
    /// Replaces the entire collection of strokes with a new set of strokes,
    ///  effectively resetting the signature to the provided strokes.
    /// </summary>
    /// <param name="strokes">Strokes to replace with.</param>
    public void ReplaceWith(IEnumerable<SignatureStroke> strokes)
    {
        _strokes.Clear();
        _strokes.AddRange(strokes);

        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Removes all strokes from the collection and raises the changed event.
    /// </summary>
    /// <remarks>Use this method to reset the collection to an empty state. The changed event is triggered
    /// after the collection is cleared, allowing subscribers to respond to the update.</remarks>
    public void Clear()
    {
        _strokes.Clear();

        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Creates a new instance of the StrokeManager class that contains copies of all strokes managed by the current
    /// instance.
    /// </summary>
    /// <remarks>The cloned StrokeManager is independent of the original. Modifications to strokes in the
    /// cloned instance do not affect the original StrokeManager or its strokes.</remarks>
    /// <returns>A StrokeManager object containing cloned strokes from the current instance.</returns>
    public StrokeManager Clone()
    {
        var clone = new StrokeManager();

        foreach (var s in _strokes)
        {
            clone.AddStroke(s.Clone());
        }

        return clone;
    }
}
