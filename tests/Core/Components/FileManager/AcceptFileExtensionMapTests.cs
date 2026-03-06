using System.Linq;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class AcceptFileExtensionMapTests
{
    [Fact]
    public void Resolve_ReturnsEmptyForNone()
    {
        var result = AcceptFileExtensionMap.Resolve(AcceptFileExtension.None);

        Assert.Empty(result);
    }

    [Fact]
    public void Resolve_ReturnsExpectedExtensionsInOrder()
    {
        var result = AcceptFileExtensionMap.Resolve(AcceptFileExtension.Jpg | AcceptFileExtension.Pdf).ToList();

        Assert.Equal([".jpg", ".pdf"], result);
    }
}
