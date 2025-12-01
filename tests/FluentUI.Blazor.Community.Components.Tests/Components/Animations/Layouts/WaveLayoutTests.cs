using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class WaveLayoutTests
{
    [Fact]
    public void Update_SetsWaveOffsets()
    {
        var layout = new WaveLayout { Amplitude = 20, Frequency = 0.5, Spacing = 10 };
        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, [2, 5, element]);

        var x = 2 * 10;
        var y = 20 * Math.Sin(0.5 * x);

        Assert.Equal(x, element.OffsetXState!.EndValue, 4);
        Assert.Equal(y, element.OffsetYState!.EndValue, 4);
    }
}
