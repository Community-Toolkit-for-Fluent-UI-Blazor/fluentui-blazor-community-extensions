using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileManagerContentTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var label = "File";
        var placeholder = "Enter a name";
        var name = "document.txt";
        var isDirectory = false;
        var isRenaming = true;

        // Act
        var content = new FileManagerContent(label, placeholder, name, isDirectory, isRenaming);

        // Assert
        Assert.Equal(label, content.Label);
        Assert.Equal(placeholder, content.Placeholder);
        Assert.Equal(name, content.Name);
        Assert.Equal(isDirectory, content.IsDirectory);
        Assert.Equal(isRenaming, content.IsRenaming);
    }

    [Fact]
    public void Name_Setter_UpdatesValue()
    {
        // Arrange
        var content = new FileManagerContent("Label", "Placeholder", "OldName", false, false);

        // Act
        content.Name = "NewName";

        // Assert
        Assert.Equal("NewName", content.Name);
    }

    [Fact]
    public void Records_AreEqual_WhenPropertiesAreEqual()
    {
        // Arrange
        var a = new FileManagerContent("A", "B", "C", true, false);
        var b = new FileManagerContent("A", "B", "C", true, false);

        // Assert
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Records_AreNotEqual_WhenPropertiesDiffer()
    {
        // Arrange
        var a = new FileManagerContent("A", "B", "C", true, false);
        var b = new FileManagerContent("A", "B", "D", true, false);

        // Assert
        Assert.NotEqual(a, b);
        Assert.False(a == b);
    }

    [Fact]
    public void WithExpression_CreatesModifiedCopy()
    {
        // Arrange
        var original = new FileManagerContent("A", "B", "C", true, false);

        // Act
        var copy = original with { Name = "D" };

        // Assert
        Assert.Equal("D", copy.Name);
        Assert.Equal(original.Label, copy.Label);
        Assert.NotEqual(original, copy);
    }
}
