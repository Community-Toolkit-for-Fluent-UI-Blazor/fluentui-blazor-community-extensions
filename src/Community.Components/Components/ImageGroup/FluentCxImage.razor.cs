using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxImage
    : FluentComponentBase, IAsyncDisposable
{
    private bool _isRendered;
    private bool _hasParameterChanged;

    [Parameter]
    public int? Width { get; set; }

    [Parameter]
    public int? Height { get; set; }

    [Parameter]
    public string? Source { get; set; }

    [Parameter]
    public string? Alt { get; set; }

    [CascadingParameter]
    private FluentCxImageGroup Parent { get; set; } = default!;

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", $"{Width}px", Width.HasValue)
        .AddStyle("height", $"{Height}px", Height.HasValue)
        .Build();

    private string InternalSpreadStyle => $"margin-left: {Parent.GetSpreadMarginLeft(this)}px; border-radius: {Parent.GetBorderRadius()}; background-color: var(--accent-fill-rest); border-color: var(--neutral-foreground-focus); border-width: 2px; border-style: solid; align-items: center; display: inline-flex; flex-shrink: 0; width: {Width}px; height: {Height}px;";

    private string InternalStackStyle => $"margin-left: {Parent.GetStackMarginLeft(this)}px; border-radius: {Parent.GetBorderRadius()}; background-color: var(--accent-fill-rest); border-color: var(--neutral-foreground-focus); border-width: 2px; border-style: solid; align-items: center; display: inline-flex; flex-shrink: 0; width: {Width}px; height: {Height}px;";

    private string InternalPopoverStyle => $"border-radius: {Parent.GetBorderRadius()}; background-color: var(--accent-fill-rest); border-color: var(--neutral-foreground-focus); border-width: 2px; border-style: solid; align-items: center; display: inline-flex; flex-shrink: 0; width: {Width}px; height: {Height}px;";


    internal RenderFragment InternalRenderer { get; set; }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Parent.Remove(this);
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

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
