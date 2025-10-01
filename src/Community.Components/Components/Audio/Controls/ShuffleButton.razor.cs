using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class ShuffleButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon displayed when shuffling is disabled.
    /// </summary>
    private static readonly Icon ShuffleOffIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.ArrowShuffleOff();

    /// <summary>
    /// Represents the icon displayed when shuffling is enabvled.
    /// </summary>
    private static readonly Icon ShuffleOnIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.ArrowShuffle();

    /// <summary>
    /// Indicates whether the collection is currently being shuffled.
    /// </summary>
    private bool _isShuffling;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the shuffle state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnShuffleChanged { get; set; }

    /// <summary>
    /// Gets or sets the label for the shuffle on button.
    /// </summary>
    [Parameter]
    public string? ShuffleOnLabel { get; set; } = "Shuffle on";

    /// <summary>
    /// Gets or sets the label displayed when the shuffle feature is turned off.
    /// </summary>
    [Parameter]
    public string? ShuffleOffLabel { get; set; } = "Shuffle off";

    /// <summary>
    /// Initializes a new instance of the <see cref="ShuffleButton"/> class.
    /// </summary>
    public ShuffleButton()
    {
        Id = $"shuffle-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the shuffle button is clicked.
    /// </summary>
    /// <returns>Returns a task which toggles on / off the shuffle</returns>
    private async Task OnToggleShuffleAsync()
    {
        _isShuffling = !_isShuffling;

        if (OnShuffleChanged.HasDelegate)
        {
            await OnShuffleChanged.InvokeAsync(_isShuffling);
        }
    }
}
