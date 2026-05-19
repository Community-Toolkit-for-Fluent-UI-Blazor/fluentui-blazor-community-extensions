using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

internal sealed class FileUploader : IFileUploader
{
    private IJSObjectReference? _module;
    private const int BufferSize = 1024 * 1024;
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Divides the specified byte array into sequential chunks of a fixed size.
    /// </summary>
    /// <remarks>The size of each chunk is determined by the constant BufferSize. Ensure that BufferSize is a
    /// positive integer to avoid runtime errors.</remarks>
    /// <param name="data">The byte array to be split into chunks. Cannot be null and must contain at least one element.</param>
    /// <returns>An enumerable collection of byte arrays, each representing a chunk of the original data. The final chunk may be
    /// smaller than the fixed chunk size if the total length is not a multiple of the chunk size.</returns>
    private static IEnumerable<byte[]> GetChunks(byte[] data)
    {
        for (var i = 0; i < data.Length; i += BufferSize)
        {
            var size = Math.Min(BufferSize, data.Length - i);
            var chunk = new byte[size];
            Buffer.BlockCopy(data, i, chunk, 0, size);

            yield return chunk;
        }
    }

    /// <inheritdoc />
    public async Task CancelAsync()
    {
        if (_cts is not null)
        {
            await _cts.CancelAsync();
        }
    }

    /// <inheritdoc />
    public async Task<string?> UploadFileAsync(string id, byte[] content, string contentType)
    {
        if (_module is not null)
        {
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Components.FileUploaderProvider.InitializeStream", id);

            var chunks = GetChunks(content);

            foreach (var chunk in chunks)
            {
                if (token.IsCancellationRequested)
                {
                    await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Components.FileUploaderProvider.CancelStream", id);
                    return null;
                }

                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Components.FileUploaderProvider.Stream", id, chunk);
            }

            return await _module.InvokeAsync<string>("FluentUI.Blazor.Community.Components.FileUploaderProvider.FinalizeStream", id, contentType);
        }

        return null;
    }

    /// <inheritdoc />
    public void Initialize(IJSObjectReference module)
    {
        _module = module;
    }

    public async Task RevokeUrlAsync(string? url)
    {
        if (string.IsNullOrEmpty(url) || _module is null)
        {
            return;
        }

        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Components.FileUploaderProvider.RevokeUrl", url);
    }
}
