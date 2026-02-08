namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of labels used for console components.
/// </summary>
public sealed record class ConsoleLabels
{
    /// <summary>
    /// Gets the default instance of the ConsoleLabel class.
    /// </summary>
    public static ConsoleLabels Default { get; } = new ConsoleLabels();

    /// <summary>
    /// Gets or init the label used for the pause button for the pause state.
    /// </summary>
    public string Pause { get; init; } = "Pause";

    /// <summary>
    /// Gets or init the label used for the export button.
    /// </summary>
    public string Export { get; init; } = "Export";

    /// <summary>
    /// Gets or init the label used for the clear button.
    /// </summary>
    public string Clear { get; init; } = "Clear";

    /// <summary>
    /// Gets or init the label used for the filter button.
    /// </summary>
    public string Filter { get; init; } = "Filter";

    /// <summary>
    /// Gets or init the label used for the settings button.
    /// </summary>
    public string Settings { get; init; } = "Settings";

    /// <summary>
    /// Gets or init the label used for the pause button for resume state.
    /// </summary>
    public string Resume { get; init; } = "Resume";
}
