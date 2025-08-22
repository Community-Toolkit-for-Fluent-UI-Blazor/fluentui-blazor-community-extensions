using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view for a <see cref="CookieItem"/>.
/// </summary>
public partial class CookieItemView
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="CookieItemView"/>.
    /// </summary>
    public CookieItemView()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the item to render.
    /// </summary>
    [Parameter]
    public CookieItem? Item {  get; set; }

    /// <summary>
    /// Gets or sets the template of the item.
    /// </summary>
    [Parameter]
    public RenderFragment<CookieItem>? Template { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that a divider is shown.
    /// </summary>
    [Parameter]
    public bool ShowDivider { get; set; }

    /// <summary>
    /// Gets or sets the accept label button.
    /// </summary>
    [Parameter]
    public string? AcceptLabel { get; set; }

    /// <summary>
    /// Gets or sets the decline label button.
    /// </summary>
    [Parameter]
    public string? DeclineLabel { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when the <see cref="CookieItem.IsActive" /> parameter changed.
    /// </summary>
    [Parameter]
    public EventCallback OnActivationChanged { get; set; }

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
