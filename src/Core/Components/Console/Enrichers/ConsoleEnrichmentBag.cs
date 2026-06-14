namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of contextual enrichment data for console log entries, including source, category,
/// correlation identifiers, properties, and tags.
/// </summary>
/// <remarks>Use this class to add structured and dynamic context to console logging operations. Properties and
/// tags can be merged from external sources to provide additional information for log analysis and correlation. This
/// type is intended to facilitate enriched, structured logging scenarios where contextual data is important for
/// diagnostics and tracing.</remarks>
public sealed class ConsoleEnrichmentBag
{
    /// <summary>
    /// Gets or sets the source of the data.
    /// </summary>
    /// <remarks>This property can be null, indicating that no source has been specified.</remarks>
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the category of the data.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier for the data.
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the current activity, which can be used for tracking and logging
    /// purposes.
    /// </summary>
    public string? ActivityId { get; set; }

    /// <summary>
    /// Gets the collection of properties associated with the enrichment data.
    /// </summary>
    public Dictionary<string, object?> Properties { get; } = [];

    /// <summary>
    /// Gets the collection of tags associated with the enrichment data.
    /// </summary>
    public HashSet<string> Tags { get; } = [];

    /// <summary>
    /// Merges the specified properties into the current properties collection, updating existing entries or adding new
    /// ones.
    /// </summary>
    /// <remarks>This method will overwrite any existing properties with the same keys from the provided
    /// dictionary. It is important to ensure that the keys in the dictionary are unique to avoid unintentional data
    /// loss.</remarks>
    /// <param name="properties">An optional dictionary containing key-value pairs of properties to merge. If null, no changes are made.</param>
    public void MergeProperties(IReadOnlyDictionary<string, object?>? properties)
    {
        if (properties is null)
        {
            return;
        }

        foreach (var property in properties)
        {
            Properties[property.Key] = property.Value;
        }
    }

    /// <summary>
    /// Adds non-empty tags from the specified collection to the existing set of tags.
    /// </summary>
    /// <remarks>This method does not check for duplicate tags; if a tag already exists in the collection, it
    /// will be added again.</remarks>
    /// <param name="tags">An enumerable collection of strings representing the tags to be merged. Only tags that are
    /// not null, empty, or whitespace are added. If <paramref name="tags"/> is null, no action is taken.</param>
    public void MergeTags(IEnumerable<string>? tags)
    {
        if (tags is null)
        {
            return;
        }

        foreach (var tag in tags)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                Tags.Add(tag);
            }
        }
    }
}
