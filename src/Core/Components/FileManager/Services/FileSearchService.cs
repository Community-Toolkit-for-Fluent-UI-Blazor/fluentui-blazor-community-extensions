using System.Runtime.CompilerServices;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides file and directory search functionality over a file system abstraction, allowing queries on names,
/// metadata, and content for items of a specified type.
/// </summary>
/// <remarks>This service supports asynchronous, recursive search operations and can be customized to search
/// within file names, metadata, or file content, depending on the provided options. The search is performed using an
/// underlying file provider abstraction, enabling use with various storage backends.</remarks>
/// <typeparam name="TItem">The type of the item associated with each file entry. Must be a reference type.</typeparam>
internal sealed class FileSearchService<TItem> : IFileSearchService<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Reference to the file provider used to access file entries and their associated data.
    /// </summary>
    private readonly IFileProvider<TItem> _provider;

    /// <summary>
    /// 
    /// </summary>
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSearchService{TItem}"/> class with the specified file provider.
    /// </summary>
    /// <param name="provider"></param>
    public FileSearchService(IFileProvider<TItem> provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Asynchronously searches for file entries under the specified root that match the given query and search options.
    /// </summary>
    /// <param name="root">The root file entry from which to begin the search. Must not be null.</param>
    /// <param name="query">The search query used to match file entries. Leading and trailing whitespace is ignored. If null or whitespace,
    /// no results are returned.</param>
    /// <param name="options">The options that control the search behavior, such as whether to search recursively and how matching is
    /// performed.</param>
    /// <param name="metadataExtractor">A function that extracts a string representation of the metadata from an item of type <typeparamref name="TItem"/>.</param>
    /// <returns>An asynchronous stream of file entries that match the search query and options. The stream is empty if no
    /// entries match or if the query is null or whitespace.</returns>
    public async IAsyncEnumerable<FileEntry<TItem>> SearchAsync(
        FileEntry<TItem> root,
        string query,
        Func<TItem, string> metadataExtractor,
        FileSearchOptions options)
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));
        ArgumentException.ThrowIfNullOrEmpty(query, nameof(query));
        ArgumentNullException.ThrowIfNull(metadataExtractor, nameof(metadataExtractor));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        if (string.IsNullOrWhiteSpace(query))
        {
            yield break;
        }

        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();
        }

        _cancellationTokenSource = new();

        var normalized = query.Trim();

        await foreach (var entry in EnumerateAsync(root, options.Recursive, _cancellationTokenSource.Token))
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                yield break;
            }

            if (await MatchesAsync(entry, normalized, options, metadataExtractor, _cancellationTokenSource.Token))
            {
                yield return entry;
            }
        }
    }

    /// <summary>
    /// Determines asynchronously whether the specified file entry matches the given search query based on the provided
    /// search options.
    /// </summary>
    /// <param name="entry">The file entry to evaluate against the search query.</param>
    /// <param name="query">The search query string to match against the file entry's name, metadata, or content.</param>
    /// <param name="options">The options that specify which parts of the file entry (name, metadata, content) to include in the search.</param>
    /// <param name="metadataExtractor">A function that extracts metadata from the file entry's item for use in metadata-based searching.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the file
    /// entry matches the search query according to the specified options; otherwise, <see langword="false"/>.</returns>
    private static async ValueTask<bool> MatchesAsync(
        FileEntry<TItem> entry,
        string query,
        FileSearchOptions options,
        Func<TItem, string> metadataExtractor,
        CancellationToken cancellationToken)
    {
        if (options.Extensions?.Length > 0 &&
            !options.Extensions.Contains(entry.Extension, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        if (options.CreatedAfter is not null &&
            entry.CreatedDate < options.CreatedAfter.Value)
        {
            return false;
        }

        if (options.CreatedBefore is not null &&
            entry.CreatedDate > options.CreatedBefore.Value)
        {
            return false;
        }

        if (options.ModifiedAfter is not null &&
            entry.ModifiedDate < options.ModifiedAfter.Value)
        {
            return false;
        }

        if (options.ModifiedBefore is not null &&
            entry.ModifiedDate > options.ModifiedBefore.Value)
        {
            return false;
        }

        if (options.MinSize is not null &&
            entry.Size < options.MinSize.Value)
        {
            return false;
        }

        if (options.MaxSize is not null &&
            entry.Size > options.MaxSize.Value)
        {
            return false;
        }

        if (options.SearchInNames &&
            entry.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (options.Fuzzy &&
            Levenshtein(entry.Name, query) <= options.FuzzyThreshold)
        {
            return true;
        }

        if (options.SearchInMetadata &&
            MatchesMetadata(entry, query, metadataExtractor))
        {
            return true;
        }

        if (options.SearchInContent &&
            await MatchesContentAsync(entry, query, cancellationToken))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determine if the given file entry matches the search query based on its metadata.
    /// </summary>
    /// <param name="entry">Entry to evaluate against the search query.</param>
    /// <param name="query">Search query to match against the entry's metadata.</param>
    /// <param name="metadataExtractor">Function to extract a string representation of the metadata from the entry's item for matching purposes.</param>
    /// <returns></returns>
    private static bool MatchesMetadata(
        FileEntry<TItem> entry,
        string query,
        Func<TItem, string> metadataExtractor)
    {
        var metadata = metadataExtractor(entry.Item);

        return metadata.Contains(query, StringComparison.OrdinalIgnoreCase) == true;
    }

    /// <summary>
    /// Determines if the content of the given file entry matches the search query.
    /// </summary>
    /// <param name="entry">Entry to evaluate against the search query.</param>
    /// <param name="query">Query to match against the content of the entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async ValueTask<bool> MatchesContentAsync(
        FileEntry<TItem> entry,
        string query,
        CancellationToken cancellationToken)
    {
        if (entry.DataProvider is not null)
        {
            var data = entry.DataProvider();
            var text = System.Text.Encoding.UTF8.GetString(data);

            return text.Contains(query, StringComparison.OrdinalIgnoreCase);
        }
        else if (entry.DataProviderAsync is not null)
        {
            var data = await entry.DataProviderAsync(cancellationToken);
            var text = System.Text.Encoding.UTF8.GetString(data);

            return text.Contains(query, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    /// <summary>
    /// Asynchronously enumerates the specified file system entry and its children, yielding each entry as it is
    /// discovered.
    /// </summary>
    /// <remarks>Entries are yielded in a depth-first order. The enumeration includes both files and
    /// directories. The method uses asynchronous operations to retrieve children, making it suitable for large or
    /// remote file systems.</remarks>
    /// <param name="root">The root file system entry to begin enumeration from. This entry is always included in the results.</param>
    /// <param name="recursive">true to recursively enumerate all subdirectories and their contents; otherwise, false to enumerate only the
    /// immediate children of the root entry.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An asynchronous stream of file system entries, starting with the root entry and including its children. If
    /// recursive is true, all descendants are included; otherwise, only immediate children are returned.</returns>
    private async IAsyncEnumerable<FileEntry<TItem>> EnumerateAsync(
        FileEntry<TItem> root,
        bool recursive,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return root;

        if (!root.IsDirectory)
        {
            yield break;
        }

        var descriptors = await _provider.GetChildrenAsync(root.Id, cancellationToken);

        foreach (var desc in descriptors)
        {
            var child = FileManagerEngine<TItem>.Map(desc, root);
            yield return child;

            if (recursive && child.IsDirectory)
            {
                await foreach (var sub in EnumerateAsync(child, recursive, cancellationToken))
                {
                    yield return sub;
                }
            }
        }
    }

    /// <summary>
    /// Calculates the Levenshtein distance between two strings, which represents the minimum number of single-character
    /// edits required to change one string into the other.
    /// </summary>
    /// <remarks>The Levenshtein distance is commonly used to measure the similarity between two strings for
    /// applications such as spell checking, DNA analysis, and natural language processing.</remarks>
    /// <param name="a">The first string to compare. Can be null or empty.</param>
    /// <param name="b">The second string to compare. Can be null or empty.</param>
    /// <returns>The Levenshtein distance between the two input strings. Returns the length of the other string if one input is
    /// null or empty.</returns>
    private static int Levenshtein(string a, string b)
    {
        if (string.IsNullOrEmpty(a))
        {
            return b.Length;
        }

        if (string.IsNullOrEmpty(b))
        {
            return a.Length;
        }

        var d = new int[a.Length + 1, b.Length + 1];

        for (var i = 0; i <= a.Length; i++)
        {
            d[i, 0] = i;
        }

        for (var j = 0; j <= b.Length; j++)
        {
            d[0, j] = j;
        }

        for (var i = 1; i < d.GetLength(0); i++)
        {
            for (var j = 1; j < d.GetLength(1); j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[a.Length, b.Length];
    }
}
