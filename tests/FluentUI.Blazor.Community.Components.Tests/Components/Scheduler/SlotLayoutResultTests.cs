using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SlotLayoutResultTests
{
    [Fact]
    public void Default_Instance_HasZeroColumns_And_NullItem()
    {
        var result = new SlotLayoutResult<string>();

        // Par conception le champ Item est initialisé via "default!" dans la classe source.
        // Au runtime il est null jusqu'à ce qu'on l'affecte.
        Assert.Equal(0, result.ColumnIndex);
        Assert.Equal(0, result.ColumnCount);
        Assert.Null(result.Item);
    }

    [Fact]
    public void Can_Set_And_Get_Properties()
    {
        var item = new SchedulerItem<string>
        {
            Id = 123,
            Title = "Meeting",
            Start = new DateTime(2025, 11, 24, 9, 0, 0),
            End = new DateTime(2025, 11, 24, 10, 0, 0)
        };

        var result = new SlotLayoutResult<string>
        {
            Item = item,
            ColumnIndex = 2,
            ColumnCount = 4
        };

        Assert.Same(item, result.Item);
        Assert.Equal(2, result.ColumnIndex);
        Assert.Equal(4, result.ColumnCount);
    }

    [Fact]
    public void Generic_Type_Preserved_When_Using_ValueType()
    {
        var item = new SchedulerItem<int>
        {
            Id = 7,
            Title = "Numeric",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(1),
            Data = 42
        };

        var result = new SlotLayoutResult<int>
        {
            Item = item,
            ColumnIndex = 1,
            ColumnCount = 1
        };

        Assert.Equal(42, result.Item.Data);
        Assert.Equal(1, result.ColumnIndex);
        Assert.Equal(1, result.ColumnCount);
    }
}
