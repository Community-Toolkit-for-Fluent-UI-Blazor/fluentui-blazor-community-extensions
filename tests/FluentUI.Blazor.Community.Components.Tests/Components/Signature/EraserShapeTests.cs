using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class EraserShapeTests
{
    [Fact]
    public void EraserShape_ShouldContain_Circle_And_Square()
    {
        Assert.Contains(EraserShape.Circle, (EraserShape[])System.Enum.GetValues(typeof(EraserShape)));
        Assert.Contains(EraserShape.Square, (EraserShape[])System.Enum.GetValues(typeof(EraserShape)));
    }
}
