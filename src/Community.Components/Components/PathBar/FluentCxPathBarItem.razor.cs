using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component inside a <see cref="FluentCxPathBar"/>.
/// </summary>
public partial class FluentCxPathBarItem
    : FluentComponentBase
{
    /// <summary>
    /// Represents the render fragment for the label.
    /// </summary>
    private readonly RenderFragment _renderLabelFragment;

    /// <summary>
    /// Gets or sets the label of the component.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the items inside the component.
    /// </summary>
    [Parameter]
    public IEnumerable<IPathBarItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the ancestor of this instance.
    /// </summary>
    [CascadingParameter]
    internal FluentCxPathBar? Ancestor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the label is visible or not.
    /// </summary>
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    /// <summary>
    /// Gets or sets the icon to show.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the callback when the component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnTapped { get; set; }

    /// <summary>
    /// Gets the internal class for the component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("fluentcx-path-bar-item")
        .Build();

    /// <summary>
    /// Occurs when the component is clicked.
    /// </summary>
    /// <returns>Returns a task which raises the <see cref="OnTapped"/> callback when completed.</returns>
    internal async Task OnTappedAsync()
    {
        if (OnTapped.HasDelegate)
        {
            await OnTapped.InvokeAsync();
        }
    }
}
