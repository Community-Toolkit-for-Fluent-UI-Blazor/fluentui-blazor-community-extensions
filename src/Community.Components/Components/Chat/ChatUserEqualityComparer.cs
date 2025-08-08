using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the comparer of two instances of <see cref="ChatUser"/>.
/// </summary>
internal sealed class ChatUserEqualityComparer
    : IEqualityComparer<ChatUser>
{
    /// <summary>
    /// Prevents the creation of an instance of <see cref="ChatUserEqualityComparer"/>.
    /// </summary>
    private ChatUserEqualityComparer()
    { }

    /// <summary>
    /// Gets the default instance of the comparer.
    /// </summary>
    public static ChatUserEqualityComparer Instance {  get; } = new ChatUserEqualityComparer();

    /// <inheritdoc />
    public bool Equals(ChatUser? x, ChatUser? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return x is null;
        }

        return x.Id == y.Id;
    }

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] ChatUser obj)
    {
        return obj.Id.GetHashCode();
    }
}
