using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the item of the <see cref="FluentCxSleekDial"/>.
/// </summary>
public class SleekDialItem
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the SleekDialItem class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that define the behavior and properties of the SleekDialItem instance. Cannot be
    /// null.</param>
    public SleekDialItem(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the parent of the item.
    /// </summary>
    [CascadingParameter]
    private FluentCxSleekDial? Parent { get; set; }

    /// <summary>
    /// Gets or sets if the item is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the icon of the item.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the text of the item.
    /// </summary>
    /// <remarks>In <see cref="SleekDialMode.Radial"/>, the text isn't rendered.</remarks>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the title of the item.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise on a click on the item.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets if the item is visible.
    /// </summary>
    /// <remarks>
    /// The value is <see langword="true"/> by default.
    /// </remarks>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the callback to raise when the <see cref="IsVisible"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsVisibleChanged { get; set; }

    /// <summary>
    /// Gets the index of the item inside the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    internal int Index => Parent?.InternalItems.IndexOf(this) ?? -1;

    /// <summary>
    /// Occurs when the item is clicked.
    /// </summary>
    /// <returns>Returns a task which raise the <see cref="OnClick"/> callback when completed.</returns>
    internal async Task OnClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        Parent?.RemoveChild(this);
        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.AddChild(this);
    }
}
