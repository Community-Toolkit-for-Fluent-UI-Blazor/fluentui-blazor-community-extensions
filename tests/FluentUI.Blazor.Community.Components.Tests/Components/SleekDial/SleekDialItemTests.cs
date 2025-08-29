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
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialItemTests
    : TestBase
{
    public SleekDialItemTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void Dispose_Calls_RemoveChild_On_Parent()
    {
        var comp = RenderComponent<FluentCxSleekDial>(
            p => p.AddChildContent<SleekDialItem>());

        // Act
        var item = comp.FindComponent<SleekDialItem>().Instance;
        item?.Dispose();

        // Assert
        Assert.Empty(comp.Instance.InternalItems);
    }

    [Fact]
    public void Index_Returns_Correct_Value_When_Parent_Is_Null()
    {
        var item = new SleekDialItem();
        Assert.Equal(-1, GetIndex(item));
    }

    [Fact]
    public void Index_Returns_Correct_Value_When_Parent_Is_Set()
    {
        var parent = new FluentCxSleekDial();
        var item = CreateItemWithParent(parent);
        parent.InternalItems.Add(item);

        Assert.Equal(0, GetIndex(item));
    }

    [Fact]
    public void OnInitialized_Calls_AddChild_On_Parent()
    {
        var comp = RenderComponent<FluentCxSleekDial>(
            p => p.AddChildContent<SleekDialItem>());
       
        Assert.Single(comp.Instance.InternalItems);
    }

    [Fact]
    public async Task OnClickAsync_Invokes_OnClick_If_Delegate()
    {
        bool called = false;
        var item = new SleekDialItem
        {
            OnClick = new EventCallback(null, () => { called = true; return Task.CompletedTask; })
        };

        await item.OnClickAsync();

        Assert.True(called);
    }

    [Fact]
    public async Task OnClickAsync_DoesNothing_If_No_Delegate()
    {
        var item = new SleekDialItem
        {
            OnClick = default
        };

        // Should not throw
        await item.OnClickAsync();
    }

    [Fact]
    public void IsVisible_Raise_IsVisibleChanged_Callback()
    {
        var called = false;

        var comp = RenderComponent<SleekDialItem>(
            parameters => parameters.Add(p => p.IsVisibleChanged, new EventCallback<bool>(null, () => { called = true; }))
                                    .Add(p => p.IsVisible, false));

        Assert.True(called);
    }

    // Helpers

    private static SleekDialItem CreateItemWithParent(FluentCxSleekDial parent)
    {
        var item = new SleekDialItem();
        typeof(SleekDialItem)
            .GetProperty("Parent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(item, parent);
        return item;
    }

    private static int GetIndex(SleekDialItem item)
    {
        return (int)typeof(SleekDialItem)
            .GetProperty("Index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty)!
            .GetValue(item)!;
    }
}

public static class SleekDialItemTestExtensions
{
    public static Task TestOnAfterRenderAsync(this SleekDialItem item, bool firstRender)
    {
        var method = typeof(SleekDialItem).GetMethod("OnAfterRenderAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (Task)method.Invoke(item, new object[] { firstRender });
    }
}
