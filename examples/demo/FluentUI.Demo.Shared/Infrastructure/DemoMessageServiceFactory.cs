using FluentUI.Blazor.Community.Services;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class DemoMessageServiceFactory
    : IMessageServiceFactory
{
    public IMessageService Create(string name)
    {
        return new DemoMessageService(name);
    }
}
