using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class WatermarkVerticalAlignmentTests
{
    [Fact]
    public void Enum_Should_Have_Expected_Values()
    {
        Assert.Equal(0, (int)WatermarkVerticalAlignment.Start);
        Assert.Equal(1, (int)WatermarkVerticalAlignment.Middle);
        Assert.Equal(2, (int)WatermarkVerticalAlignment.End);
    }
}
