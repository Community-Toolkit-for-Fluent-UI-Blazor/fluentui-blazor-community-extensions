namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents the result of a chat group creation operation.
/// </summary>
public sealed record ChatGroupCreateResult
{
    /// <summary>
    /// Gets a value indicating whether the group creation was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Gets the unique identifier of the created group, if successful.
    /// </summary>
    public long? GroupId { get; init; }

    /// <summary>
    /// Gets the error message if the group creation failed, otherwise null.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets the name of the created group, if successful.
    /// </summary>
    public string? GroupName { get; init; }
}
