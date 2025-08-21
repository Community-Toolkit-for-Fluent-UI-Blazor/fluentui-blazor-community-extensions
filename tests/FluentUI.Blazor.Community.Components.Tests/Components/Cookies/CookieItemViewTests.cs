using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class CookieItemViewTests
    : TestBase
{
    public CookieItemViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<FileManagerState>();
        Services.AddScoped<DeviceInfoState>();
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void ComponentRendersDividerWhenShowDividerIsTrue()
    {
        // Arrange
        var cut = RenderComponent<CookieItemView>(parameters => parameters
            .Add(p => p.ShowDivider, true)
            .Add(p => p.Item, new CookieItem()));

        // Act
        var divider = cut.Markup.Contains("fluent-divider");

        // Assert
        Assert.True(divider);
    }

    [Fact]
    public void ComponentDisplaysCorrectLabelsForMultipleLanguages()
    {
        // Arrange
        var cut = RenderComponent<CookieItemView>(parameters => parameters
            .Add(p => p.AcceptLabel, "Aceptar")
            .Add(p => p.DeclineLabel, "Rechazar")
            .Add(p => p.Item, new CookieItem()));

        // Act
        var acceptButton = cut.Markup.Contains("Aceptar");
        var declineButton = cut.Markup.Contains("Rechazar");

        // Assert
        Assert.True(acceptButton);
        Assert.True(declineButton);
    }

    [Fact]
    public void Constructor_Default()
    {
        // Act
        var cut = new CookieItemView();

        // Assert
        Assert.False(string.IsNullOrEmpty(cut.Id));
    }

    [Fact]
    public void Parameters_AreSetCorrectly()
    {
        // Arrange
        var item = new CookieItem { Name = "Test" };
        var template = (RenderFragment<CookieItem>)(item => builder => builder.AddContent(0, "template"));
        var acceptLabel = "Accepter";
        var declineLabel = "Refuser";

        // Act
        var cut = RenderComponent<CookieItemView>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.Template, template)
            .Add(p => p.ShowDivider, true)
            .Add(p => p.AcceptLabel, acceptLabel)
            .Add(p => p.DeclineLabel, declineLabel)
        );

        // Assert
        Assert.Equal(item, cut.Instance.Item);
        Assert.Equal(template, cut.Instance.Template);
        Assert.True(cut.Instance.ShowDivider);
        Assert.Equal(acceptLabel, cut.Instance.AcceptLabel);
        Assert.Equal(declineLabel, cut.Instance.DeclineLabel);
    }

    [Fact]
    public async Task OnActivationChangedAsync_InvokesEventCallback_WhenDelegateSet()
    {
        // Arrange
        var called = false;

        var cut = RenderComponent<CookieItemView>(p =>
            p.Add(m => m.OnActivationChanged, EventCallback.Factory.Create(this, () =>
            {
                called = true;
                return Task.CompletedTask;
            })));

        // Act
        var task = cut.Instance.GetType().GetMethod("OnActivationChangedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        Assert.True(called);
    }
}
