using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Audio;

/// <summary>
/// Represents the <see cref="ChatAudioWaveVisualizer"/> component, which is used to visualize audio waves in a chat interface.
/// </summary>
public partial class ChatAudioWaveVisualizer : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatAudioWaveVisualizer"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatAudioWaveVisualizer(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets number of waves to display in the visualizer. Default is 36.
    /// </summary>
    [Parameter]
    public int WaveCount { get; set; } = 36;

    /// <summary>
    /// Gets or sets the elapsed recording time to display in the visualizer.
    /// </summary>
    [Parameter]
    public string? RecordingTime { get; set; } = "00:00";
}
