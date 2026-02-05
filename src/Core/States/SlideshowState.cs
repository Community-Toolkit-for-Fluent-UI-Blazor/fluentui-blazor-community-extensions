using System.Collections.Concurrent;

namespace FluentUI.Blazor.Community.Components.States;

/// <summary>
/// Provides a thread-safe container for managing image sizes, allowing for concurrent access and modification of image
/// dimensions identified by unique identifiers.
/// </summary>
/// <remarks>This class is designed to handle scenarios where multiple threads may read or update image size
/// information simultaneously, ensuring thread safety and consistency.</remarks>
internal sealed class SlideshowState
{
    /// <summary>
    /// Represents the size of each container.
    /// </summary>
    private readonly ConcurrentDictionary<string, (double, double)> _sizes = new();

    /// <summary>
    /// Occurs when the size of an image changes, providing the image identifier and its new width and height.
    /// </summary>
    /// <remarks>This event is raised whenever the dimensions of an image are updated. Subscribers can use the
    /// event arguments to identify which image changed and to obtain the new width and height values. This is useful
    /// for updating UI elements or performing actions that depend on the image's current size.</remarks>
    public event EventHandler<string>? SizeChanged;

    /// <summary>
    /// Adds a new size or updates the size of an existing image identified by the specified ID.
    /// </summary>
    /// <remarks>This method raises the ImageSizeChanged event after the image size is added or updated. If
    /// the specified ID does not exist, a new entry is created; otherwise, the existing size is updated.</remarks>
    /// <param name="id">The unique identifier of the image whose size is being added or updated. Cannot be null.</param>
    /// <param name="width">The width of the image, in pixels. Must be a non-negative value.</param>
    /// <param name="height">The height of the image, in pixels. Must be a non-negative value.</param>
    public void AddOrUpdateSize(string? id, double width, double height)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        _sizes.AddOrUpdate(id, (width, height), (_, _) => (width, height));
        SizeChanged?.Invoke(this, id);
    }

    /// <summary>
    /// Gets the dimensions of the image associated with the specified identifier.
    /// </summary>
    /// <remarks>If the identifier is null or empty, the method will return (0, 0) without attempting to
    /// retrieve the image size.</remarks>
    /// <param name="id">The unique identifier of the image for which the size is requested. Must not be null or empty.</param>
    /// <returns>A tuple containing the width and height of the image in pixels. Returns (0, 0) if the image is not found or the
    /// identifier is invalid.</returns>
    public (double width, double height) GetSize(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return (0, 0);
        }

        if (_sizes.TryGetValue(id, out var size))
        {
            return size;
        }

        return (0, 0);
    }

    /// <summary>
    /// Removes the size entry associated with the specified identifier from the collection.
    /// </summary>
    /// <remarks>If the specified identifier does not exist in the collection, no action is taken.</remarks>
    /// <param name="id">The unique identifier of the image size to remove. This parameter cannot be null or empty.</param>
    public void RemoveSize(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        _sizes.TryRemove(id, out _);
    }
}
