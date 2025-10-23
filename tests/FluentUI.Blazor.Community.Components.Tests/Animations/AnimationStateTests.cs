using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;
public class AnimationStateTests
{
    private class DummyInterpolator : IInterpolator<double>
    {
        public double Lerp(double start, double end, double amount) => start + (end - start) * amount;
    }

    [Fact]
    public void Interpolate_ReturnsStartValue_WhenNowEqualsStartTime()
    {
        var state = new AnimationState<double>
        {
            StartValue = 10,
            EndValue = 20,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = DateTime.Now
        };
        var interpolator = new DummyInterpolator();
        var result = state.Interpolate(state.StartTime, interpolator);
        Assert.Equal(10, result);
    }

    [Fact]
    public void Interpolate_ReturnsEndValue_WhenNowEqualsStartTimePlusDuration()
    {
        var state = new AnimationState<double>
        {
            StartValue = 10,
            EndValue = 20,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = DateTime.Now
        };
        var interpolator = new DummyInterpolator();
        var result = state.Interpolate(state.StartTime + state.Duration, interpolator);
        Assert.Equal(20, result);
    }

    [Fact]
    public void Apply_SetsAllProperties()
    {
        var state = new AnimationState<double>();
        var now = DateTime.Now;
        state.Apply(1, 2, now, TimeSpan.FromSeconds(5));
        Assert.Equal(1, state.StartValue);
        Assert.Equal(2, state.EndValue);
        Assert.Equal(now, state.StartTime);
        Assert.Equal(TimeSpan.FromSeconds(5), state.Duration);
    }
}
