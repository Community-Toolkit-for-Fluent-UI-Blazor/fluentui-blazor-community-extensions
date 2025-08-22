using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageReplyTests : TestBase
{
    public ChatMessageReplyTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void Renders_Close_Button_When_AllowDismiss_True()
    {
        // Arrange & Act
        var cut = RenderComponent<ChatMessageReply>(parameters => parameters
            .Add(p => p.AllowDismiss, true)
        );

        // Assert
        var comp = cut.Find(".chat-reply-messagebar-close");

        Assert.NotNull(comp);
    }

    [Fact]
    public void Does_Not_Render_Close_Button_When_AllowDismiss_False()
    {
        // Arrange & Act
        var cut = RenderComponent<ChatMessageReply>(parameters => parameters
            .Add(p => p.AllowDismiss, false)
        );

        // Assert
        Assert.Throws<ElementNotFoundException>(() =>
            cut.Find(".chat-reply-messagebar-close")
        );
    }

    [Fact]
    public void Calls_OnDismissAsync_When_Close_Icon_Clicked()
    {
        // Arrange
        var dismissed = false;
        var cut = RenderComponent<ChatMessageReply>(parameters => parameters
            .Add(p => p.AllowDismiss, true)
            .Add(p => p.OnDismiss, () =>
            {
                dismissed = true;
                return Task.CompletedTask;
            })
        );

        // Act
        cut.Find(".chat-reply-messagebar-close").Click();

        // Assert
        Assert.True(dismissed);
    }
}
