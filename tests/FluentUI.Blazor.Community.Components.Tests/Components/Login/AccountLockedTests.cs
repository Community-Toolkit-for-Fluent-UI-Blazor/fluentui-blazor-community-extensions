using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class AccountLockedTests : TestBase
{
    public AccountLockedTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddScoped<AccountState>();
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<IExternalProviderService, MockExternalProviderService>();
    }

    [Fact]
    public void Rendering_AccountLocked_Alone_Throws_InvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => RenderComponent<AccountLocked>());
    }

    [Fact]
    public async Task OnLoginAsync_Requests_Login_View_When_Rendered_Inside_FluentCxLogin()
    {
        var cut = RenderComponent<FluentCxLogin>();
        await cut.Instance.SetViewAsync(AccountManagerView.AccountLocked);

        var child = cut.FindComponent<AccountLocked>();

        var method = child.Instance.GetType().GetMethod("OnLoginAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);

        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.Login, cut.Instance.View);
    }

    private class MockExternalProviderService
        : IExternalProviderService
    {
        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            return ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(Array.Empty<ExternalAuthenticationProvider>());
        }
    }
}
