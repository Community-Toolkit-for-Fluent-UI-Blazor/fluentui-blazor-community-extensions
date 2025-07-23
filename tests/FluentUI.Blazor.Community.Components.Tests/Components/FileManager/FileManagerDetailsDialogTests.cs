using System.Collections.Generic;
using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.FileManager;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileManagerDetailsDialogTests : TestBase
{
    [Fact]
    public void RendersNothing_WhenContentIsNull()
    {
        var cut = RenderComponent<FileManagerDetailsDialog<NoFileEntryData>>(parameters => parameters
            .Add(p => p.Content, null)
        );

        cut.MarkupMatches("<div class=\"fluent-dialog-body\"></div>");
    }

    [Fact]
    public void RendersFileManagerEntryDetails_WhenContentIsProvided()
    {
        var content = new FileManagerDetailsDialogContent<NoFileEntryData>(
            FileExtensionTypeLabels.Default,
            []);

        var cut = RenderComponent<FileManagerDetailsDialog<NoFileEntryData>>(parameters => parameters
            .Add(p => p.Content, content)
        );

        cut.FindComponent<FileManagerEntryDetails<NoFileEntryData>>();
        Assert.Equal(content.FileExtensionTypeLabels, cut.Instance.Content.FileExtensionTypeLabels);
        Assert.Equal(content.Entries, cut.Instance.Content.Entries);
    }

    [Fact]
    public void ContentParameter_IsSettable()
    {
        var content = new FileManagerDetailsDialogContent<NoFileEntryData>(
            FileExtensionTypeLabels.Default,
            []);

        var cut = RenderComponent<FileManagerDetailsDialog<NoFileEntryData>>();
        cut.SetParametersAndRender(parameters => parameters.Add(p => p.Content, content));
        Assert.Equal(content, cut.Instance.Content);
    }
}
