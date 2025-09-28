using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;

public class TrackInfoTests : TestBase
{
    [Fact]
    public void RendersNothing_WhenTrackIsNull()
    {
        // Arrange & Act
        var cut = RenderComponent<TrackInfo>(parameters => parameters
            .Add(p => p.Track, null)
        );

        // Assert
        Assert.Contains("stack-horizontal", cut.Markup);
        Assert.DoesNotContain("stack-vertical", cut.Markup);
    }

    [Fact]
    public void RendersTrackInfo_WhenTrackIsProvided()
    {
        var track = new AudioTrackItem
        {
            Title = "Test Title",
            Artist = "Test Artist",
            Cover = "cover.png"
        };

        var cut = RenderComponent<TrackInfo>(parameters => parameters
            .Add(p => p.Track, track)
        );

        cut.Find("img").MarkupMatches(@"<img width=""48"" height=""48"" src=""cover.png"" title=""Test Title"" aria-label=""Test Title"">");

        cut.Markup.Contains("Test Title");
        cut.Markup.Contains("Test Artist");
        cut.Markup.Contains("color: var(--accent-fill-rest)");
        cut.Markup.Contains("color: var(--neutral-fill-inverse-rest)");
    }

    [Fact]
    public void DoesNotRenderImage_WhenTrackCoverIsNullOrEmpty()
    {
        var track = new AudioTrackItem
        {
            Title = "Test Title",
            Artist = "Test Artist",
            Cover = null
        };

        var cut = RenderComponent<TrackInfo>(parameters => parameters
            .Add(p => p.Track, track)
        );

        Assert.Empty(cut.FindAll("img"));
    }

    [Fact]
    public void CallsOnHandleClickAsync_WhenClicked()
    {
        var track = new AudioTrackItem
        {
            Title = "Test Title",
            Artist = "Test Artist",
            Cover = "cover.png"
        };

        var clicked = false;
        var cut = RenderComponent<TrackInfo>(parameters => parameters
            .Add(p => p.Track, track)
            .Add(p => p.OnClick, EventCallback.Factory.Create<AudioTrackItem>(this, (e) => clicked = true))
        );

        cut.Find("div").Click();
        Assert.True(clicked);
    }
}
