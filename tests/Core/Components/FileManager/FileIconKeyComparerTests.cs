using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileIconKeyComparerTests
{
    [Fact]
    public void Equals_IsCaseInsensitiveForKey()
    {
        var comparer = FileIconKeyComparer.Instance;

        var left = ("KEY", FileView.List);
        var right = ("key", FileView.List);

        Assert.True(comparer.Equals(left, right));
    }

    [Fact]
    public void Equals_ConsidersView()
    {
        var comparer = FileIconKeyComparer.Instance;

        var left = ("key", FileView.List);
        var right = ("key", FileView.Details);

        Assert.False(comparer.Equals(left, right));
    }

    [Fact]
    public void GetHashCode_IsStableForCaseInsensitiveKeys()
    {
        var comparer = FileIconKeyComparer.Instance;

        Assert.Equal(comparer.GetHashCode(("key", FileView.List)), comparer.GetHashCode(("KEY", FileView.List)));
    }
}
