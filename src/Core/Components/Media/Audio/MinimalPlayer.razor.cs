using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a minimal audio player component with play/pause functionality.
/// </summary>
public partial class MinimalPlayer : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MinimalPlayer"/> class with a unique identifier.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component.</param>
    public MinimalPlayer(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = $"minimal-player-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the event callback that is triggered when the play/pause button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlayPause { get; set; }

    /// <summary>
    /// Gets or sets the label for the play button.
    /// </summary>
    [Parameter]
    public string PlayLabel { get; set; } = "Play";

    /// <summary>
    /// Gets or sets the label for the pause button.
    /// </summary>
    [Parameter]
    public string PauseLabel { get; set; } = "Pause";
}
