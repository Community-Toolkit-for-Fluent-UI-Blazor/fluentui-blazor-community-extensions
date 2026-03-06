using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an equality comparer for instances of the <see cref="FileEntry{TItem}" /> class, enabling comparison and hashing based on
/// the entry's contents.
/// </summary>
/// <remarks>This comparer can be used to compare <see cref="FileEntry{TItem}" /> objects for equality in collections such as
/// dictionaries or hash sets. The specific comparison logic should be defined in the implementation of the Equals and
/// GetHashCode methods.</remarks>
/// <typeparam name="TItem">The type of the item contained within the file entry. Must be a reference type with a parameterless constructor.</typeparam>
internal class FileEntryComparer<TItem> : IEqualityComparer<FileEntry<TItem>>
     where TItem : class, new()
{
    /// <summary>
    /// Gets the default equality comparer for the <see cref="FileEntry{TItem}"/> type.
    /// </summary>
    public static FileEntryComparer<TItem> Default { get; } = new();

    /// <inheritdoc />
    public bool Equals(FileEntry<TItem>? x, FileEntry<TItem>? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] FileEntry<TItem> obj)
    {
        return obj.Id.GetHashCode();
    }
}
