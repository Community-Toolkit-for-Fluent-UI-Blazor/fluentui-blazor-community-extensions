using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an individual color option in a color palette.
/// </summary>
public partial class ColorPaletteItem : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPaletteItem"/> class.
    /// </summary>
    public ColorPaletteItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets the internal class the component use.
    /// </summary>
    private string? InternalClass => DefaultClassBuilder
        .AddClass("palette-item")
        .AddClass("focused", when: IsFocused)
        .AddClass("selected", when: IsSelected)
        .AddClass("selected-flash", when: IsAnimated)
        .Build();

    /// <summary>
    /// Gets the internal style the component use.
    /// </summary>
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("width", $"{Size}px")
        .AddStyle("height", $"{Size}px")
        .Build();

    /// <summary />
    public ElementReference Element { get; set; } = default!;

    /// <summary>
    /// Gets or sets the color value for this palette item.
    /// </summary>
    [Parameter]
    public string Color { get; set; } = "transparent";

    /// <summary>
    /// Gets or sets the size of the color square in pixels. Default is 32.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 32;

    /// <summary>
    /// Gets or sets a value indicating whether this item is focused.
    /// </summary>
    [Parameter]
    public bool IsFocused { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this item is selected.
    /// </summary>
    [Parameter]
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the callback to invoke when the item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the selection change should be animated.
    /// </summary>
    [Parameter]
    public bool IsAnimated { get; set; }

    /// <summary>
    /// Gets or sets the callback to invoke when the component is ready.
    /// </summary>
    [Parameter]
    public EventCallback<ColorPaletteItem> OnReady { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && OnReady.HasDelegate)
        {
            await OnReady.InvokeAsync(this);
        }
    }
}
