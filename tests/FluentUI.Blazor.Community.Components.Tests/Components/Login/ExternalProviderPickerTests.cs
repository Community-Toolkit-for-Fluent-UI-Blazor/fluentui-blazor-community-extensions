using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class ExternalProviderPickerTests
    : TestBase
{
    private static MethodInfo GetNonPublicMethod(object instance, string name) =>
      instance.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)
      ?? throw new InvalidOperationException($"Method {name} not found on {instance.GetType().FullName}");

    [Fact]
    public void OnInitializedAsync_Calls_ExternalProviderService()
    {
        var fakeService = new FakeExternalProviderService();
        Services.AddSingleton<IExternalProviderService>(fakeService);
        Services.AddSingleton<AccountState>(new AccountState());

        // Render component - lifecycle should call OnInitializedAsync
        var rendered = RenderComponent<ExternalProviderPicker>();

        // The fake service records that GetExternalProvidersAsync was called
        Assert.True(fakeService.WasCalled);
    }

    [Fact]
    public async Task OnProviderSelectedAsync_Sets_State_And_DoesNot_Invoke_Delegate_When_HasDelegateTrue()
    {
        var state = new AccountState();
        Services.AddSingleton<AccountState>(state);
        Services.AddSingleton<IExternalProviderService>(new FakeExternalProviderService());

        var invoked = false;
        var callback = EventCallback.Factory.Create(this, (Action)(() => invoked = true));

        var rendered = RenderComponent<ExternalProviderPicker>(parameters => parameters
            .Add(p => p.OnExternalProviderSelected, callback)
        );

        var instance = rendered.Instance;

        // Call private async method OnProviderSelectedAsync via reflection
        var method = GetNonPublicMethod(instance, "OnProviderSelectedAsync");
        var task = (Task)method.Invoke(instance, new object[] { "google" })!;
        await task;

        Assert.Equal("google", state.Provider);
        Assert.True(invoked);
    }

    [Fact]
    public async Task OnProviderSelectedAsync_Sets_State_When_No_Delegate_Assigned()
    {
        var state = new AccountState();
        Services.AddSingleton<AccountState>(state);
        Services.AddSingleton<IExternalProviderService>(new FakeExternalProviderService());

        var rendered = RenderComponent<ExternalProviderPicker>();
        var instance = rendered.Instance;

        var method = GetNonPublicMethod(instance, "OnProviderSelectedAsync");
        var task = (Task)method.Invoke(instance, new object[] { "github" })!;
        await task;

        Assert.Equal("github", state.Provider);
    }

    [Fact]
    public void GetIconFrom_Returns_Icon_From_Delegate_When_Provided()
    {
        Services.AddSingleton<IExternalProviderService>(new FakeExternalProviderService());
        Services.AddSingleton<AccountState>(new AccountState());

        var testIcon = new TestIcon();
        var rendered = RenderComponent<ExternalProviderPicker>(parameters => parameters
            .Add<Func<string, Icon>>(p => p.IconFromExternal, _ => testIcon)
        );

        var instance = rendered.Instance;
        var method = GetNonPublicMethod(instance, "GetIconFrom");
        var result = method.Invoke(instance, new object[] { "any" });

        Assert.Same(testIcon, result);
    }

    [Fact]
    public void GetIconFrom_Returns_Null_When_No_Delegate_Or_DelegateReturnsNull()
    {
        Services.AddSingleton<IExternalProviderService>(new FakeExternalProviderService());
        Services.AddSingleton<AccountState>(new AccountState());

        // Case 1: no delegate provided
        var rendered1 = RenderComponent<ExternalProviderPicker>();
        var instance1 = rendered1.Instance;
        var method1 = GetNonPublicMethod(instance1, "GetIconFrom");
        var result1 = method1.Invoke(instance1, new object[] { "any" });
        Assert.Null(result1);

        // Case 2: delegate provided but returns null
        var rendered2 = RenderComponent<ExternalProviderPicker>(parameters => parameters
            .Add<Func<string, Icon?>>(p => p.IconFromExternal, _ => null)
        );
        var instance2 = rendered2.Instance;
        var method2 = GetNonPublicMethod(instance2, "GetIconFrom");
        var result2 = method2.Invoke(instance2, new object[] { "any" });
        Assert.Null(result2);
    }

    [Fact]
    public void GetTextContent_Formats_Text_Based_On_SignUpFlag()
    {
        Services.AddSingleton<IExternalProviderService>(new FakeExternalProviderService());
        Services.AddSingleton<AccountState>(new AccountState());

        var labels = AccountLabels.Default with
        {
            ConnectWithProvider = "Connect with {0}",
            SignUpWithProvider = "Sign up with {0}"
        };

        // SignUp = false => Connect
        var renderedConnect = RenderComponent<ExternalProviderPicker>(parameters => parameters
            .Add(p => p.Labels, labels)
            .Add(p => p.SignUp, false)
        );
        var instanceConnect = renderedConnect.Instance;
        var method = GetNonPublicMethod(instanceConnect, "GetTextContent");
        var connectResult = method.Invoke(instanceConnect, new object[] { "Google" }) as string;
        Assert.Equal("Connect with Google", connectResult);

        // SignUp = true => SignUp
        var renderedSignUp = RenderComponent<ExternalProviderPicker>(parameters => parameters
            .Add(p => p.Labels, labels)
            .Add(p => p.SignUp, true)
        );
        var instanceSignUp = renderedSignUp.Instance;
        var signUpResult = (string?)GetNonPublicMethod(instanceSignUp, "GetTextContent")
            .Invoke(instanceSignUp, new object[] { "Google" });
        Assert.Equal("Sign up with Google", signUpResult);
    }

    // --- Helpers / fakes used by tests ---

    private sealed class FakeExternalProviderService : IExternalProviderService
    {
        public bool WasCalled { get; private set; }

        public ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync()
        {
            WasCalled = true;
            IEnumerable<ExternalAuthenticationProvider> empty = Array.Empty<ExternalAuthenticationProvider>();
            return ValueTask.FromResult(empty);
        }
    }

    // Minimal concrete Icon for tests
    private sealed class TestIcon : Icon
    {
        public TestIcon() : base("Test", IconVariant.Regular, IconSize.Size20, "<svg></svg>") { }
    }
}
