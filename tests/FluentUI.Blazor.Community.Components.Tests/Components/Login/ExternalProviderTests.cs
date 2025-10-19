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

public class ExternalProviderTests : TestBase
{
    private class MockExternalProviderService
    : IExternalProviderService
    {
        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            return ValueTask.FromResult<IEnumerable<ExternalAuthenticationProvider>>(Array.Empty<ExternalAuthenticationProvider>());
        }
    }

    public ExternalProviderTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddScoped<AccountState>();
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<IExternalProviderService, MockExternalProviderService>();
    }

    private static void SetPrivateProperty(object instance, string name, object value)
    {
        var prop = instance.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (prop is null)
            throw new InvalidOperationException($"Property '{name}' not found on {instance.GetType().FullName}");
        prop.SetValue(instance, value);
    }

    private static T GetPrivateField<T>(object instance, string name)
    {
        var fld = instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        if (fld is null)
            throw new InvalidOperationException($"Field '{name}' not found on {instance.GetType().FullName}");
        return (T)fld.GetValue(instance)!;
    }

    [Fact]
    public void ExternalProvider_RenderAlone_Throws_InvalidOperationException()
    {
        // Rendu isolé doit lever car Parent en cascading est requis
        Assert.Throws<InvalidOperationException>(() => RenderComponent<ExternalProvider>());
    }

    [Fact]
    public async Task GetAssociateProviderNameAccount_And_Message_Returns_FormattedStrings()
    {
        // Arrange: rendre FluentCxLogin en injectant le ExternalProvider via ExternalProviderContent
        var labels = new AccountLabels
        {
            AssociateProviderNameAccount = "Provider: {0}",
            AssociateProviderNameAccountMessage = "Message: {0}"
        };

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.Labels, labels)
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ExternalProvider);

        var child = cut.FindComponent<ExternalProvider>();

        // Fournir un AccountState injecté sur l'instance enfant via réflexion
        var accountState = new AccountState { Provider = "GitHub" };
        SetPrivateProperty(child.Instance, "State", accountState);

        // Act: appeler les méthodes privées par réflexion
        var type = child.Instance.GetType();
        var m1 = type.GetMethod("GetAssociateProviderNameAccount", BindingFlags.Instance | BindingFlags.NonPublic);
        var m2 = type.GetMethod("GetAssociateProviderNameAccountMessage", BindingFlags.Instance | BindingFlags.NonPublic);

        var r1 = (string)m1!.Invoke(child.Instance, null)!;
        var r2 = (string)m2!.Invoke(child.Instance, null)!;

        // Assert
        Assert.Equal("Provider: GitHub", r1);
        Assert.Equal("Message: GitHub", r2);
    }

    [Fact]
    public async Task OnRegisterAsync_When_RequireConfirmedAccount_Requests_RegisterConfirmation_View()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create<AccountManagerView>(new object(), (AccountManagerView v) => captured = v))
            
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ExternalProvider);

        var child = cut.FindComponent<ExternalProvider>();
        child.SetParametersAndRender(p => p.Add(x => x.OnRegister, EventCallback.Factory.Create(new object(), (ExternalProviderRegisterEventArgs e) => e.RequireConfirmedAccount = true)));
        // fournir un AccountState et NavigationManager sur l'instance enfant
        SetPrivateProperty(child.Instance, "State", new AccountState { Provider = "GH", ReturnUrl = "/r" });

        // Préparer email sur le modèle privé _model.Email
        var model = GetPrivateField<object>(child.Instance, "_model");
        var emailProp = model.GetType().GetProperty("Email", BindingFlags.Instance | BindingFlags.Public);
        emailProp!.SetValue(model, "a@b.com");

        // Act: invoquer OnRegisterAsync (méthode privée)
        var method = child.Instance.GetType().GetMethod("OnRegisterAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        // Assert: FluentCxLogin doit avoir demandé la vue RegisterConfirmation
        Assert.Equal(AccountManagerView.RegisterConfirmation, cut.Instance.View);
    }

    [Fact]
    public async Task OnRegisterAsync_When_Successful_Navigates_To_ReturnUrl()
    {
        // Arrange: prepare a fake NavigationManager to capture navigation
        string? navigatedTo = null;
        var fakeNav = new TestNavigationManager(uri => navigatedTo = uri);

        var cut = RenderComponent<FluentCxLogin>();
        await cut.Instance.SetViewAsync(AccountManagerView.ExternalProvider);

        var child = cut.FindComponent<ExternalProvider>();

        // inject services/properties directly on the child instance
        SetPrivateProperty(child.Instance, "State", new AccountState { Provider = "GH", ReturnUrl = "/returned" });
        SetPrivateProperty(child.Instance, "NavigationManager", fakeNav);

        // set email on model
        var model = GetPrivateField<object>(child.Instance, "_model");
        var emailProp = model.GetType().GetProperty("Email", BindingFlags.Instance | BindingFlags.Public);
        emailProp!.SetValue(model, "user@example.com");

        // Act
        var method = child.Instance.GetType().GetMethod("OnRegisterAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var task = (Task)method!.Invoke(child.Instance, null)!;
        await task.ConfigureAwait(true);

        // Assert navigation to State.ReturnUrl
        Assert.Equal("/returned", navigatedTo);
    }

    [Fact]
    public async Task OnAfterRenderAsync_Processing_Failures_Trigger_Correct_View_And_State()
    {
        AccountManagerView? captured = null;

        var cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ExternalProvider);

        var child = cut.FindComponent<ExternalProvider>();
        child.SetParametersAndRender(p => p.Add(x => x.OnProcessing, EventCallback.Factory.Create(new object(), (ExternalProviderProcessingEventArgs e) =>
        {
            e.FailReason = ExternalProviderProcessingFailReason.LoginInfoUnavailable;
        })));

        SetPrivateProperty(child.Instance, "State", new AccountState { Provider = "P" });

        await child.Instance.OnFirstAfterRenderAsync();

        // inject required state

        Assert.Equal(AccountManagerView.Login, captured);

        // Now re-render with LockedOut reason to ensure AccountLocked view requested
        captured = null;

        cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ExternalProvider);

        child = cut.FindComponent<ExternalProvider>();
        child.SetParametersAndRender(p => p.Add(x => x.OnProcessing, EventCallback.Factory.Create(new object(), (ExternalProviderProcessingEventArgs e) =>
        {
            e.FailReason = ExternalProviderProcessingFailReason.LockedOut;
        })));

        SetPrivateProperty(child.Instance, "State", new AccountState { Provider = "P" });

        await child.Instance.OnFirstAfterRenderAsync();


        // After re-render the OnAfterRenderAsync of the new child will run; expect AccountLocked
        Assert.Equal(AccountManagerView.AccountLocked, captured);

        // AskForRegister scenario: ensure _mustRegister true and model.Email set
        captured = null;

        cut = RenderComponent<FluentCxLogin>(ps => ps
            .Add(p => p.OnViewChanged, EventCallback.Factory.Create(new object(), (AccountManagerView v) => captured = v))
        );

        await cut.Instance.SetViewAsync(AccountManagerView.ExternalProvider);

        child = cut.FindComponent<ExternalProvider>();
        child.SetParametersAndRender(p => p.Add(x => x.OnProcessing, EventCallback.Factory.Create(new object(), (ExternalProviderProcessingEventArgs e) =>
        {
            e.FailReason = ExternalProviderProcessingFailReason.AskForRegister;
            e.Email = "ask@reg.test";
        })));

        SetPrivateProperty(child.Instance, "State", new AccountState { Provider = "P" });

        await child.Instance.OnFirstAfterRenderAsync();


        // Access private fields to verify state
        var mustRegister = (bool)child.Instance.GetType().GetField("_mustRegister", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance)!;
        var modelField = child.Instance.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(child.Instance)!;
        var modelEmail = (string)modelField.GetType().GetProperty("Email", BindingFlags.Instance | BindingFlags.Public)!.GetValue(modelField)!;

        Assert.True(mustRegister);
        Assert.Equal("ask@reg.test", modelEmail);
    }

    // Small Test NavigationManager implementation to capture calls
    private class TestNavigationManager : NavigationManager
    {
        private readonly Action<string> _onNavigate;
        public TestNavigationManager(Action<string> onNavigate)
        {
            _onNavigate = onNavigate;
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            _onNavigate(uri);
        }
    }
}
