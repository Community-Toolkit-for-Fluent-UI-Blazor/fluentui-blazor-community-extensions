using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components;

public class PopupAnimationTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)PopupAnimation.None);
        Assert.Equal(1, (int)PopupAnimation.Slide);
        Assert.Equal(2, (int)PopupAnimation.SlideScale);
        Assert.Equal(3, (int)PopupAnimation.Fade);
        Assert.Equal(4, (int)PopupAnimation.Scale);
        Assert.Equal(5, (int)PopupAnimation.FadeScale);
        Assert.Equal(6, (int)PopupAnimation.Custom);
    }
}
