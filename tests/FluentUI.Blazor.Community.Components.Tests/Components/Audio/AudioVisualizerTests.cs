using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class AudioVisualizerTests : TestBase
{
    public AudioVisualizerTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void AudioVisualizer_Render_Default()
    {
        // Arrange & Act
        var cut = RenderComponent<AudioVisualizer>(p => p.Add(x => x.Anchor, "a123456"));

        // Assert
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void OnInitialized_ThrowsIfAnchorIsNullOrWhitespace()
    {
        // Anchor null
        var ex1 = Assert.Throws<InvalidOperationException>(() =>
            RenderComponent<AudioVisualizer>(p => p.Add(a => a.Anchor, null))
        );
        Assert.Contains("Anchor parameter is required", ex1.Message);

        // Anchor vide
        var ex2 = Assert.Throws<InvalidOperationException>(() =>
            RenderComponent<AudioVisualizer>(p => p.Add(a => a.Anchor, " "))
        );
        Assert.Contains("Anchor parameter is required", ex2.Message);
    }

    [Fact]
    public void Constructor_SetsId()
    {
        var cut = RenderComponent<AudioVisualizer>(p => p.Add(a => a.Anchor, "audio1"));
        Assert.False(string.IsNullOrWhiteSpace(cut.Instance.Id));
    }

    [Fact]
    public void OnAfterRenderAsync_ImportsJsAndInitializesVisualizer()
    {
        // Arrange
        var jsModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/AudioVideo/Audio/AudioVisualizer.razor.js");
        jsModule.SetupVoid("fluentCxAudioVisualizer.initialize");

        var cut = RenderComponent<AudioVisualizer>(p => p
            .Add(a => a.Anchor, "audio1")
            .Add(a => a.Mode, VisualizerMode.Spectrum)
            .Add(a => a.Cover, "cover.png")
            .Add(a => a.Width, 400)
            .Add(a => a.Height, 200)
        );

        // Assert
        JSInterop.VerifyInvoke("import");
        jsModule.VerifyInvoke("fluentCxAudioVisualizer.initialize");
    }

    [Fact]
    public async Task DisposeAsync_CallsJsDisposeAndDisposesModule()
    {
        // Arrange
        var jsModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/AudioVideo/Audio/AudioVisualizer.razor.js");
        jsModule.SetupVoid("fluentCxAudioVisualizer.dispose");

        var cut = RenderComponent<AudioVisualizer>(p => p.Add(a => a.Anchor, "audio1"));

        // Act
        await cut.Instance.DisposeAsync();

        // Assert
        jsModule.VerifyInvoke("fluentCxAudioVisualizer.dispose");
    }

    [Fact]
    public async Task DisposeAsync_HandlesJsDisconnectedException()
    {
        // Arrange
        var cut = RenderComponent<AudioVisualizer>(p => p.Add(a => a.Anchor, "audio1"));
        var moduleField = typeof(AudioVisualizer).GetField("_module", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        moduleField?.SetValue(cut.Instance, new ThrowingJSObjectReference());

        // Act & Assert (ne doit pas lever d'exception)
        await cut.Instance.DisposeAsync();
    }

    // Helper pour simuler JSDisconnectedException
    private class ThrowingJSObjectReference : IJSObjectReference
    {
        public ValueTask DisposeAsync()
        {
            throw new JSDisconnectedException("Simulated disconnect");
        }
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => ValueTask.FromResult<TValue>(default!);
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, System.Threading.CancellationToken cancellationToken, object?[]? args) => throw new NotImplementedException();
    }

    [Fact]
    public void AudioVisualizer_JSInterop_ModuleIsLoaded()
    {
        // Arrange
        var jsModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Audio/AudioVisualizer.razor.js");

        // Act
        var cut = RenderComponent<AudioVisualizer>(p => p.Add(x => x.Anchor, "a123456"));

        // Assert
        JSInterop.VerifyInvoke("import");
    }
}
