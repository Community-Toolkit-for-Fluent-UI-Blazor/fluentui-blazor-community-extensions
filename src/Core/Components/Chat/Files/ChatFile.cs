namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents a base chat file.
/// </summary>
public abstract record ChatFile
    : IChatFile
{
    /// <inheritdoc />
    public long Id { get; init; }

    /// <inheritdoc />
    public long MessageId { get; init; }

    /// <inheritdoc />
    public DateTimeOffset CreatedDate { get; init; }

    /// <inheritdoc />
    public string ContentType { get; init; } = string.Empty;

    /// <inheritdoc />
    public string Name { get; init; } = string.Empty;

    /// <inheritdoc />
    public long OwnerId { get; init; } = default!;

    /// <inheritdoc />
    public DateTimeOffset? DeletedDate { get; init; }

    /// <inheritdoc />
    public bool IsDeleted { get; init; }
}
