namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for undo and redo functionality in a signature component.
/// </summary>
/// <remarks>This class allows customization of undo and redo behavior, including enabling or disabling the
/// feature, setting the maximum number of undo steps, and specifying which actions are tracked. Use these options to
/// control how users can revert or repeat actions within the signature interface.</remarks>
public class SignatureUndoRedoOptions
{
    /// <summary>
    /// Gets or sets the maximum number of undo steps (0 = unlimited).
    /// </summary>
    public int MaxUndoSteps { get; set; } = 50;

    /// <summary>
    /// Gets or sets a value indicating whether undo/redo is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether eraser actions should be included in the undo stack.
    /// </summary>
    public bool TrackEraserActions { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether grid changes should be tracked.
    /// </summary>
    public bool TrackOptionChanges { get; set; }

    /// <summary>
    /// Resets the undo tracking configuration to its default values.
    /// </summary>
    /// <remarks>This method restores the maximum undo steps, enables undo tracking, enables eraser action
    /// tracking, and disables option change tracking. Use this method to revert the tracking settings to their initial
    /// state after customizations.</remarks>
    public void Reset()
    {
        MaxUndoSteps = 50;
        Enabled = true;
        TrackEraserActions = true;
        TrackOptionChanges = false;
    }
}
