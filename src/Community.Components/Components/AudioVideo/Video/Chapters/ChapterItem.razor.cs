using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chapter item within a collection or sequence of chapters.
/// </summary>
public partial class ChapterItem
{
    /// <summary>
    /// Represents the content to be rendered inside this component.
    /// </summary>
    private readonly RenderFragment _item;

    /// <summary>
    /// Gets or sets the chapter associated with this item.
    /// </summary>
    [Parameter]
    public Chapter? Chapter { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the chapter item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets the child content to be rendered inside this component.
    /// </summary>
    [CascadingParameter]
    private ChapterList? Parent { get; set; }

    /// <summary>
    /// Gets the template for rendering the item.
    /// </summary>
    internal RenderFragment ItemTemplate => _item;

    /// <summary>
    /// Gets the CSS style string applied to the chapter details section.
    /// </summary>
    private string? ChapterDetailsStyle => new StyleBuilder(Style)
        .AddStyle("height", "148px")
        .AddStyle("width", "calc(100% - 202px)", Orientation == Orientation.Horizontal)
        .Build();

    /// <summary>
    /// Gets the orientation of the chapter item based on its parent's orientation.
    /// </summary>
    private Orientation Orientation => Parent?.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;

    /// <summary>
    /// Invokes the associated click event handler asynchronously, if one is assigned.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
