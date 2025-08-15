using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the item of the <see cref="FluentCxSleekDial"/>.
/// </summary>
public class SleekDialItem
    : FluentComponentBase, IDisposable
{
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
    /// Gets the index of the item inside the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    internal int Index => Parent?.InternalItems.IndexOf(this) ?? -1;

    /// <summary>
    /// Gets or sets the angle of the item.
    /// </summary>
    internal string? Angle { get; private set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveChild(this);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.AddChild(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && Parent is not null)
        {
            await Parent.OnCreatedAsync(this);
        }
    }

    internal async Task OnClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
