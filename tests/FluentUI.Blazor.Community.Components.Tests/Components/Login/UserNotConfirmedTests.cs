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

public class UserNotConfirmedTests : TestBase
{
    private class MockExternalProviderService
: IExternalProviderService
    {
        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            return ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(Array.Empty<ExternalAuthenticationProvider>());
        }
    }

    public UserNotConfirmedTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddScoped<AccountState>();
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<IExternalProviderService, MockExternalProviderService>();
    }

    [Fact]
    public void Rendering_UserNotConfirmed_Alone_Throws_InvalidOperationException()
    {
        // Quand le composant est rendu sans parent FluentCxLogin, OnInitialized doit lancer InvalidOperationException
        Assert.Throws<InvalidOperationException>(() => RenderComponent<UserNotConfirmed>());
    }

    [Fact]
    public async Task OnLoginAsync_Requests_Login_View_When_Rendered_Inside_FluentCxLogin()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.UserNotConfirmed);

        var child = cut.FindComponent<UserNotConfirmed>();

        var method = child.Instance.GetType().GetMethod("OnLoginAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);

        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.Login, captured);
    }
}
