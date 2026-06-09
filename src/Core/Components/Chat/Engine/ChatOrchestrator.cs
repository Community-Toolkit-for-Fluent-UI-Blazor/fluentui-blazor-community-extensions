using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace FluentUI.Blazor.Community.Components.Chat.Engine;

internal sealed class ChatOrchestrator<TItem>
{
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Appelé par Virtualize
    /// </summary>
    public async ValueTask<ItemsProviderResult<TItem>> ProvideAsync(
        Func<ItemsProviderRequest, CancellationToken, ValueTask<ItemsProviderResult<TItem>>> loader,
        ItemsProviderRequest request)
    {
        var cts = new CancellationTokenSource();
        var previous = Interlocked.Exchange(ref _cts, cts);
        previous?.Cancel();
        previous?.Dispose();

        var token = cts.Token;

        try
        {
            return await loader(request, token);
        }
        catch (OperationCanceledException)
        {
            return new ItemsProviderResult<TItem>([], 0);
        }
    }
}
