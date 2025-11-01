using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of chapters, typically used to organize or manage chapter information within a larger
/// context such as a book or document.
/// </summary>
public partial class ChapterList
{
    /// <summary>
    /// List of chapters contained in this ChapterList.
    /// </summary>
    private readonly List<ChapterItem> _chapters = [];

    /// <summary>
    /// Initializes a new instance of the ChapterList class with a unique identifier.
    /// </summary>
    public ChapterList()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the orientation in which child elements are arranged.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Gets or sets the child content to be rendered inside this component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Adds a chapter item to the collection.
    /// </summary>
    /// <param name="value">The chapter item to add to the collection. Cannot be null.</param>
    internal void AddItem(ChapterItem value)
    {
        _chapters.Add(value);
    }

    /// <summary>
    /// Removes a chapter item from the collection.
    /// </summary>
    internal void RemoveItem(ChapterItem chapterItem)
    {
        _chapters.Remove(chapterItem);
    }
}
