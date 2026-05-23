using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class AcceptFileExtensionTests
{
    [Fact]
    public void None_IsZero()
    {
        Assert.Equal(0UL, (ulong)AcceptFileExtension.None);
    }

    [Theory]
    [InlineData(AcceptFileExtension.Jpg, 1UL)]
    [InlineData(AcceptFileExtension.Jpeg, 2UL)]
    [InlineData(AcceptFileExtension.Png, 4UL)]
    [InlineData(AcceptFileExtension.Gif, 8UL)]
    [InlineData(AcceptFileExtension.Bmp, 16UL)]
    [InlineData(AcceptFileExtension.Webp, 32UL)]
    [InlineData(AcceptFileExtension.Svg, 64UL)]
    [InlineData(AcceptFileExtension.Tiff, 128UL)]
    [InlineData(AcceptFileExtension.Heic, 256UL)]
    [InlineData(AcceptFileExtension.Pdf, 512UL)]
    [InlineData(AcceptFileExtension.Doc, 1024UL)]
    [InlineData(AcceptFileExtension.Docx, 2048UL)]
    [InlineData(AcceptFileExtension.Xls, 4096UL)]
    [InlineData(AcceptFileExtension.Xlsx, 8192UL)]
    [InlineData(AcceptFileExtension.Ppt, 16384UL)]
    [InlineData(AcceptFileExtension.Pptx, 32768UL)]
    [InlineData(AcceptFileExtension.Txt, 65536UL)]
    [InlineData(AcceptFileExtension.Csv, 131072UL)]
    [InlineData(AcceptFileExtension.Json, 262144UL)]
    [InlineData(AcceptFileExtension.Xml, 524288UL)]
    [InlineData(AcceptFileExtension.Md, 1048576UL)]
    [InlineData(AcceptFileExtension.Yaml, 2097152UL)]
    [InlineData(AcceptFileExtension.Zip, 4194304UL)]
    [InlineData(AcceptFileExtension.Rar, 8388608UL)]
    [InlineData(AcceptFileExtension.SevenZip, 16777216UL)]
    [InlineData(AcceptFileExtension.Tar, 33554432UL)]
    [InlineData(AcceptFileExtension.Gz, 67108864UL)]
    [InlineData(AcceptFileExtension.Cs, 134217728UL)]
    [InlineData(AcceptFileExtension.Js, 268435456UL)]
    [InlineData(AcceptFileExtension.Ts, 536870912UL)]
    [InlineData(AcceptFileExtension.Html, 1073741824UL)]
    [InlineData(AcceptFileExtension.Css, 2147483648UL)]
    [InlineData(AcceptFileExtension.Sql, 4294967296UL)]
    [InlineData(AcceptFileExtension.Py, 8589934592UL)]
    [InlineData(AcceptFileExtension.Java, 17179869184UL)]
    [InlineData(AcceptFileExtension.Obj, 34359738368UL)]
    [InlineData(AcceptFileExtension.Fbx, 68719476736UL)]
    [InlineData(AcceptFileExtension.Stl, 137438953472UL)]
    [InlineData(AcceptFileExtension.Step, 274877906944UL)]
    [InlineData(AcceptFileExtension.Ttf, 549755813888UL)]
    [InlineData(AcceptFileExtension.Otf, 1099511627776UL)]
    [InlineData(AcceptFileExtension.Woff, 2199023255552UL)]
    [InlineData(AcceptFileExtension.Woff2, 4398046511104UL)]
    [InlineData(AcceptFileExtension.Bin, 8796093022208UL)]
    [InlineData(AcceptFileExtension.Dat, 17592186044416UL)]
    [InlineData(AcceptFileExtension.FcxSurf, 35184372088832UL)]
    public void Flags_HaveExpectedValues(AcceptFileExtension value, ulong expected)
    {
        Assert.Equal(expected, (ulong)value);
    }

    [Fact]
    public void CommonGroups_CombineExpectedFlags()
    {
        var imageCommon = AcceptFileExtension.Jpg | AcceptFileExtension.Jpeg | AcceptFileExtension.Png |
            AcceptFileExtension.Gif | AcceptFileExtension.Bmp | AcceptFileExtension.Webp |
            AcceptFileExtension.Svg | AcceptFileExtension.Tiff | AcceptFileExtension.Heic;

        var documentCommon = AcceptFileExtension.Pdf | AcceptFileExtension.Doc | AcceptFileExtension.Docx |
            AcceptFileExtension.Xls | AcceptFileExtension.Xlsx | AcceptFileExtension.Ppt | AcceptFileExtension.Pptx;

        var textStructured = AcceptFileExtension.Json | AcceptFileExtension.Xml | AcceptFileExtension.Csv |
            AcceptFileExtension.Md | AcceptFileExtension.Yaml;

        var archiveCommon = AcceptFileExtension.Zip | AcceptFileExtension.Rar | AcceptFileExtension.SevenZip |
            AcceptFileExtension.Tar | AcceptFileExtension.Gz;

        var devCommon = AcceptFileExtension.Cs | AcceptFileExtension.Js | AcceptFileExtension.Ts |
            AcceptFileExtension.Html | AcceptFileExtension.Css | AcceptFileExtension.Sql |
            AcceptFileExtension.Py | AcceptFileExtension.Java;

        var fontCommon = AcceptFileExtension.Ttf | AcceptFileExtension.Otf |
            AcceptFileExtension.Woff | AcceptFileExtension.Woff2;

        var modelCommon = AcceptFileExtension.Obj | AcceptFileExtension.Fbx |
            AcceptFileExtension.Stl | AcceptFileExtension.Step;

        var binaryCommon = AcceptFileExtension.Bin | AcceptFileExtension.Dat | AcceptFileExtension.FcxSurf;

        Assert.Equal(AcceptFileExtension.ImageCommon, imageCommon);
        Assert.Equal(AcceptFileExtension.DocumentCommon, documentCommon);
        Assert.Equal(AcceptFileExtension.TextStructured, textStructured);
        Assert.Equal(AcceptFileExtension.ArchiveCommon, archiveCommon);
        Assert.Equal(AcceptFileExtension.DevCommon, devCommon);
        Assert.Equal(AcceptFileExtension.FontCommon, fontCommon);
        Assert.Equal(AcceptFileExtension.ModelCommon, modelCommon);
        Assert.Equal(AcceptFileExtension.BinaryCommon, binaryCommon);
    }

    [Fact]
    public void All_CombinesAllGroups()
    {
        var expected = AcceptFileExtension.ImageCommon | AcceptFileExtension.DocumentCommon |
            AcceptFileExtension.TextStructured | AcceptFileExtension.ArchiveCommon |
            AcceptFileExtension.DevCommon | AcceptFileExtension.FontCommon |
            AcceptFileExtension.ModelCommon | AcceptFileExtension.BinaryCommon;

        Assert.Equal(AcceptFileExtension.All, expected);
    }

    [Fact]
    public void All_IncludesEveryBaseFlag()
    {
        var baseFlags = new[]
        {
            AcceptFileExtension.Jpg,
            AcceptFileExtension.Jpeg,
            AcceptFileExtension.Png,
            AcceptFileExtension.Gif,
            AcceptFileExtension.Bmp,
            AcceptFileExtension.Webp,
            AcceptFileExtension.Svg,
            AcceptFileExtension.Tiff,
            AcceptFileExtension.Heic,
            AcceptFileExtension.Pdf,
            AcceptFileExtension.Doc,
            AcceptFileExtension.Docx,
            AcceptFileExtension.Xls,
            AcceptFileExtension.Xlsx,
            AcceptFileExtension.Ppt,
            AcceptFileExtension.Pptx,
            AcceptFileExtension.Csv,
            AcceptFileExtension.Json,
            AcceptFileExtension.Xml,
            AcceptFileExtension.Md,
            AcceptFileExtension.Yaml,
            AcceptFileExtension.Zip,
            AcceptFileExtension.Rar,
            AcceptFileExtension.SevenZip,
            AcceptFileExtension.Tar,
            AcceptFileExtension.Gz,
            AcceptFileExtension.Cs,
            AcceptFileExtension.Js,
            AcceptFileExtension.Ts,
            AcceptFileExtension.Html,
            AcceptFileExtension.Css,
            AcceptFileExtension.Sql,
            AcceptFileExtension.Py,
            AcceptFileExtension.Java,
            AcceptFileExtension.Obj,
            AcceptFileExtension.Fbx,
            AcceptFileExtension.Stl,
            AcceptFileExtension.Step,
            AcceptFileExtension.Ttf,
            AcceptFileExtension.Otf,
            AcceptFileExtension.Woff,
            AcceptFileExtension.Woff2,
            AcceptFileExtension.Bin,
            AcceptFileExtension.Dat,
            AcceptFileExtension.FcxSurf
        };

        foreach (var flag in baseFlags)
        {
            Assert.True(AcceptFileExtension.All.HasFlag(flag));
        }
    }

    [Fact]
    public void All_DoesNotIncludeTxt()
    {
        Assert.False(AcceptFileExtension.All.HasFlag(AcceptFileExtension.Txt));
    }
}
