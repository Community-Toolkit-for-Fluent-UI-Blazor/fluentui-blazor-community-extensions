using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialItemTests
{
    [Fact]
    public void Dispose_Calls_RemoveChild_On_Parent()
    {
        // Arrange
        var parent = new FluentCxSleekDial();
        var item = CreateItemWithParent(parent);

        // Act
        item.Dispose();

        // Assert
        Assert.Empty(parent.InternalItems);
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
        var parent = new FluentCxSleekDial();
        var item = CreateItemWithParent(parent);

        item.TestOnInitialized();

        Assert.Single(parent.InternalItems);
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
    public static void TestOnInitialized(this SleekDialItem item)
    {
        var method = typeof(SleekDialItem).GetMethod("OnInitialized", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(item, null);
    }

    public static Task TestOnAfterRenderAsync(this SleekDialItem item, bool firstRender)
    {
        var method = typeof(SleekDialItem).GetMethod("OnAfterRenderAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (Task)method.Invoke(item, new object[] { firstRender });
    }
}
