namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for asynchronously loading and saving signature options.
/// </summary>
/// <remarks>Implementations of this interface provide persistent storage for signature options, enabling
/// retrieval and update operations. Methods are asynchronous to support non-blocking I/O scenarios, such as file or
/// database access.</remarks>
public interface ISignatureOptionsStorage
{
    /// <summary>
    /// Asynchronously loads the signature options configuration.
    /// </summary>
    /// <returns>A ValueTask that represents the asynchronous operation. The result contains a SignatureOptions instance if the
    /// configuration is available; otherwise, null.</returns>
    ValueTask<SignatureRenderingOptions?> LoadAsync();

    /// <summary>
    /// Asynchronously saves the signature configuration using the specified options.
    /// </summary>
    /// <param name="options">The options that define the signature settings to be saved. Cannot be null.</param>
    /// <returns>A ValueTask that represents the asynchronous save operation.</returns>
    ValueTask SaveAsync(SignatureRenderingOptions options);
}
