using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat.Slideshow;

public class ChatMessageSlideShowItemDocumentTests : TestBase
{
    private class EmptyItem
    {
    }

    public ChatMessageSlideShowItemDocumentTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void Constructor_SetsId()
    {
        var component = new ChatMessageSlideShowItemDocument<EmptyItem>();
        Assert.False(string.IsNullOrEmpty(component.Id));
    }

    [Fact]
    public void OnAfterRenderAsync_FirstRender_VideoBinaryChatFile_LoadsContentAndInvokesJs()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Chat/Slideshow/ChatMessageSlideShowItemDocument.razor.js");
        mockModule.SetupVoid("loadVideo", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()).SetVoidResult();

        var comp = RenderComponent<ChatMessageSlideShowItemDocument<EmptyItem>>(parameters =>
        {
            parameters.Add(p => p.Item, new BinaryChatFile()
            {
                ContentType = "video/mp4",
                Data = new byte[] { 0x00, 0x01, 0x02, 0x03 }
            });
        });

        mockModule.VerifyInvoke("loadVideo");
    }

    [Fact]
    public void OnAfterRenderAsync_FirstRender_AudioBinaryChatFile_LoadsContentAndInvokesJs()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Chat/Slideshow/ChatMessageSlideShowItemDocument.razor.js");

        var comp = RenderComponent<ChatMessageSlideShowItemDocument<EmptyItem>>(parameters =>
        {
            parameters.Add(p => p.Item, new BinaryChatFile()
            {
                ContentType = "audio/mpeg",
                Data = new byte[] { 0x00, 0x01, 0x02, 0x03 }
            });
        });

        mockModule.VerifyNotInvoke("import");
        mockModule.VerifyNotInvoke("loadVideo");
    }

    [Fact]
    public void OnAfterRenderAsync_FirstRender_UrlChatFile_LoadsContent()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Chat/Slideshow/ChatMessageSlideShowItemDocument.razor.js");

        var comp = RenderComponent<ChatMessageSlideShowItemDocument<EmptyItem>>(parameters =>
        {
            parameters.Add(p => p.Item, new UrlChatFile()
            {
                ContentType = "html/txt",
                Url = "https://example.com"
            });
        });

        mockModule.VerifyNotInvoke("import");
        mockModule.VerifyNotInvoke("loadVideo");
    }

    [Fact]
    public async Task OnAfterRenderAsync_NotFirstRender_DoesNothing()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Chat/Slideshow/ChatMessageSlideShowItemDocument.razor.js");

        var comp = RenderComponent<ChatMessageSlideShowItemDocument<EmptyItem>>(parameters =>
        {
            parameters.Add(p => p.Item, new UrlChatFile()
            {
                ContentType = "html/txt",
                Url = "https://example.com"
            });
        });

        await comp.InvokeAsync(() => { });

        mockModule.VerifyNotInvoke("import");
    }

    [Fact]
    public async Task DisposeAsync_WithModule_DisposesModule()
    {
        var jsModule = new Mock<IJSObjectReference>();
        jsModule.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var comp = RenderComponent<ChatMessageSlideShowItemDocument<EmptyItem>>(parameters =>
        {
            parameters.Add(p => p.Item, new BinaryChatFile()
            {
                ContentType = "video/mp4",
                Data = [0x00, 0x01, 0x02, 0x03]
            });
        });

        typeof(ChatMessageSlideShowItemDocument<EmptyItem>)
            .GetField("_module", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(comp.Instance, jsModule.Object);

        await comp.Instance.DisposeAsync();

        jsModule.Verify(m => m.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_WithoutModule_DoesNothing()
    {
        var comp = RenderComponent<ChatMessageSlideShowItemDocument<EmptyItem>>(parameters =>
        {
            parameters.Add(p => p.Item, new BinaryChatFile()
            {
                ContentType = "audio/mp3",
                Data = [0x00, 0x01, 0x02, 0x03]
            });
        });

        await comp.Instance.DisposeAsync();
    }
}
