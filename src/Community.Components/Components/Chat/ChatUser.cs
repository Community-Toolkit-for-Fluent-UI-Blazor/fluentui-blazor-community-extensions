namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chat user.
/// </summary>
public sealed class ChatUser
    : IEquatable<ChatUser>
{
    /// <summary>
    /// Gets or sets the id of the user.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the avatar of the user.
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// Gets or sets the initials of the user.
    /// </summary>
    public string? Initials { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the id of the culture of the user.
    /// </summary>
    public long CultureId { get; set; }

    /// <summary>
    /// Gets or sets the name of the culture of the user.
    /// </summary>
    public string? CultureName { get; set; }

    /// <summary>
    /// Gets or sets the roles of the user.
    /// </summary>
    public List<string> Roles { get; set; } = [];

    /// <inheritdoc />
    public bool Equals(ChatUser? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is ChatUser other)
        {
            return Equals(other);
        }

        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc />
    public static bool operator ==(ChatUser? left, ChatUser? right)
    {
        if (left is null || right is null)
        {
            return false;
        }

        return left.Id == right.Id;
    }

    /// <inheritdoc />
    public static bool operator !=(ChatUser? left, ChatUser? right)
    {
        return !(left == right);
    }
}
