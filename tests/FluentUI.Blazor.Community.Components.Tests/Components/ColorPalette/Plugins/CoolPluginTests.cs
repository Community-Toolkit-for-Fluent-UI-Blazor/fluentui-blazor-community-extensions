using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;

public class CoolPluginTests
{
    [Fact]
    public void Name_ShouldReturnCool()
    {
        var plugin = new CoolPlugin();
        Assert.Equal("Cool", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        var plugin = new CoolPlugin();
        var options = new GenerationOptions();
        Assert.Throws<ArgumentException>(() => plugin.Generate("invalid", 3, options));
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        var plugin = new CoolPlugin();
        var options = new GenerationOptions();
        var result = plugin.Generate("#0000FF", 5, options);
        Assert.Equal(5, result.Count);
    }
}
