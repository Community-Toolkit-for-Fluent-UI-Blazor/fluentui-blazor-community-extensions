using System.Linq;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class AcceptFileCategoryMapTests
{
    [Fact]
    public void Resolve_ReturnsEmptyForNone()
    {
        var result = AcceptFileCategoryMap.Resolve(AcceptFileCategory.None);

        Assert.Empty(result);
    }

    [Fact]
    public void Resolve_ReturnsExpectedMappings()
    {
        var result = AcceptFileCategoryMap.Resolve(AcceptFileCategory.Image | AcceptFileCategory.Document).ToList();

        var expected = new[]
        {
            "image/*",
            ".pdf",
            ".doc",
            ".docx",
            ".xls",
            ".xlsx",
            ".ppt",
            ".pptx"
        };

        Assert.Equal(expected, result);
    }
}
