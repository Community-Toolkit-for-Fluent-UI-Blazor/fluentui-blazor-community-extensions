using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Clipboard;

internal sealed class ClipboardInitializer : IClipboardInitializer
{
    private IJSObjectReference? _module;

    public void Initialize(IJSObjectReference module)
    {
        _module = module;
    }

    public ValueTask<string?> ReadTextAsync()
    {
        if (_module is null)
        {
            throw new InvalidOperationException("Clipboard module has not been initialized.");
        }

        return _module.InvokeAsync<string?>("FluentUI.Blazor.Community.Components.Clipboard.ReadText");
    }

    public ValueTask<bool> WriteHtmlAsync(string? html)
    {
        if (_module is null)
        {
            throw new InvalidOperationException("Clipboard module has not been initialized.");
        }

        if (string.IsNullOrEmpty(html))
        {
            return ValueTask.FromResult(false);
        }

        return _module.InvokeAsync<bool>("FluentUI.Blazor.Community.Components.Clipboard.WriteHtml", html);
    }

    public ValueTask<bool> WriteImageAsync(byte[] bytes, string mimeType)
    {
        if (_module is null)
        {
            throw new InvalidOperationException("Clipboard module has not been initialized.");
        }

        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfLessThan(bytes.Length, 1, nameof(bytes));

        return _module.InvokeAsync<bool>("FluentUI.Blazor.Community.Components.Clipboard.WriteImage", bytes, mimeType);
    }

    public ValueTask<bool> WriteTextAsync(string? text)
    {
        if (_module is null)
        {
            throw new InvalidOperationException("Clipboard module has not been initialized.");
        }

        if (string.IsNullOrEmpty(text))
        {
            return ValueTask.FromResult(false);
        }

        return _module.InvokeAsync<bool>("FluentUI.Blazor.Community.Components.Clipboard.WriteText", text);
    }
}
