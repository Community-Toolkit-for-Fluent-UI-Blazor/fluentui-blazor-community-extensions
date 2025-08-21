using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class FluentCxCookieTests : TestBase
{
    public FluentCxCookieTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public async Task StoreCookieAsync_ActivatesAllCookies()
    {
        // Arrange
        var items = new List<CookieItem> { new() { Name = "A", IsActive = false } };
        var comp = RenderComponent<FluentCxCookie>(
            param => param.Add(p => p.Items, items)
        );

        // Act
        var task = comp.Instance.GetType().GetMethod("StoreCookieAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, [true]) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        Assert.All(comp.Instance.Items, i => Assert.True(i.IsActive));
    }

    [Fact]
    public async Task StoreCookieInternalAsync_CallsJsInterop()
    {
        // Arrange
        var items = new List<CookieItem> { new() { Name = "A", IsActive = true } };
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Cookies/FluentCxCookie.razor.js");
        mockModule.SetupVoid("setCookiePolicy", items).SetVoidResult();

        var comp = RenderComponent<FluentCxCookie>(
            param => param.Add(p => p.Items, items)
        );

        // Act
        var task = comp.Instance.GetType().GetMethod("StoreCookieInternalAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, [items]) as Task;

        if(task is not null)
        {
            await task;
        }

        // Assert
        JSInterop.VerifyInvoke("import");
        JSInterop.VerifyInvoke("setCookiePolicy", 1);
    }

    [Fact]
    public async Task InitGoogleAnalyticsAsync_CallsJsInterop_WhenActive()
    {
        // Arrange
        var items = new List<CookieItem> { new() { Name = "Google Analytics", IsActive = true } };

        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Cookies/FluentCxCookie.razor.js");
        mockModule.SetupVoid("initializeGoogleAnalytics", "GA_ID").SetVoidResult();
        mockModule.Setup<IEnumerable<CookieItem>>("getCookiePolicy").SetResult(items);

        var comp = RenderComponent<FluentCxCookie>(
            param => param.Add(p => p.Items, items)
                          .Add(p => p.GoogleAnalyticsId, "GA_ID")
        );

        // Act
        var task = comp.Instance.GetType().GetMethod("InitGoogleAnalyticsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        JSInterop.VerifyInvoke("import");
        JSInterop.VerifyInvoke("initializeGoogleAnalytics", 2); // First call : OnAfterRenderAsync, Second call : InitGoogleAnalyticsAsync
        JSInterop.VerifyInvoke("getCookiePolicy", 1);
    }

    [Fact]
    public async Task InitOtherCookiesAsync_InvokesEventCallback()
    {
        // Arrange
        var called = new List<string>();
        var callback = EventCallback.Factory.Create<string>(this, (string name) =>
        {
            called.Add(name);
            return Task.CompletedTask;
        });

        var items = new List<CookieItem> { new() { Name = "A", IsActive = true } };
        var comp = RenderComponent<FluentCxCookie>(
            param => param.Add(p => p.Items, items)
                          .Add(p => p.OnInitActiveCookie, callback)
        );

        // Act
        var task = comp.Instance.GetType().GetMethod("InitOtherCookiesAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, [items]) as Task;

        if(task is not null)
        {
            await task;
        }

        // Assert
        Assert.Contains("A", called);
    }

    [Fact]
    public async Task OnAcceptAsync_CallsExpectedMethods()
    {
         // Arrange
         var called=new List<string>();
        var items = new List<CookieItem> {
            new()
            {
                Name = "Google Analytics",
                IsActive = true
            },
            new()
            {
                Name = "Another cookie",
                IsActive = true
            }
        };

        var callback = EventCallback.Factory.Create<string>(this, (string name) =>
        {
            called.Add(name);
            return Task.CompletedTask;
        });

        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Cookies/FluentCxCookie.razor.js");
        mockModule.SetupVoid("initializeGoogleAnalytics", "GA_ID").SetVoidResult();
        mockModule.Setup<IEnumerable<CookieItem>>("getCookiePolicy").SetResult(items);
        mockModule.SetupVoid("setCookiePolicy", items).SetVoidResult();

        var comp = RenderComponent<FluentCxCookie>(
            param => param.Add(p => p.Items, items)
                          .Add(p => p.GoogleAnalyticsId, "GA_ID")
                          .Add(p => p.OnInitActiveCookie, callback)
        );

        // Act
        var task = comp.Instance.GetType().GetMethod("OnAcceptAsync", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        JSInterop.VerifyInvoke("import");
        JSInterop.VerifyInvoke("initializeGoogleAnalytics", 2); // First call : OnAfterRenderAsync, Second call : InitGoogleAnalyticsAsync
        JSInterop.VerifyInvoke("getCookiePolicy", 1);
        JSInterop.VerifyInvoke("setCookiePolicy", 1);

        Assert.Contains("Another cookie", called);
    }

    [Fact]
    public async Task OnDeclineAsync_CallsStoreCookieAsync()
    {
        // Arrange
        var items = new List<CookieItem> {
            new()
            {
                Name = "Another cookie",
                IsActive = true
            },
            new()
            {
                Name = "A new cookie",
                IsActive = null
            }
        };

        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Cookies/FluentCxCookie.razor.js");
        mockModule.SetupVoid("initializeGoogleAnalytics", "GA_ID").SetVoidResult();
        mockModule.Setup<IEnumerable<CookieItem>>("getCookiePolicy").SetResult(items);
        mockModule.SetupVoid("setCookiePolicy", items).SetVoidResult();

        var comp = RenderComponent<FluentCxCookie>(
            param => param.Add(p => p.Items, items)
        );

        // Act
        var task = comp.Instance.GetType().GetMethod("OnDeclineAsync", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        JSInterop.VerifyInvoke("import");
        JSInterop.VerifyInvoke("getCookiePolicy", 1);
        JSInterop.VerifyInvoke("setCookiePolicy", 1);

        Assert.True(items.All(i => i.IsActive == false));
    }

    [Fact]
    public async Task OnManageCookiesAsync_ShowsDialogAndStoresCookies()
    {
        // Arrange
        var items = new List<CookieItem> { new() { Name = "A", IsActive = true } };
        var mockDialogService = new Mock<IDialogService>();
        var mockDialogReference = new Mock<IDialogReference>();
        mockDialogReference.SetupGet(r => r.Result).Returns(Task.FromResult(DialogResult.Ok(items)));
        mockDialogService
            .Setup(s => s.ShowDialogAsync<ManageCookie>(It.IsAny<object>(), It.IsAny<DialogParameters>()))
            .ReturnsAsync(mockDialogReference.Object);
        Services.AddScoped(_ => mockDialogService.Object);

        var comp = RenderComponent<FluentCxCookie>(
           param => param.Add(p => p.Items, items)
        );

        var task = comp.Instance.GetType().GetMethod("OnManageCookiesAsync", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)!
            .Invoke(comp.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        var cookieState = comp.Instance.GetType().GetField("_cookieState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(comp.Instance) as IEnumerable<CookieItem>;

        Assert.Equal(items, cookieState);
    }
}
