using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class UnknownErrorTests : TestBase
{
    private class MockExternalProviderService
    : IExternalProviderService
    {
        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            return ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(Array.Empty<ExternalAuthenticationProvider>());
        }
    }

    public UnknownErrorTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddScoped<AccountState>();
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<IExternalProviderService, MockExternalProviderService>();
    }

    [Fact]
    public void Rendering_UnknownError_Alone_Throws_InvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => RenderComponent<UnknownError>());
    }

    [Fact]
    public async Task OnLoginAsync_Requests_Login_View_When_Rendered_Inside_FluentCxLogin()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.UnknownError);

        var child = cut.FindComponent<UnknownError>();

        var method = child.Instance.GetType().GetMethod("OnLoginAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);

        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.Login, captured);
    }
}
