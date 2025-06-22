using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an image.
/// </summary>
public class FluentCxImage
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
    private FluentCxImageGroup? Parent { get; set; }

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", $"{Width}px", Width.HasValue)
        .AddStyle("height", $"{Height}px", Height.HasValue)
        .AddStyle("margin-left", GetMarginLeft(), Parent is not null)
        .AddStyle("border-radius", $"{Parent?.GetBorderRadius()}", Parent is not null)
        .AddStyle("display", "inline-flex", Parent is not null)
        .AddStyle("flex-shrink", "0", Parent is not null)
        .Build();

    private string GetMarginLeft() =>
        Parent?.GroupType == ImageGroupLayout.Spread
            ? $"{Parent.GetSpreadMarginLeft(this)}px"
            : $"{Parent?.GetStackMarginLeft(this)}px";
    /// <summary>
    /// Gets or sets the internal renderer for the component.
    /// </summary>
    internal RenderFragment InternalRenderer { get; set; } = default!;

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Parent?.Remove(this);
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
        Parent?.OnItemParemetersChanged(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        InternalRenderer = builder =>
        {
            builder.OpenElement(0, "img");
            builder.AddAttribute(1, "src", Source);
            builder.AddAttribute(2, "alt", Alt);
            builder.AddAttribute(3, "style", InternalStyle);
            builder.CloseElement();
        };
    }

    /// <inheritdoc />
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            Parent?.Add(this);
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
