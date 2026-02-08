using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel that provides configuration options for console output within the application.
/// </summary>
/// <remarks>Use ConsoleOptionsPanel to allow users to customize console-related settings, such as output
/// formatting and logging levels, through the Options property. This panel is typically used within a user interface to
/// enable dynamic adjustment of console behavior during application execution.</remarks>
public partial class ConsoleOptionsPanel : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the ConsoleOptionsPanel class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine how console export options are applied.</param>
    public ConsoleOptionsPanel(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the options used to configure console behavior.
    /// </summary>
    /// <remarks>The Options property allows customization of various console settings, such as output
    /// formatting and logging levels. Modifying these options can affect how console output is displayed and managed
    /// during application execution.</remarks>
    [Parameter]
    public ConsoleOptions Options { get; set; } = new ConsoleOptions();

    /// <summary>
    /// 
    /// </summary>
    private string BatchIntervalMs
    {
        get => Options.BatchInterval.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        set
        {
            if (double.TryParse(value, CultureInfo.InvariantCulture, out var result) &&
                result < TimeSpan.MaxValue.TotalMilliseconds)
            {
                Options.BatchInterval = TimeSpan.FromMilliseconds(result);
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of messages to retain in the console.
    /// </summary>
    private string MaxMessages
    {
        get => Options.MaxMessages.ToString(CultureInfo.InvariantCulture);
        set
        {
            if (int.TryParse(value, out var result))
            {
                Options.MaxMessages = result;
            }
        }
    }
}
