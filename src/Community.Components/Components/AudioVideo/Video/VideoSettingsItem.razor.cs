using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a configurable item within a video settings menu, providing display information and click handling for
/// video-related UI components.
/// </summary>
/// <remarks>This class is typically used as a child component within a <see cref="FluentCxVideo"/> parent to
/// represent individual settings options, such as toggles or actions, in a video settings interface. The component
/// supports custom labeling, icon display, and click event handling. Instances are automatically registered with their
/// parent <see cref="FluentCxVideo"/> component when initialized.</remarks>
public partial class VideoSettingsItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoSettingsItem"/> class.
    /// </summary>
    public VideoSettingsItem()
    {
        Id = Identifier.NewId();

    }

    /// <summary>
    /// Gets or sets the label text to display for the component.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets the icon to display for this component.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }
}
