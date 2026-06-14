using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a user interface panel that enables filtering of console output based on configurable criteria.
/// </summary>
/// <remarks>Use this panel to customize which console messages are displayed by applying filters defined in the
/// associated library configuration. The panel is intended to be integrated into applications that require dynamic
/// control over console output visibility.</remarks>
public partial class ConsoleFilterPanel
    : FluentComponentBase
{
    /// <summary>
    /// Contains all values defined in the ConsoleLevel enumeration.
    /// </summary>
    /// <remarks>This array provides a complete list of console logging levels, which can be used to enumerate
    /// or validate available logging options in console applications.</remarks>
    private static readonly ConsoleLevel[] s_allLevels = Enum.GetValues<ConsoleLevel>();

    /// <summary>
    /// Initializes a new instance of the ConsoleFilterPanel class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine the behavior and available options for the ConsoleFilterPanel. Cannot
    /// be null.</param>
    public ConsoleFilterPanel(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the filter used to determine which console output is displayed.
    /// </summary>
    /// <remarks>Set this property to customize the criteria for displaying console messages. The default
    /// value is a new instance of the ConsoleFilter class.</remarks>
    [Parameter]
    public ConsoleFilter Filter { get; set; } = new ConsoleFilter();

    private void ToggleLevel(ConsoleLevel level, bool isChecked)
    {
        if (isChecked)
        {
            Filter.Levels?.Add(level);
        }
        else
        {
            Filter.Levels?.Remove(level);
        }
    }
}
