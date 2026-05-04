namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for asynchronously importing data from a string input and returning the result of the import
/// operation.
/// </summary>
/// <typeparam name="TPayload">The type of the payload data that is produced by the import operation.</typeparam>
public interface ISurfaceImporter<TPayload>
{
    /// <summary>
    /// Asynchronously imports data from the specified input string and returns the result of the import operation.
    /// </summary>
    /// <param name="input">The input string containing the data to be imported. The format and content of the string must be compatible
    /// with the expected import schema.</param>
    /// <returns>A value task that represents the asynchronous import operation. The result contains information about the
    /// success or failure of the import, along with any imported payload data.</returns>
    ValueTask<ImportSurfaceResult<TPayload>> ImportAsync(string input);

    /// <summary>
    /// Determines whether the specified input string meets the required validation criteria.
    /// </summary>
    /// <param name="input">The input string to validate. Cannot be null.</param>
    /// <returns>true if the input is valid according to the implemented criteria; otherwise, false.</returns>
    bool IsValidInput(string input);
}
