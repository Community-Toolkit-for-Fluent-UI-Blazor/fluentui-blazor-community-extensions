using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an image.
/// </summary>
public class FluentCxImageGroupItem
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents a value indicating if a parameter has changed.
    /// </summary>
    private bool _hasParameterChanged;

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
    /// Gets the style of the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", $"{(int)Parent.Size}px")
        .AddStyle("height", $"{(int)Parent.Size}px")
        .AddStyle("margin-left", GetMarginLeft(), !Parent.IsInPopover(this))
        .AddStyle("border-radius", $"{Parent.Shape.ToBorderRadius()}")
        .AddStyle("background-color", Parent.BackgroundStyle, !string.IsNullOrEmpty(Parent.BackgroundStyle) && string.IsNullOrWhiteSpace(Style))
        .AddStyle("border", Parent.BorderStyle, !string.IsNullOrEmpty(Parent.BorderStyle) && string.IsNullOrWhiteSpace(Style))
        .AddStyle("display", "inline-flex")
        .AddStyle("flex-shrink", "0")
        .Build();

    /// <summary>
    /// Gets the margin-left of the component.
    /// </summary>
    /// <returns>Returns the margin-left of the component.</returns>
    private string GetMarginLeft() =>
        Parent?.GroupLayout == ImageGroupLayout.Spread
            ? $"{Parent.GetSpreadMarginLeft(this)}px"
            : $"{Parent?.GetStackMarginLeft(this)}px";

    /// <summary>
    /// Gets or sets the internal renderer for the component.
    /// </summary>
    internal RenderFragment InternalRenderer { get; set; } = default!;

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Parent.Remove(this);
        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Parent is null)
        {
            throw new InvalidOperationException("FluentCxImageGroupItem must be used inside a FluentCxImageGroup component.");
        }

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
            Parent.Add(this);
        }
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasParameterChanged)
        {
            Parent.OnItemParemetersChanged(this);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasParameterChanged = parameters.HasValueChanged(nameof(Source), Source) ||
                               parameters.HasValueChanged(nameof(Class), Class) ||
                               parameters.HasValueChanged(nameof(Alt), Alt);

        return base.SetParametersAsync(parameters);
    }
}
