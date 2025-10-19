using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ForgotPasswordConfirmationTests : TestBase
{
    private class MockExternalProviderService
    : IExternalProviderService
    {
        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            return ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(Array.Empty<ExternalAuthenticationProvider>());
        }
    }

    public ForgotPasswordConfirmationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddScoped<AccountState>();
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<IExternalProviderService, MockExternalProviderService>();
    }

    [Fact]
    public async Task OnResendEmailAsync_Invokes_Handler_When_Email_Is_Present()
    {
        string? captured = null;

        var cut = RenderComponent<FluentCxLogin>();

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPasswordConfirmation);
        
        var child = cut.FindComponent<ForgotPasswordConfirmation>();
        child.SetParametersAndRender(p => p.Add(x => x.Email, "resend@x.test")
            .Add(p => p.OnResendEmail, EventCallback.Factory.Create<string>(new object(), (string s) => captured = s))
            );

        var method = child.Instance.GetType().GetMethod("OnResendEmailAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Equal("resend@x.test", captured);
    }

    [Fact]
    public async Task OnResendEmailAsync_Does_Not_Invoke_When_Email_Is_Whitespace_Or_NoDelegate()
    {
        string? captured = null;


        var cut = RenderComponent<FluentCxLogin>();

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPasswordConfirmation);

        var child = cut.FindComponent<ForgotPasswordConfirmation>();
        child.SetParametersAndRender(p => p.Add(x => x.Email, "        ")
            .Add(p => p.OnResendEmail, EventCallback.Factory.Create<string>(new object(), (string s) => captured = s))
            );

        var method = child.Instance.GetType().GetMethod("OnResendEmailAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Null(captured);
    }

    [Fact]
    public async Task OnLoginAsync_Requests_Login_View_When_Parent_Present()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPasswordConfirmation);

        var child = cut.FindComponent<ForgotPasswordConfirmation>();

        var method = child.Instance.GetType().GetMethod("OnLoginAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.Login, captured);
    }
}
