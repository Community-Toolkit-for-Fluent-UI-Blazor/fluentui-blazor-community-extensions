namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a media importer.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public sealed class ChatMediaImporterEventArgs<TItem> where TItem : class, new()
{
    /// <summary>
    /// Gets or sets the selected items.
    /// </summary>
    public IEnumerable<TItem> Items { get; set; } = [];
}
