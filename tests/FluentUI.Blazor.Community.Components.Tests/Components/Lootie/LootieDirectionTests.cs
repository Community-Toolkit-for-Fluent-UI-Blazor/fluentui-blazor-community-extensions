using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lootie;
public class LootieDirectionTests
{
    [Fact]
    public void Forward_Should_Have_Value_One()
    {
        Assert.Equal(1, (int)LootieDirection.Forward);
    }

    [Fact]
    public void Backward_Should_Have_Value_MinusOne()
    {
        Assert.Equal(-1, (int)LootieDirection.Backward);
    }

    [Theory]
    [InlineData(1, LootieDirection.Forward)]
    [InlineData(-1, LootieDirection.Backward)]
    public void Can_Parse_Int_To_LootieDirection(int value, LootieDirection expected)
    {
        var direction = (LootieDirection)value;
        Assert.Equal(expected, direction);
    }

    [Theory]
    [InlineData("Forward", LootieDirection.Forward)]
    [InlineData("Backward", LootieDirection.Backward)]
    public void Can_Parse_Name_To_LootieDirection(string name, LootieDirection expected)
    {
        var parsed = (LootieDirection)Enum.Parse(typeof(LootieDirection), name);
        Assert.Equal(expected, parsed);
    }
}
