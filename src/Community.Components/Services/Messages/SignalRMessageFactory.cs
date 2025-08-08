using FluentUI.Blazor.Community.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace FluentUI.Blazor.Community.Services;

internal sealed class SignalRMessageFactory(
    IConfiguration configuration,
        NavigationManager navigationManager,
        ICookieProvider cookieProvider)
    : IMessageServiceFactory
{
    public IMessageService Create(string name)
    {
        return new SignalRMessageService(name, configuration, navigationManager, cookieProvider);
    }
}
