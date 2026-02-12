using System.Reflection;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialItemTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var item = new SleekDialItem(new LibraryConfiguration());

        Assert.False(item.Disabled);
        Assert.True(item.IsVisible);
        Assert.Null(item.Text);
        Assert.Null(item.Title);
    }

    [Fact]
    public async Task OnClickAsync_DoesNotThrow_WhenNoDelegate()
    {
        var item = new SleekDialItem(new LibraryConfiguration());

        await item.OnClickAsync();
    }

    [Fact]
    public async Task OnClickAsync_InvokesDelegate()
    {
        var item = new SleekDialItem(new LibraryConfiguration());
        var clicked = false;

        var onClickProperty = typeof(SleekDialItem).GetProperty("OnClick", BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(onClickProperty);

        onClickProperty!.SetValue(item, new EventCallbackFactory().Create(this, () => clicked = true));

        await item.OnClickAsync();

        Assert.True(clicked);
    }
}
