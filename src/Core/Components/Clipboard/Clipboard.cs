namespace FluentUI.Blazor.Community.Components.Clipboard;

internal sealed class Clipboard(IClipboardInitializer clipboardInitializer)
    : IClipboard
{
    /// <inheritdoc />
    public ValueTask<string?> ReadTextAsync()
    {
        return clipboardInitializer.ReadTextAsync();
    }

    /// <inheritdoc />
    public ValueTask<bool> WriteHtmlAsync(string? html)
    {
        return clipboardInitializer.WriteHtmlAsync(html);
    }

    /// <inheritdoc />
    public ValueTask<bool> WriteImageAsync(byte[] bytes, string mimeType)
    {
        return clipboardInitializer.WriteImageAsync(bytes, mimeType);
    }

    /// <inheritdoc />
    public ValueTask<bool> WriteTextAsync(string? text)
    {
        return clipboardInitializer.WriteTextAsync(text);
    }
}
