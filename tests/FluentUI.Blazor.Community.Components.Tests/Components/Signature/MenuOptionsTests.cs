using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class MenuOptionsTests
{
    [Fact]
    public void Default_Values_Should_Be_True()
    {
        var options = new MenuOptions();

        Assert.True(options.IsVisible);
        Assert.True(options.ShowExport);
        Assert.True(options.ShowUndo);
        Assert.True(options.ShowRedo);
        Assert.True(options.ShowClear);
        Assert.True(options.ShowEraser);
        Assert.True(options.ShowCustomOptions);
    }

    [Fact]
    public void Properties_Should_Be_Settable()
    {
        var options = new MenuOptions
        {
            IsVisible = false,
            ShowExport = false,
            ShowUndo = false,
            ShowRedo = false,
            ShowClear = false,
            ShowEraser = false,
            ShowCustomOptions = false
        };

        Assert.False(options.IsVisible);
        Assert.False(options.ShowExport);
        Assert.False(options.ShowUndo);
        Assert.False(options.ShowRedo);
        Assert.False(options.ShowClear);
        Assert.False(options.ShowEraser);
        Assert.False(options.ShowCustomOptions);
    }
}
