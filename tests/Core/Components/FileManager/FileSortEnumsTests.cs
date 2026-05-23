using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileSortEnumsTests
{
    [Fact]
    public void FileSortBy_HasExpectedValues()
    {
        var expectations = new[]
        {
            (FileSortBy.Name, 0),
            (FileSortBy.Extension, 1),
            (FileSortBy.Size, 2),
            (FileSortBy.CreatedDate, 3),
            (FileSortBy.ModifiedDate, 4),
            (FileSortBy.Type, 5)
        };

        foreach (var (value, expected) in expectations)
        {
            Assert.Equal(expected, (int)value);
        }
    }

    [Fact]
    public void FileSortLayout_HasExpectedValues()
    {
        var expectations = new[]
        {
            (FileSortLayout.None, 0),
            (FileSortLayout.Folders, 1),
            (FileSortLayout.Files, 2)
        };

        foreach (var (value, expected) in expectations)
        {
            Assert.Equal(expected, (int)value);
        }
    }

    [Fact]
    public void FileSortMode_HasExpectedValues()
    {
        var expectations = new[]
        {
            (FileSortMode.Ascending, 0),
            (FileSortMode.Descending, 1)
        };

        foreach (var (value, expected) in expectations)
        {
            Assert.Equal(expected, (int)value);
        }
    }

    [Fact]
    public void FileStructureView_HasExpectedValues()
    {
        var expectations = new[]
        {
            (FileStructureView.Hierarchical, 0),
            (FileStructureView.Flat, 1)
        };

        foreach (var (value, expected) in expectations)
        {
            Assert.Equal(expected, (int)value);
        }
    }

    [Fact]
    public void FileView_HasExpectedValues()
    {
        var expectations = new[]
        {
            (FileView.List, 0),
            (FileView.Details, 1),
            (FileView.Mosaic, 2),
            (FileView.SmallIcons, 3),
            (FileView.MediumIcons, 4),
            (FileView.LargeIcons, 5),
            (FileView.VeryLargeIcons, 6)
        };

        foreach (var (value, expected) in expectations)
        {
            Assert.Equal(expected, (int)value);
        }
    }
}
