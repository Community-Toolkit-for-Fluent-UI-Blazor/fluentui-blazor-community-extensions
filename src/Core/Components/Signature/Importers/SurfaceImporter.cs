namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for implementing surface importers that process payloads of a specified type.
/// </summary>
/// <typeparam name="TPayload">The type of payload to be imported by the surface importer.</typeparam>
public abstract class SurfaceImporter<TPayload>
    : ISurfaceImporter<TPayload>
{
    /// <summary>
    /// Gets the specific error message when the input is not valid for import.
    /// </summary>
    protected virtual string ErrorMessage => "The provided input is invalid and cannot be imported.";

    /// <inheritdoc />
    public ValueTask<ImportSurfaceResult<TPayload>> ImportAsync(string input)
    {
        if (!IsValidInput(input))
        {
            return new ValueTask<ImportSurfaceResult<TPayload>>(new ImportSurfaceResult<TPayload>(ErrorMessage: ErrorMessage));
        }

        return ParseAsync(input);
    }

    /// <summary>
    /// Determines whether the specified input string is considered valid according to the current validation logic.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom validation logic for the input
    /// string.</remarks>
    /// <param name="input">The input string to validate. May be null or empty depending on the implementation.</param>
    /// <returns>true if the input is valid; otherwise, false.</returns>
    public virtual bool IsValidInput(string input)
    {
        return true;
    }

    /// <summary>
    /// Asynchronously parses the specified input string and returns a payload representing the parsed surface data.
    /// </summary>
    /// <param name="input">The input string to parse. Cannot be null.</param>
    /// <returns>A value task that represents the asynchronous parse operation. The result contains a payload with the parsed
    /// surface data.</returns>
    protected abstract ValueTask<ImportSurfaceResult<TPayload>> ParseAsync(string input);
}
