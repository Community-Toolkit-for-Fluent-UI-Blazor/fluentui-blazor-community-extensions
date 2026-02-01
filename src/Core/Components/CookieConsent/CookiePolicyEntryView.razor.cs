using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view for a <see cref="CookiePolicyEntry"/>.
/// </summary>
public partial class CookiePolicyEntryView
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="CookiePolicyEntryView"/>.
    /// </summary>
    public CookiePolicyEntryView(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the item to render.
    /// </summary>
    [Parameter]
    public CookiePolicyEntry? Item { get; set; }

    /// <summary>
    /// Gets or sets the template of the item.
    /// </summary>
    [Parameter]
    public RenderFragment<CookiePolicyEntry>? Template { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is expanded.
    /// </summary>
    /// <remarks>This property is typically used to control the visibility of additional content associated
    /// with the item. When set to <see langword="true"/>, the item is expanded to show its details; otherwise, it is
    /// collapsed.</remarks>
    private bool IsExpanded { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when the <see cref="CookiePolicyEntry.IsActive" /> parameter changed.
    /// </summary>
    [Parameter]
    public EventCallback OnActivationChanged { get; set; }

    /// <summary>
    /// Toggles the expanded state of the element, switching it between expanded and collapsed.
    /// </summary>
    /// <remarks>This method updates the IsExpanded property, which indicates whether the element is currently
    /// expanded. Calling this method will change the state to its opposite.</remarks>
    private void Toggle()
    {
        IsExpanded = !IsExpanded;
    }

    /// <summary>
    /// Occurs when the activation has changed.
    /// </summary>
    /// <returns>Returns a task which invokes <see cref="OnActivationChanged"/> when completed.</returns>
    private async Task OnActivationChangedAsync()
    {
        if (OnActivationChanged.HasDelegate)
        {
            await OnActivationChanged.InvokeAsync();
        }
    }
}
