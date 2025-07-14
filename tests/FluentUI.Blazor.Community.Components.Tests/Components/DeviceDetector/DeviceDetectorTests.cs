using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using OperatingSystem = FluentUI.Blazor.Community.Components.OperatingSystem;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;

public class DeviceDetectorTests : TestBase
{
    public DeviceDetectorTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<DeviceInfoState>();
    }

    [Fact]
    public void FluentCxDeviceDetector_Default()
    {
        // Arrange
        var cut = RenderComponent<FluentCxDeviceDetector>();

        // Act

        // Assert
        Assert.NotNull(cut.Instance);
        Assert.Null(cut.Instance.DeviceInfo);
    }

    [Fact]
    public void FluentCxDeviceDetector_WithChildContent()
    {
        // Arrange
        var cut = RenderComponent<FluentCxDeviceDetector>(parameters =>
        {
            parameters.AddChildContent("<div id=\"test-content\">Test Content</div>");
        });

        // Act

        // Assert
        var testContent = cut.Find("#test-content");
        Assert.NotNull(testContent);
        Assert.Equal("Test Content", testContent.TextContent);
    }

    [Fact]
    public void FluentCxDeviceDetector_CascadingValue()
    {
        // Arrange
        var cut = RenderComponent<FluentCxDeviceDetector>(parameters =>
        {
            parameters.AddChildContent(builder =>
            {
                builder.OpenComponent<TestChildComponent>(0);
                builder.CloseComponent();
            });
        });

        // Act

        // Assert
        var childComponent = cut.FindComponent<TestChildComponent>();
        Assert.NotNull(childComponent.Instance.DeviceDetector);
        Assert.Same(cut.Instance, childComponent.Instance.DeviceDetector);
    }

    [Fact]
    public async Task FluentCxDeviceDetector_AfterRender_InitializesJavaScript()
    {
        // Arrange
        var mockDeviceInfo = new DeviceInfo
        {
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            Browser = Browser.Chrome,
            OperatingSystem = OperatingSystem.Windows10,
            Touch = false,
            Mobile = Mobile.NotMobileDevice,
            IsTablet = false
        };

        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/DeviceDetector/FluentCxDeviceDetector.razor.js");
        mockModule.Setup<DeviceInfo>("getDeviceInfo").SetResult(mockDeviceInfo);
        mockModule.Setup<bool>("getDeviceOrientation").SetResult(false);

        // Act
        var cut = RenderComponent<FluentCxDeviceDetector>();
        await cut.InvokeAsync(() => { }); // Trigger re-render to ensure OnAfterRenderAsync is called

        // Assert
        JSInterop.VerifyInvoke("import");
        mockModule.VerifyInvoke("getDeviceInfo");
        mockModule.VerifyInvoke("getDeviceOrientation");
    }

    [Fact]
    public async Task FluentCxDeviceDetector_ChangeOrientation_NullDeviceInfo()
    {
        // Arrange
        var cut = RenderComponent<FluentCxDeviceDetector>();

        // Act & Assert (should not throw)
        await cut.Instance.ChangeOrientation(true);
        await cut.Instance.ChangeOrientation(false);
        Assert.Null(cut.Instance.DeviceInfo);
    }

    [Fact]
    public async Task FluentCxDeviceDetector_JSInvokable_ChangeOrientation()
    {
        // Arrange
        var mockDeviceInfo = new DeviceInfo
        {
            Browser = Browser.Chrome,
            OperatingSystem = OperatingSystem.Windows10,
            Mobile = Mobile.NotMobileDevice,
            Orientation = DeviceOrientation.Landscape
        };

        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/DeviceDetector/FluentCxDeviceDetector.razor.js");
        mockModule.Setup<DeviceInfo>("getDeviceInfo").SetResult(mockDeviceInfo);
        mockModule.Setup<bool>("getDeviceOrientation").SetResult(false);

        var cut = RenderComponent<FluentCxDeviceDetector>();
        await cut.InvokeAsync(() => { });

        // Act - Simulate JavaScript calling the JSInvokable method
        await cut.Instance.ChangeOrientation(true);

        // Assert
        Assert.Equal(DeviceOrientation.Portrait, cut.Instance.DeviceInfo?.Orientation);
    }

    [Fact]
    public async Task FluentCxDeviceDetector_DeviceInfo_PopulatedAfterRender()
    {
        // Arrange
        var expectedDeviceInfo = new DeviceInfo
        {
            UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X)",
            Browser = Browser.Safari,
            OperatingSystem = OperatingSystem.Mac,
            Touch = true,
            Mobile = Mobile.IPhone,
            IsTablet = false,
            Orientation = DeviceOrientation.Portrait
        };

        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/DeviceDetector/FluentCxDeviceDetector.razor.js");
        mockModule.Setup<DeviceInfo>("getDeviceInfo").SetResult(expectedDeviceInfo);
        mockModule.Setup<bool>("getDeviceOrientation").SetResult(true);

        // Act
        var cut = RenderComponent<FluentCxDeviceDetector>();
        await cut.InvokeAsync(() => { });

        // Assert
        Assert.NotNull(cut.Instance.DeviceInfo);
        Assert.Equal(expectedDeviceInfo.Browser, cut.Instance.DeviceInfo.Browser);
        Assert.Equal(expectedDeviceInfo.OperatingSystem, cut.Instance.DeviceInfo.OperatingSystem);
        Assert.Equal(expectedDeviceInfo.Mobile, cut.Instance.DeviceInfo.Mobile);
        Assert.Equal(expectedDeviceInfo.Touch, cut.Instance.DeviceInfo.Touch);
        Assert.Equal(expectedDeviceInfo.IsTablet, cut.Instance.DeviceInfo.IsTablet);
    }

    [Fact]
    public async Task FluentCxDeviceDetector_DisposeAsync_DisposesModule()
    {
        // Arrange
        var cut = RenderComponent<FluentCxDeviceDetector>();
        var moduleMock = new NonThrowingJSObjectReference();
        var moduleField = typeof(FluentCxDeviceDetector).GetField("_module", BindingFlags.NonPublic | BindingFlags.Instance);
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
        var cut = RenderComponent<FluentCxDeviceDetector>();
        var moduleMock = new ThrowingJSObjectReference();
        var moduleField = typeof(FluentCxDeviceDetector).GetField("_module", BindingFlags.NonPublic | BindingFlags.Instance);
        moduleField?.SetValue(cut.Instance, moduleMock);

        // Act & Assert (should not throw)
        await cut.Instance.DisposeAsync();
    }
}

// Test helper component to verify cascading value
public class TestChildComponent : ComponentBase
{
    [CascadingParameter]
    public FluentCxDeviceDetector? DeviceDetector { get; set; }

    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        builder.AddContent(0, "Test child component");
    }
}

// Helper for simulating JSDisconnectedException
public class ThrowingJSObjectReference : IJSObjectReference
{
    public bool IsDisposed { get; private set; }
    public ValueTask DisposeAsync()
    {
        IsDisposed = true;
        throw new JSDisconnectedException("Simulated disconnect");
    }
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw new NotImplementedException();
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
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw new NotImplementedException();
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new NotImplementedException();
}
