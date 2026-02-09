using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a mechanism for comparing subtitle languages for equality.
/// </summary>
/// <remarks>Use this comparer to determine whether two instances of <see cref="SubtitleLanguage"/> represent the
/// same language, such as when using collections that require custom equality logic. This class is intended for
/// internal use and is not thread-safe.</remarks>
internal sealed class SubtitleLanguageComparer : IEqualityComparer<SubtitleLanguage>
{
    /// <summary>
    /// Gets the default instance of the SubtitleLanguageComparer class.
    /// </summary>
    /// <remarks>Use this property to obtain a standard comparer for subtitle languages when no custom
    /// comparison logic is required. The default instance is thread-safe and can be reused across multiple
    /// operations.</remarks>
    public static SubtitleLanguageComparer Default { get; } = new SubtitleLanguageComparer();

    /// <inheritdoc />
    public bool Equals(SubtitleLanguage? x, SubtitleLanguage? y)
    {
        return string.Equals(x?.Code, y?.Code, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x?.Name, y?.Name, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] SubtitleLanguage obj)
    {
        return HashCode.Combine(obj.Code, obj.Name);
    }
}
