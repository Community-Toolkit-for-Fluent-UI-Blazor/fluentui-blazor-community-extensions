using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMediaImporterEventArgsTest
{
    private class DummyItem { public int Id { get; set; } }

    [Fact]
    public void Constructor_InitializesItemsToEmpty()
    {
        var args = new ChatMediaImporterEventArgs<DummyItem>();
        Assert.NotNull(args.Items);
        Assert.Empty(args.Items);
    }

    [Fact]
    public void Items_SetAndGet_ReturnsExpectedValue()
    {
        var items = new List<DummyItem>
        {
            new DummyItem { Id = 1 },
            new DummyItem { Id = 2 }
        };
        var args = new ChatMediaImporterEventArgs<DummyItem>
        {
            Items = items
        };
        Assert.Equal(items, args.Items);
    }
}
