using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class LineCapTests
{
    [Fact]
    public void LineCap_Should_Have_Expected_Values()
    {
        Assert.Equal(0, (int)LineCap.Butt);
        Assert.Equal(1, (int)LineCap.Round);
        Assert.Equal(2, (int)LineCap.Square);
    }
}
