namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a provider interface for accessing and managing files and directories of a specified type.
/// </summary>
/// <remarks>Implementations of this interface enable abstraction over different file systems or storage backends,
/// allowing consumers to enumerate, access, and retrieve file data and metadata in a consistent manner.</remarks>
/// <typeparam name="TItem">The type of the file or directory metadata managed by the provider. Must be a reference type.</typeparam>
public interface IFileProvider<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Gets the set of capabilities supported by the file provider.
    /// </summary>
    /// <remarks>Use this property to determine which operations, such as reading, writing, or deleting files,
    /// are available through the current file provider implementation.</remarks>
    IFileProviderCapabilities Capabilities { get; }

    /// <summary>
    /// Asynchronously retrieves the child entries associated with the specified parent identifier.
    /// </summary>
    /// <param name="parentId">The unique identifier of the parent entry for which to retrieve child entries. Cannot be null.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a read-only list of child entry
    /// descriptors. The list is empty if the parent has no children.</returns>
    ValueTask<IReadOnlyList<EntryDescriptor<TItem>>> GetChildrenAsync(string parentId);

    /// <summary>
    /// Determines asynchronously whether the specified parent item has any child items.
    /// </summary>
    /// <param name="parentId">The identifier of the parent item to check for child items. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the parent
    /// item has one or more children; otherwise, <see langword="false"/>.</returns>
    ValueTask<bool> HasChildrenAsync(string parentId);
}
