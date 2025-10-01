using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class VolumeButtonTests : TestBase
{
    public VolumeButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void VolumeLabel_DefaultValue_IsVolume()
    {
        var cut = RenderComponent<VolumeButton>();
        Assert.Equal("Volume", cut.Instance.Label);
    }

    [Fact]
    public void Constructor_SetsUniqueId()
    {
        var cut = RenderComponent<VolumeButton>();
        Assert.StartsWith("volume-button-", cut.Instance.Id);
        Assert.False(string.IsNullOrWhiteSpace(cut.Instance.Id));
    }

    [Theory]
    [InlineData(0.0, "VolumeZero")]
    [InlineData(0.2, "VolumeOne")]
    [InlineData(0.6, "VolumeTwo")]
    public void VolumeIcon_ReturnsExpectedIcon(double volume, string expectedIconField)
    {
        var cut = RenderComponent<VolumeButton>();
        var instance = cut.Instance;
        var volumeField = instance.GetType().GetField("_volume", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        volumeField.SetValue(instance, volume);

        var iconProp = instance.GetType().GetProperty("VolumeIcon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var icon = iconProp.GetValue(instance);

        var expectedIcon = instance.GetType().GetField(expectedIconField, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
        Assert.Equal(expectedIcon, icon);
    }

    [Theory]
    [InlineData(1.0, "Volume (100%)")]
    [InlineData(0.5, "Volume (50%)")]
    [InlineData(0.0, "Volume (0%)")]
    public void AriaLabel_ReturnsExpectedString(double volume, string expectedLabel)
    {
        var cut = RenderComponent<VolumeButton>();
        var instance = cut.Instance;
        var volumeField = instance.GetType().GetField("_volume", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        volumeField.SetValue(instance, volume);

        var ariaLabelProp = instance.GetType().GetProperty("AriaLabel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var ariaLabel = ariaLabelProp.GetValue(instance);

        Assert.Equal(expectedLabel, ariaLabel);
    }

    [Fact]
    public async Task OnVolumeChangedAsync_InvokesCallback_WhenDelegateSet()
    {
        double? receivedValue = null;
        var cut = RenderComponent<VolumeButton>(parameters => parameters
            .Add(p => p.OnVolumeChanged, EventCallback.Factory.Create<double>(this, v => receivedValue = v))
        );

        var method = cut.Instance.GetType().GetMethod("OnVolumeChangedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method.Invoke(cut.Instance, new object[] { 0.7 });

        Assert.Equal(0.7, receivedValue);
    }

    [Fact]
    public async Task OnVolumeChangedAsync_DoesNothing_WhenNoDelegate()
    {
        var cut = RenderComponent<VolumeButton>();
        var method = cut.Instance.GetType().GetMethod("OnVolumeChangedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method.Invoke(cut.Instance, new object[] { 0.3 });
    }
}
