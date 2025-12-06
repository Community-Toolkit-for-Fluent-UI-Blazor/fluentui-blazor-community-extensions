using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a compact seek bar component for media playback, providing functionality to display and interact with
/// playback progress.
/// </summary>
public partial class CompactSeekBar
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompactSeekBar"/> class.
    /// </summary>
    public CompactSeekBar()
    {
        Id = $"compact-seekbar-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the duration of the track, in seconds.
    /// </summary>
    [Parameter]
    public double Duration { get; set; }

    /// <summary>
    /// Gets or sets the current playback time of the media, in seconds.
    /// </summary>
    [Parameter]
    public double CurrentTime { get; set; }

    /// <summary>
    /// Gets the progression percentage of the current time relative to the duration.
    /// </summary>
    private string Progression => string.Format(CultureInfo.InvariantCulture, "{0}%", CurrentTime / Duration * 100);
}
