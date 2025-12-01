using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class MappedItemRectTests
{
    [Fact]
    public void DefaultValues_AreZeroAndFalse()
    {
        var sut = new MappedItemRect();

        Assert.Equal(new RectangleF(), sut.Rect);
        Assert.False(sut.ShowLeftAnchor);
        Assert.False(sut.ShowRightAnchor);
        Assert.False(sut.ShowTopAnchor);
        Assert.False(sut.ShowBottomAnchor);
    }

    [Fact]
    public void Equality_SameValues_AreEqual()
    {
        var rect = new RectangleF(1f, 2f, 3f, 4f);
        var a = new MappedItemRect { Rect = rect, ShowLeftAnchor = true, ShowRightAnchor = false, ShowTopAnchor = true, ShowBottomAnchor = false };
        var b = new MappedItemRect { Rect = rect, ShowLeftAnchor = true, ShowRightAnchor = false, ShowTopAnchor = true, ShowBottomAnchor = false };

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void WithExpression_ProducesNewInstance_OriginalUnchanged()
    {
        var rect = new RectangleF(0, 0, 10, 10);
        var original = new MappedItemRect { Rect = rect, ShowLeftAnchor = false, ShowRightAnchor = false, ShowTopAnchor = false, ShowBottomAnchor = false };

        var modified = original with { ShowBottomAnchor = true, Rect = new RectangleF(1, 1, 11, 11) };

        // original unchanged
        Assert.False(original.ShowBottomAnchor);
        Assert.Equal(rect, original.Rect);

        // modified reflects changes
        Assert.True(modified.ShowBottomAnchor);
        Assert.Equal(new RectangleF(1, 1, 11, 11), modified.Rect);
        Assert.NotEqual(original, modified);
    }

    [Fact]
    public void ToString_ContainsPropertyNamesAndValues()
    {
        var rect = new RectangleF(2f, 3f, 4f, 5f);
        var sut = new MappedItemRect { Rect = rect, ShowLeftAnchor = true, ShowTopAnchor = true };

        var s = sut.ToString() ?? string.Empty;

        Assert.Contains("Rect", s, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ShowLeftAnchor", s, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ShowTopAnchor", s, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2", s); // numeric presence check
        Assert.Contains("3", s);
    }
}
