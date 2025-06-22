using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an image.
/// </summary>
public partial class FluentCxImage
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents a value indicating if the component is rendered on the screen.
    /// </summary>
    private bool _isRendered;

    /// <summary>
    /// Represents a value indicating if a parameter has changed.
    /// </summary>
    private bool _hasParameterChanged;

    /// <summary>
    /// Gets or sets the width of the image.
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the image.
    /// </summary>
    [Parameter]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the source of the image.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the alt property.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; }

    /// <summary>
    /// Gets the parent of the component.
    /// </summary>
    [CascadingParameter]
    private FluentCxImageGroup Parent { get; set; } = default!;

    /// <summary>
    /// Gets the internal style for the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", $"{Width}px", Width.HasValue)
        .AddStyle("height", $"{Height}px", Height.HasValue)
        .Build();

    /// <summary>
    /// Gets the style when the group is set to spread mode.
    /// </summary>
    private string InternalSpreadStyle => $"margin-left: {Parent.GetSpreadMarginLeft(this)}px; border-radius: {Parent.GetBorderRadius()}; background-color: var(--accent-fill-rest); border-color: var(--neutral-foreground-focus); border-width: 2px; border-style: solid; align-items: center; display: inline-flex; flex-shrink: 0; width: {Width}px; height: {Height}px;";

    /// <summary>
    /// Gets the style when the group is set to stack mode.
    /// </summary>
    private string InternalStackStyle => $"margin-left: {Parent.GetStackMarginLeft(this)}px; border-radius: {Parent.GetBorderRadius()}; background-color: var(--accent-fill-rest); border-color: var(--neutral-foreground-focus); border-width: 2px; border-style: solid; align-items: center; display: inline-flex; flex-shrink: 0; width: {Width}px; height: {Height}px;";

    /// <summary>
    /// Gets the style when the component is inside a <see cref="FluentPopover" />
    /// </summary>
    private string InternalPopoverStyle => $"border-radius: {Parent.GetBorderRadius()}; background-color: var(--accent-fill-rest); border-color: var(--neutral-foreground-focus); border-width: 2px; border-style: solid; align-items: center; display: inline-flex; flex-shrink: 0; width: {Width}px; height: {Height}px;";

    /// <summary>
    /// Gets or sets the internal renderer for the component.
    /// </summary>
    internal RenderFragment InternalRenderer { get; set; }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Parent.Remove(this);
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Sets the size of the image.
    /// </summary>
    /// <param name="size">Size of the image.</param>
    internal void SetGroupSize(int size)
    {
        Width = size;
        Height = size;
        Parent.OnItemParemetersChanged(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent.Add(this);
    }

    /// <inheritdoc />
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            _isRendered = true;
        }
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Parent is not null &&
            _isRendered &&
            _hasParameterChanged)
        {
            Parent.OnItemParemetersChanged(this);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasParameterChanged = parameters.HasValueChanged(nameof(Width), Width) ||
                               parameters.HasValueChanged(nameof(Height), Height) ||
                               parameters.HasValueChanged(nameof(Source), Source) ||
                               parameters.HasValueChanged(nameof(Class), Class) ||
                               parameters.HasValueChanged(nameof(Alt), Alt);

        return base.SetParametersAsync(parameters);
    }
}
