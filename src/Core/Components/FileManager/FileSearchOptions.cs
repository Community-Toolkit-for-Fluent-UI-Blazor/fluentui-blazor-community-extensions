namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents options for configuring file search behavior, including which file attributes to search and whether to
/// search recursively.
/// </summary>
/// <remarks>Use this class to specify the criteria for file searches, such as searching within file names,
/// content, or metadata, and whether to include subdirectories in the search. All properties are optional and can be
/// set to customize the search according to specific requirements.</remarks>
public sealed class FileSearchOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the search operation includes item names.
    /// </summary>
    public bool SearchInNames { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the search operation includes the content of items.
    /// </summary>
    public bool SearchInContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the search operation includes metadata fields.
    /// </summary>
    public bool SearchInMetadata { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation should be applied recursively to all nested elements.
    /// </summary>
    public bool Recursive { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether fuzzy matching is enabled.
    /// </summary>
    public bool Fuzzy { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed distance for fuzzy matching operations.
    /// </summary>
    /// <remarks>A lower value requires closer matches, while a higher value allows for more differences
    /// between compared items. Adjust this value to control the strictness of fuzzy comparisons.</remarks>
    public int FuzzyThreshold { get; set; } = 2;

    /// <summary>
    /// Gets or sets the list of file extensions to include in the filter criteria.
    /// </summary>
    /// <remarks>Each extension should be specified without the leading period (for example, "txt" or "jpg").
    /// If the collection is null or empty, no extension-based filtering is applied.</remarks>
    public string[]? Extensions { get; set; }

    /// <summary>
    /// Gets or sets the earliest creation date to filter results.
    /// </summary>
    /// <remarks>Set this property to limit results to items created after the specified date and time. If
    /// null, no lower bound is applied to the creation date filter.</remarks>
    public DateTime? CreatedAfter { get; set; }

    /// <summary>
    /// Gets or sets the latest creation date to filter results.
    /// </summary>
    public DateTime? CreatedBefore { get; set; }

    /// <summary>
    /// Gets or sets the earliest modification date to filter results.
    /// </summary>
    /// <remarks>Use this property to retrieve only items that have been modified after the specified date and
    /// time. If the value is null, no filtering by modification date is applied.</remarks>
    public DateTime? ModifiedAfter { get; set; }

    /// <summary>
    /// Gets or sets the latest modification date and time to filter items.
    /// </summary>
    /// <remarks>Use this property to include only items that were last modified on or before the specified
    /// date and time. Set to null to disable this filter.</remarks>
    public DateTime? ModifiedBefore { get; set; }

    /// <summary>
    /// Gets or sets the minimum allowed size, in bytes, for the associated content.
    /// </summary>
    public long? MinSize { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed size, in bytes.
    /// </summary>
    /// <remarks>Set this property to limit the size of the associated resource. If null, no maximum size is
    /// enforced.</remarks>
    public long? MaxSize { get; set; }

    internal FileSearchOptions Clone()
    {
        return new FileSearchOptions
        {
            CreatedAfter = CreatedAfter,
            CreatedBefore = CreatedBefore,
            Extensions = Extensions,
            Fuzzy = Fuzzy,
            FuzzyThreshold = FuzzyThreshold,
            MaxSize = MaxSize,
            MinSize = MinSize,
            ModifiedAfter = ModifiedAfter,
            ModifiedBefore = ModifiedBefore,
            Recursive = Recursive,
            SearchInContent = SearchInContent,
            SearchInMetadata = SearchInMetadata,
            SearchInNames = SearchInNames
        };
    }
}

