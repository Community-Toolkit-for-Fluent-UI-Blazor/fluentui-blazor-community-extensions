using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lootie;

public class LootieLabelsTests
{
    [Fact]
    public void Default_Should_Have_English_Labels()
    {
        var labels = LootieLabels.Default;

        Assert.Equal("Pause", labels.Pause);
        Assert.Equal("Play", labels.Play);
        Assert.Equal("Stop", labels.Stop);
        Assert.Equal("Loop", labels.Loop);
        Assert.Equal("Speed", labels.Speed);
        Assert.Equal("Direction", labels.Direction);
    }

    [Fact]
    public void French_Should_Have_French_Labels()
    {
        var labels = LootieLabels.French;

        Assert.Equal("Pause", labels.Pause);
        Assert.Equal("Lecture", labels.Play);
        Assert.Equal("Arrêter", labels.Stop);
        Assert.Equal("Boucle", labels.Loop);
        Assert.Equal("Vitesse", labels.Speed);
        Assert.Equal("Direction", labels.Direction);
    }

    [Fact]
    public void Can_Create_Custom_Labels()
    {
        var custom = new LootieLabels
        {
            Pause = "P",
            Play = "L",
            Stop = "S",
            Loop = "B",
            Speed = "V",
            Direction = "D"
        };

        Assert.Equal("P", custom.Pause);
        Assert.Equal("L", custom.Play);
        Assert.Equal("S", custom.Stop);
        Assert.Equal("B", custom.Loop);
        Assert.Equal("V", custom.Speed);
        Assert.Equal("D", custom.Direction);
    }
}
