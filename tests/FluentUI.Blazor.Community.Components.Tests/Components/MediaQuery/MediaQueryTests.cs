using System.Reflection;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class MediaQueryTests : TestBase
{
    // Helper for simulating JSDisconnectedException
    public class ThrowingJSObjectReference : IJSObjectReference
    {
        public bool IsDisposed { get; private set; }
        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            throw new JSDisconnectedException("Simulated disconnect");
        }
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            if(identifier == "dispose")
            {
                return default;
            }

            throw new NotImplementedException();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new NotImplementedException();
    }

    // Helper for simulating normal disposal
    public class NonThrowingJSObjectReference : IJSObjectReference
    {
        public bool IsDisposed { get; private set; }
        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return ValueTask.CompletedTask;
        }
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            if (identifier == "dispose")
            {
                return default;
            }

            throw new NotImplementedException();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new NotImplementedException();
    }


    public MediaQueryTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void FluentCxMediaQuery_Default()
    {
        // Arrange
        var cut = RenderComponent<FluentCxMediaQuery>();

        // Act

        // Assert
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public async Task FluentCxDeviceDetector_AfterRender_InitializesJavaScript()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/MediaQuery/FluentCxMediaQuery.razor.js");
        mockModule.Setup<bool>("initialize").SetResult(true);
        // Act
        var cut = RenderComponent<FluentCxMediaQuery>(a =>
        {
            a.Add(p => p.Query, "(max-width: 1280px)");
        });
        await cut.InvokeAsync(() => { }); // Trigger re-render to ensure OnAfterRenderAsync is called

        // Assert
        JSInterop.VerifyInvoke("import");
        mockModule.VerifyInvoke("initialize");
    }

    [Fact]
    public async Task FluentCxDeviceDetector_DisposeAsync_DisposesModule()
    {
        // Arrange
        var cut = RenderComponent<FluentCxMediaQuery>();
        var moduleMock = new NonThrowingJSObjectReference();
        var moduleField = typeof(FluentCxMediaQuery).GetField("_module", BindingFlags.NonPublic | BindingFlags.Instance);
        moduleField?.SetValue(cut.Instance, moduleMock);

        // Act
        await cut.Instance.DisposeAsync();

        // Assert
        Assert.True(moduleMock.IsDisposed);
    }

    [Fact]
    public async Task FluentCxDeviceDetector_DisposeAsync_HandlesJSDisconnectedException()
    {
        // Arrange
        var cut = RenderComponent<FluentCxMediaQuery>();
        var moduleMock = new ThrowingJSObjectReference();
        var moduleField = typeof(FluentCxMediaQuery).GetField("_module", BindingFlags.NonPublic | BindingFlags.Instance);
        moduleField?.SetValue(cut.Instance, moduleMock);

        // Act & Assert (should not throw)
        await cut.Instance.DisposeAsync();
    }
}
