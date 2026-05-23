using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Clipboard;

/// <summary>
/// Represents an internal service for initializing clipboard functionality with a JavaScript module reference.
/// </summary>
internal interface IClipboardInitializer
{
    /// <summary>
    /// Initializes the clipboard service with the specified JavaScript module reference.
    /// </summary>
    /// <param name="module">The JavaScript module reference used for clipboard operations.</param>
    void Initialize(IJSObjectReference module);

    /// <summary>
    /// Writes the specified text asynchronously.
    /// </summary>
    /// <param name="text">The text to write.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the text
    /// was written successfully; otherwise, <see langword="false"/>.</returns>
    ValueTask<bool> WriteTextAsync(string? text);

    /// <summary>
    /// Writes the specified HTML asynchronously.
    /// </summary>
    /// <param name="html">The HTML to write.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the HTML
    /// was written successfully; otherwise, <see langword="false"/>.</returns>
    ValueTask<bool> WriteHtmlAsync(string? html);

    /// <summary>
    /// Writes the specified image asynchronously.
    /// </summary>
    /// <param name="bytes">The image bytes to write.</param>
    /// <param name="mimeType">The MIME type of the image.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the image
    /// was written successfully; otherwise, <see langword="false"/>.</returns>
    ValueTask<bool> WriteImageAsync(byte[] bytes, string mimeType);

    /// <summary>
    /// Reads the text from the clipboard asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the text if available; otherwise, <see langword="null"/>.</returns>
    ValueTask<string?> ReadTextAsync();
}
