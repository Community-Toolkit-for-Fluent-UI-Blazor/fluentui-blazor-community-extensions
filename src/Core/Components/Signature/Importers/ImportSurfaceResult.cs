namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of an attempt to import a surface, including the success status, the imported payload if
/// successful, and an error message if the operation failed.
/// </summary>
/// <typeparam name="TPayload">The type of the payload contained in the import result.</typeparam>
/// <param name="Payload">The payload resulting from a successful import operation, or null if the import failed.</param>
/// <param name="ErrorMessage">A message describing the error if the import operation failed; otherwise, null.</param>
public sealed record ImportSurfaceResult<TPayload>(
    SurfacePayload<TPayload>? Payload = null,
    string? ErrorMessage = null)
{
    /// <summary>
    /// Gets if the import is successfull.
    /// </summary>
    public bool Success => Payload is not null && string.IsNullOrEmpty(ErrorMessage);
}
