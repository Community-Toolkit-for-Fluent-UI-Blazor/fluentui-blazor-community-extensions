using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class ExternalProviderService()
    : IExternalProviderService
{
    public async ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
    {
        return await ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(
            [
            new ExternalAuthenticationProvider("Microsoft", "Microsoft")
            ]);
    }
}
