using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class AudioControlsTests : TestBase
{
    public AudioControlsTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Constructor_SetsId()
    {
        var cut = RenderComponent<AudioControls>();
        Assert.StartsWith("audio-controls-", cut.Instance.Id);
        Assert.False(string.IsNullOrWhiteSpace(cut.Instance.Id));
    }

    [Fact]
    public void IsDownloadVisible_DefaultIsTrue()
    {
        var cut = RenderComponent<AudioControls>();
        Assert.True(cut.Instance.IsDownloadVisible);
    }

    [Fact]
    public void IsDownloadVisible_CanBeSet()
    {
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.IsDownloadVisible, false));
        Assert.False(cut.Instance.IsDownloadVisible);
    }

    [Fact]
    public async Task OnShuffleChanged_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnShuffleChanged, EventCallback.Factory.Create<bool>(this, _ => called = true)));
        await cut.Instance.OnShuffleChanged.InvokeAsync(true);
        Assert.True(called);
    }

    [Fact]
    public async Task OnPlaylistToggled_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnPlaylistToggled, EventCallback.Factory.Create<bool>(this, _ => called = true)));
        await cut.Instance.OnPlaylistToggled.InvokeAsync(true);
        Assert.True(called);
    }

    [Fact]
    public async Task OnPrevious_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnPrevious, EventCallback.Factory.Create(this, () => called = true)));
        await cut.Instance.OnPrevious.InvokeAsync();
        Assert.True(called);
    }

    [Fact]
    public async Task OnDownload_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnDownload, EventCallback.Factory.Create(this, () => called = true)));
        await cut.Instance.OnDownload.InvokeAsync();
        Assert.True(called);
    }

    [Fact]
    public async Task OnStop_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnStop, EventCallback.Factory.Create(this, () => called = true)));
        await cut.Instance.OnStop.InvokeAsync();
        Assert.True(called);
    }

    [Fact]
    public async Task OnNext_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnNext, EventCallback.Factory.Create(this, () => called = true)));
        await cut.Instance.OnNext.InvokeAsync();
        Assert.True(called);
    }

    [Fact]
    public async Task OnRepeatModeChanged_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnRepeatModeChanged, EventCallback.Factory.Create<AudioVideoRepeatMode>(this, _ => called = true)));
        await cut.Instance.OnRepeatModeChanged.InvokeAsync(AudioVideoRepeatMode.SingleLoop);
        Assert.True(called);
    }

    [Fact]
    public async Task OnPlayPauseToggled_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnPlayPauseToggled, EventCallback.Factory.Create<bool>(this, _ => called = true)));
        await cut.Instance.OnPlayPauseToggled.InvokeAsync(true);
        Assert.True(called);
    }

    [Fact]
    public async Task OnVolumeChanged_IsInvoked()
    {
        bool called = false;
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.OnVolumeChanged, EventCallback.Factory.Create<double>(this, _ => called = true)));
        await cut.Instance.OnVolumeChanged.InvokeAsync(0.5);
        Assert.True(called);
    }

    [Fact]
    public void IsPreviousDisabled_CanBeSet()
    {
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.IsPreviousDisabled, true));
        Assert.True(cut.Instance.IsPreviousDisabled);
    }

    [Fact]
    public void IsNextDisabled_CanBeSet()
    {
        var cut = RenderComponent<AudioControls>(p => p.Add(a => a.IsNextDisabled, true));
        Assert.True(cut.Instance.IsNextDisabled);
    }
}
