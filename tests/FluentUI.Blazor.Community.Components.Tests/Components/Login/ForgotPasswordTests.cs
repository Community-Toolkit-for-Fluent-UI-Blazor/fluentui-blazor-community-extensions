using System.Reflection;
using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ForgotPasswordTests : TestBase
{
    private class MockExternalProviderService
    : IExternalProviderService
    {
        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            return ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(Array.Empty<ExternalAuthenticationProvider>());
        }
    }

    public ForgotPasswordTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddScoped<AccountState>();
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<IExternalProviderService, MockExternalProviderService>();
    }

    [Fact]
    public void ForgotPassword_RenderAlone_Throws_InvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => RenderComponent<ForgotPassword>());
    }

    [Fact]
    public async Task OnLoginAsync_Sets_Login_View_When_NotProcessing()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create<AccountManagerView>(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPassword);

        var child = cut.FindComponent<ForgotPassword>();

        // ensure Parent.State is available on FluentCxLogin; invoke private method OnLoginAsync
        var method = child.Instance.GetType().GetMethod("OnLoginAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var t = (Task)method!.Invoke(child.Instance, null)!;
        await t.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.Login, captured);
    }

    [Fact]
    public async Task OnLoginAsync_Noop_When_IsProcessing()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPassword);
        var child = cut.FindComponent<ForgotPassword>();

        // set private field _isProcessing = true
        child.Instance.GetType().GetField("_isProcessing", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(child.Instance, true);

        var method = child.Instance.GetType().GetMethod("OnLoginAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var t = (Task)method!.Invoke(child.Instance, null)!;
        await t.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.ForgotPassword, captured);
    }

    [Fact]
    public async Task OnSignUpAsync_Requests_Register_View_When_NotProcessing()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create<AccountManagerView>(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPassword);
        var child = cut.FindComponent<ForgotPassword>();

        var method = child.Instance.GetType().GetMethod("OnSignUpAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var t = (Task)method!.Invoke(child.Instance, null)!;
        await t.ConfigureAwait(true);

        Assert.Equal(AccountManagerView.Register, captured);
    }

    [Fact]
    public async Task OnSendInstructionsAsync_Invokes_OnInstructionsSent_When_Successful()
    {
        string? sentEmail = null;

        var cut = RenderComponent<FluentCxLogin>();
        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPassword);

        var child = cut.FindComponent<ForgotPassword>();
        child.SetParametersAndRender(p => p.Add(x => x.OnSendInstructions, EventCallback.Factory.Create(new object(), (SendPasswordEventArgs e) =>
            {
            }))
                                    .Add(p=>p.OnInstructionsSent, EventCallback.Factory.Create(new object(), (string s) => sentEmail = s))
        );

        // set _model.Email
        var model = child.Instance.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance)!;
        var emailProp = model.GetType().GetProperty("Email", BindingFlags.Instance | BindingFlags.Public);
        emailProp!.SetValue(model, "u@e.com");

        // Act
        var method = child.Instance.GetType().GetMethod("OnSendInstructionsAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        Assert.Equal("u@e.com", sentEmail);
    }

    [Fact]
    public async Task OnSendInstructionsAsync_Sets_ErrorMessage_On_EmailNotFound()
    {
        string? errorMessage = null;

        var labels = new AccountLabels
        {
            EmailNotFoundErrorMessage = "Not found: {0}"
        };

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.Labels, labels)
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPassword);

        var child = cut.FindComponent<ForgotPassword>();
        child.SetParametersAndRender(p => p.Add(x => x.OnSendInstructions, EventCallback.Factory.Create<SendPasswordEventArgs>(new object(), (SendPasswordEventArgs e) =>
        {
            e.FailReason = SendPasswordFailReason.EmailNotFound;
        })));

        // set _model.Email
        var model = child.Instance.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance)!;
        var emailProp = model.GetType().GetProperty("Email", BindingFlags.Instance | BindingFlags.Public);
        emailProp!.SetValue(model, "noone@x.test");

        // Act
        var method = child.Instance.GetType().GetMethod("OnSendInstructionsAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        // retrieve private _errorMessage
        errorMessage = (string?)child.Instance.GetType().GetField("_errorMessage", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance);

        Assert.Equal("Not found: noone@x.test", errorMessage);
    }

    [Fact]
    public async Task OnSendInstructionsAsync_Sets_ErrorMessage_On_NoServerResponse()
    {
        string? errorMessage = null;

        var labels = new AccountLabels
        {
            NoServerResponseErrorMessage = "Server issue"
        };

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.Labels, labels)
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ForgotPassword);

        var child = cut.FindComponent<ForgotPassword>();
        child.SetParametersAndRender(p => p.Add(x => x.OnSendInstructions, EventCallback.Factory.Create<SendPasswordEventArgs>(new object(), (SendPasswordEventArgs e) =>
        {
            e.FailReason = SendPasswordFailReason.NoServerResponse;
        })));

        // set _model.Email
        var model = child.Instance.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance)!;
        var emailProp = model.GetType().GetProperty("Email", BindingFlags.Instance | BindingFlags.Public);
        emailProp!.SetValue(model, "someone@x.test");

        // Act
        var method = child.Instance.GetType().GetMethod("OnSendInstructionsAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        // retrieve private _errorMessage
        errorMessage = (string?)child.Instance.GetType().GetField("_errorMessage", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance);

        Assert.Equal("Server issue", errorMessage);
    }
}
