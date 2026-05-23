namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when an entry is renamed.
/// </summary>
/// <param name="Id">The unique identifier of the entry being renamed.</param>
/// <remarks>Set the Success property to <see langword="true"/> to indicate that the rename operation completed
/// successfully. This class is typically used in event handlers to communicate the result of a rename action.</remarks>
public sealed record RenameEntryEventArgs(string Id)
{
    /// <summary>
    /// Gets the new name to be assigned.
    /// </summary>
    public string NewName { get; init; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool Success { get; set; }
}
