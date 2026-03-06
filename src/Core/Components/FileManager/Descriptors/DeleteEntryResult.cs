namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of an attempt to delete an entry, including the entry identifier, success status, and any
/// error message.
/// </summary>
/// <param name="Id">The unique identifier of the entry that was attempted to be deleted.</param>
/// <remarks>Use this class to determine whether a delete operation was successful and to retrieve error details
/// if the operation failed.</remarks>
public sealed record DeleteEntryResult(string Id)
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the error message associated with the current operation or state.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
