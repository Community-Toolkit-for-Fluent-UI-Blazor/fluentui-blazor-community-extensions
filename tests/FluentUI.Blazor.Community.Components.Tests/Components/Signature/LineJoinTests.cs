using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class LineJoinTests
{
    [Fact]
    public void LineJoin_Should_Have_Expected_Values()
    {
        Assert.Equal(0, (int)LineJoin.Miter);
        Assert.Equal(1, (int)LineJoin.Round);
        Assert.Equal(2, (int)LineJoin.Bevel);
    }
}
