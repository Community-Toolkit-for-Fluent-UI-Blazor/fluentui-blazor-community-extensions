using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button that displays video settings and allows users to invoke a settings action in a video player
/// interface.
/// </summary>
public partial class VideoSettingsButton
{
    /// <summary>
    /// Value indicating whether the settings popover is open.
    /// </summary>
    private bool _isSettingsPopoverOpen;

    /// <summary>
    /// Represents the icon displayed when shuffling is disabled.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Settings();

    /// <summary>
    /// Gets or sets the label for the next button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Settings";

    /// <summary>
    /// Gets or sets a value indicating whether the theater button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    /// <remarks>Use this property to specify child markup or components that will be rendered within the body
    /// of this component. Typically set automatically when the component is used as a container in Razor
    /// syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TheaterButton"/> class.
    /// </summary>
    public VideoSettingsButton()
    {
        Id = $"video-settings-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Closes the settings popover and updates the component state.
    /// </summary>
    internal void ClosePopover()
    {
        _isSettingsPopoverOpen = false;
        StateHasChanged();
    }
}
