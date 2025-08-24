using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the item of the <see cref="FluentCxSleekDial"/>.
/// </summary>
public class SleekDialItem
    : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Represents a value if the <see cref="IsVisible"/> property has changed.
    /// </summary>
    private bool _isVisibleChanged;

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
    /// Gets or sets the angle of the item.
    /// </summary>
    internal string? Angle { get; private set; }

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

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_isVisibleChanged)
        {
            if (IsVisibleChanged.HasDelegate)
            {
                await IsVisibleChanged.InvokeAsync(IsVisible);
            }

            Parent?.Viewer?.Refresh();
        }
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        _isVisibleChanged = parameters.HasValueChanged(nameof(IsVisible), IsVisible);

        await base.SetParametersAsync(parameters);
    }
}
