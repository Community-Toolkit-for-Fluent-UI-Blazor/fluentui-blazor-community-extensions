using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class AcceptFileCategoryTests
{
    [Fact]
    public void None_IsZero()
    {
        Assert.Equal(0, (int)AcceptFileCategory.None);
    }

    [Theory]
    [InlineData(AcceptFileCategory.Image, 1)]
    [InlineData(AcceptFileCategory.Audio, 2)]
    [InlineData(AcceptFileCategory.Video, 4)]
    [InlineData(AcceptFileCategory.Document, 8)]
    [InlineData(AcceptFileCategory.Text, 16)]
    [InlineData(AcceptFileCategory.Archive, 32)]
    [InlineData(AcceptFileCategory.Code, 64)]
    [InlineData(AcceptFileCategory.ThreeD, 128)]
    [InlineData(AcceptFileCategory.Font, 256)]
    [InlineData(AcceptFileCategory.Binary, 512)]
    [InlineData(AcceptFileCategory.Proprietary, 1024)]
    public void Flags_HaveExpectedValues(AcceptFileCategory value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }

    [Fact]
    public void All_CombinesAllFlags()
    {
        var expected = AcceptFileCategory.None;

        foreach (var value in Enum.GetValues<AcceptFileCategory>())
        {
            if (value is AcceptFileCategory.None or AcceptFileCategory.All)
            {
                continue;
            }

            expected |= value;
            Assert.True(AcceptFileCategory.All.HasFlag(value));
        }

        Assert.Equal(AcceptFileCategory.All, expected);
    }
}
