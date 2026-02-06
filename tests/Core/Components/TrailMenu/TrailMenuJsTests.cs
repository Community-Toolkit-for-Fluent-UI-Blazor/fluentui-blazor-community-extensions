using System.Reflection;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuJsTests
{
    [Fact]
    public async Task OnMutatedAsync_ResetsMeasuredCount()
    {
        var menu = new FluentCxTrailMenu(new LibraryConfiguration());
        SetPrivateField(menu, "_measuredCount", 3);

        var js = new TrailMenuJs(null!, menu);

        await js.OnMutatedAsync();

        var measuredCount = (int)GetPrivateField(menu, "_measuredCount");
        Assert.Equal(0, measuredCount);
    }

    [Fact]
    public async Task DisposeAsync_DoesNotThrowWhenUninitialized()
    {
        var menu = new FluentCxTrailMenu(new LibraryConfiguration());
        var js = new TrailMenuJs(null!, menu);

        var exception = await Record.ExceptionAsync(() => js.DisposeAsync(null).AsTask());

        Assert.Null(exception);
    }

    private static object GetPrivateField(object instance, string name)
    {
        var field = instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        return field!.GetValue(instance)!;
    }

    private static void SetPrivateField(object instance, string name, object value)
    {
        var field = instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        field!.SetValue(instance, value);
    }
}
