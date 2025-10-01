using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class DownloadButtonTests : TestBase
{
    public DownloadButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void DownloadLabel_DefaultValue_IsDownload()
    {
        // Arrange & Act
        var cut = RenderComponent<DownloadButton>();

        // Assert
        Assert.Equal("Download", cut.Instance.Label);
    }

    [Fact]
    public void Constructor_SetsUniqueId()
    {
        // Arrange & Act
        var cut = RenderComponent<DownloadButton>();

        // Assert
        Assert.StartsWith("download-button-", cut.Instance.Id);
        Assert.False(string.IsNullOrWhiteSpace(cut.Instance.Id));
    }

    [Fact]
    public async Task OnDownloadAsync_InvokesCallback_WhenDelegateSet()
    {
        // Arrange
        var wasCalled = false;
        var cut = RenderComponent<DownloadButton>(parameters => parameters
            .Add(p => p.OnDownload, EventCallback.Factory.Create(this, () => wasCalled = true))
        );

        // Act
        await (Task)cut.Instance.GetType()
            .GetMethod("OnClickAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, null);

        // Assert
        Assert.True(wasCalled);
    }

    [Fact]
    public async Task OnDownloadAsync_DoesNothing_WhenNoDelegate()
    {
        // Arrange
        var cut = RenderComponent<DownloadButton>();

        // Act & Assert (should not throw)
        await (Task)cut.Instance.GetType()
            .GetMethod("OnClickAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, null);
    }
}
