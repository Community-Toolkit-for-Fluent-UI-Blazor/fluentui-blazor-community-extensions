using System.Globalization;
using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides search functionality for console messages and observes changes in the console state to maintain an
/// up-to-date search index.
/// </summary>
/// <remarks>This service automatically rebuilds its search index to reflect the current state of the console when
/// initialized or when new messages are added. It is designed to be used in scenarios where efficient searching and
/// real-time updates of console messages are required.</remarks>
internal sealed class ConsoleSearchService
    : IConsoleSearchService, IConsoleStateObserver
{
    /// <summary>
    /// Represents a collection of tokens used to construct search queries, including terms to include, exclude, and
    /// match as exact phrases.
    /// </summary>
    /// <remarks>Use this class to organize and manage the components of a search query. The Includes and
    /// Excludes lists allow for fine-grained control over which terms are considered or omitted during filtering, while
    /// the Phrases list enables matching of exact sequences of words. This class is intended to facilitate the creation
    /// of complex search criteria in filtering and searching operations.</remarks>
    private sealed class QueryTokens
    {
        /// <summary>
        /// Gets the list of items that are included in the current context.
        /// </summary>
        /// <remarks>The returned list is initialized as empty and can be populated as needed. This
        /// property is read-only; items can be added or removed from the list, but the list reference itself cannot be
        /// changed.</remarks>
        public List<string> Includes { get; } = [];

        /// <summary>
        /// Gets the list of items that are excluded from processing.
        /// </summary>
        /// <remarks>This property provides a collection of strings representing the excluded items. The
        /// list is initialized as empty and can be populated as needed.</remarks>
        public List<string> Excludes { get; } = [];

        /// <summary>
        /// Gets the list of phrases associated with the instance.
        /// </summary>
        public List<string> Phrases { get; } = [];
    }

    /// <summary>
    /// Provides access to the current state of the console.
    /// </summary>
    /// <remarks>This field is initialized when the containing class is constructed and is used to manage the
    /// console's state throughout the lifecycle of the class.</remarks>
    private readonly IConsoleState _state;

    /// <summary>
    /// Stores a mapping of string keys to sets of unique GUIDs, using case-sensitive, ordinal string comparison.
    /// </summary>
    /// <remarks>This dictionary ensures that each string key is associated with a collection of distinct
    /// GUIDs. String comparisons are performed using StringComparer.Ordinal, making the mapping case-sensitive and
    /// culture-invariant.</remarks>
    private readonly Dictionary<string, HashSet<Guid>> _index = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets the lock instance used for synchronizing access to shared resources.
    /// </summary>
    /// <remarks>This lock is intended to ensure thread safety when accessing shared data. It is recommended
    /// to use this lock whenever modifying shared state to prevent race conditions.</remarks>
    private readonly Lock _lock = new();

    /// <summary>
    /// Initializes a new instance of the ConsoleSearchService class and registers it as an observer of the specified
    /// console state.
    /// </summary>
    /// <remarks>The search index is automatically rebuilt during initialization to ensure it reflects the
    /// current state of the console.</remarks>
    /// <param name="state">The console state to observe. This parameter must not be null.</param>
    public ConsoleSearchService(IConsoleState state)
    {
        _state = state;
        _state.RegisterObserver(this);
        RebuildIndex();
    }

    /// <summary>
    /// Indexes the words contained in the specified console message, associating each word with the message's unique
    /// identifier.
    /// </summary>
    /// <remarks>This method processes the content of the message by splitting it into individual words and
    /// updates the internal index accordingly. Each word is associated with the message's ID, allowing for efficient
    /// retrieval of messages based on their content.</remarks>
    /// <param name="message">The console message to be indexed. The message must contain the content to be split into words for indexing.</param>
    private void IndexMessage(ConsoleMessage message)
    {
        var text = BuildIndexableText(message);

        foreach (var token in Tokenize(text))
        {
            if (!_index.TryGetValue(token, out var messageIds))
            {
                messageIds = [];
                _index[token] = messageIds;
            }

            messageIds.Add(message.Id);
        }
    }

    /// <summary>
    /// Builds a concatenated string containing indexable text derived from the specified console message.
    /// </summary>
    /// <remarks>The method appends various components of the console message to create a comprehensive
    /// indexable string. If the message contains an exception, its string representation is included. Tags and
    /// properties are also appended in a key-value format.</remarks>
    /// <param name="message">The console message from which to build the indexable text. This parameter cannot be null.</param>
    /// <returns>A string representing the indexable text, which includes the message, category, source, exception details (if
    /// any), tags, and properties.</returns>
    private static string BuildIndexableText(ConsoleMessage message)
    {
        var builder = new StringBuilder();

        Append(message.Message);
        Append(message.Category);
        Append(message.Source);

        if (message.Exception is not null)
        {
            Append(message.Exception.ToString());
        }

        if (message.Tags is not null)
        {
            foreach (var tag in message.Tags)
            {
                Append(tag);
            }
        }

        if (message.Properties is not null)
        {
            foreach (var property in message.Properties)
            {
                Append($"{property.Key}:{property.Value}");
            }
        }

        return builder.ToString();

        void Append(string? text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                builder.Append(' ')
                       .Append(text);
            }
        }
    }

    /// <summary>
    /// Normalizes the specified string by removing diacritical marks and converting it to a standard Unicode form.
    /// </summary>
    /// <remarks>This method converts the input string to lowercase, decomposes characters to separate base
    /// characters from diacritics, removes non-spacing marks, and then recomposes the string. Use this method to
    /// prepare strings for comparison or search operations where diacritics should be ignored.</remarks>
    /// <param name="text">The string to normalize. If null or empty, an empty string is returned.</param>
    /// <returns>A normalized string with diacritical marks removed and in a standard Unicode form. Returns an empty string if
    /// the input is null or empty.</returns>
    private static string Normalize(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var formD = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(formD.Length);

        foreach (var ch in formD)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);

            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Splits the specified text into a sequence of alphanumeric word tokens, omitting any tokens with a length of one
    /// character or less.
    /// </summary>
    /// <remarks>The input text is normalized before tokenization to ensure consistent handling of whitespace
    /// and punctuation. Non-alphanumeric characters are ignored, and only tokens longer than one character are included
    /// in the result.</remarks>
    /// <param name="text">The input text to tokenize. If null, an empty sequence is returned.</param>
    /// <returns>An enumerable collection of strings, each representing a token consisting of two or more consecutive
    /// alphanumeric characters extracted from the input text.</returns>
    private static IEnumerable<string> Tokenize(string? text)
    {
        var norm = Normalize(text);
        var i = 0;
        var len = norm.Length;

        while (i < len)
        {
            while (i < len && !char.IsLetterOrDigit(norm[i]))
            {
                i++;
            }

            var start = i;

            while (i < len && char.IsLetterOrDigit(norm[i]))
            {
                i++;
            }

            if (start < i)
            {
                var token = norm[start..i];

                if (token.Length > 1)
                {
                    yield return token;
                }
            }
        }
    }

    /// <summary>
    /// Parses a search query string into a collection of tokens representing included terms, excluded terms, and
    /// phrases.
    /// </summary>
    /// <remarks>Terms prefixed with '-' are treated as exclusions, while terms prefixed with '+' or without a
    /// prefix are treated as inclusions. Phrases enclosed in double quotes are parsed as single tokens and included in
    /// the phrases collection. The input is trimmed before parsing, and normalization is applied to each
    /// token.</remarks>
    /// <param name="query">The search query string to parse. If null or empty, an empty collection of tokens is returned.</param>
    /// <returns>A QueryTokens object containing the parsed included terms, excluded terms, and phrases from the query string.</returns>
    private static QueryTokens ParseQuery(string? query)
    {
        var tokens = new QueryTokens();

        if (string.IsNullOrEmpty(query))
        {
            return tokens;
        }

        var i = 0;
        var trimmedQuery = query.Trim();
        var len = trimmedQuery.Length;

        while (i < len)
        {
            if (char.IsWhiteSpace(trimmedQuery[i]))
            {
                i++;
                continue;
            }

            if (trimmedQuery[i] == '"')
            {
                var start = ++i;

                while (i < len && trimmedQuery[i] != '"')
                {
                    i++;
                }

                if (start < i)
                {
                    var phrase = trimmedQuery[start..i];

                    if (!string.IsNullOrEmpty(phrase))
                    {
                        tokens.Phrases.Add(Normalize(phrase));
                    }
                }

                if (i < len && trimmedQuery[i] == '"')
                {
                    i++;
                    continue;
                }
            }

            var sign = '+';

            if (trimmedQuery[i] == '+' || trimmedQuery[i] == '-')
            {
                sign = trimmedQuery[i];
                i++;
            }

            var startToken = i;

            while (i < len && !char.IsWhiteSpace(trimmedQuery[i]))
            {
                i++;
            }

            var raw = trimmedQuery[startToken..i];
            var norm = Normalize(raw);

            if (string.IsNullOrEmpty(norm))
            {
                continue;
            }

            if (sign == '-')
            {
                tokens.Excludes.Add(norm);
            }
            else
            {
                tokens.Includes.Add(norm);
            }
        }

        return tokens;
    }

    /// <inheritdoc />
    public void OnCleared()
    {
        if (_lock.TryEnter())
        {
            _index.Clear();
            _lock.Exit();
        }
    }

    /// <inheritdoc />
    public void OnMessagesAdded(IReadOnlyList<ConsoleMessage> messages)
    {
        if (_lock.TryEnter())
        {
            foreach (var message in messages)
            {
                IndexMessage(message);
            }

            _lock.Exit();
        }
    }

    /// <inheritdoc />
    public void RebuildIndex()
    {
        if (_lock.TryEnter())
        {
            _index.Clear();

            foreach (var message in _state.Messages)
            {
                IndexMessage(message);
            }

            _lock.Exit();
        }
    }

    /// <inheritdoc />
    public async ValueTask<IReadOnlyList<ConsoleMessage>> SearchAsync(
        string query,
        ConsoleSearchOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new ConsoleSearchOptions();
        var tokens = ParseQuery(query);
        var currentMessages = _state.Messages;
        var byId = currentMessages.ToDictionary(m => m.Id);
        HashSet<Guid>? candidates = null;

        if (tokens.Includes.Count == 0)
        {
            candidates = [.. byId.Keys];
        }
        else
        {
            foreach (var token in tokens.Includes)
            {
                if (!_index.TryGetValue(token, out var set))
                {
                    return [];
                }

                candidates = candidates is null ? [.. set] : [.. candidates.Intersect(set)];

                if (candidates.Count == 0)
                {
                    break;
                }
            }

            if (candidates is null || candidates.Count == 0)
            {
                return [];
            }

            foreach (var token in tokens.Excludes)
            {
                if (_index.TryGetValue(token, out var set))
                {
                    candidates.ExceptWith(set);
                }
            }

            if (tokens.Phrases.Count > 0)
            {
                var normalizedPhrases = tokens.Phrases.ToList();
                candidates.RemoveWhere(id =>
                {
                    if (!byId.TryGetValue(id, out var message))
                    {
                        return true;
                    }

                    var normText = Normalize(BuildIndexableText(message));

                    foreach (var phrase in normalizedPhrases)
                    {
                        if (!normText.Contains(phrase, StringComparison.Ordinal))
                        {
                            return true;
                        }
                    }

                    return false;
                });
            }
        }

        var results = candidates.Where(byId.ContainsKey).Select(id => byId[id]).OrderBy(m => m.Timestamp).ToList();

        if (options.Top.HasValue)
        {
            results = [.. results.Take(options.Top.Value)];
        }

        return await ValueTask.FromResult(results);
    }
}
