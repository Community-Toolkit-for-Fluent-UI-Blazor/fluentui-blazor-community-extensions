using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;
public class SeekBarTests : TestBase
{
   /* private SeekBar CreateSeekBar(IJSObjectReference? module = null)
    {
        var seekBar = new SeekBar();
        var jsMock = Services.GetRequiredService<Mock<IJSRuntime>>();
        jsMock.Setup(js => js.InvokeAsync<IJSObjectReference>(It.IsAny<string>(), It.IsAny<object[]>()))
              .ReturnsAsync(module ?? Mock.Of<IJSObjectReference>());
        typeof(SeekBar).GetProperty("JS", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(seekBar, jsMock.Object);
        return seekBar;
    }

    [Fact]
    public async Task OnAfterRenderAsync_FirstRender_InitializesModule()
    {
        var seekBar = CreateSeekBar();
        var method = typeof(SeekBar).GetMethod("OnAfterRenderAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { true });

        var moduleField = typeof(SeekBar).GetField("_module", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(moduleField.GetValue(seekBar));
    }

    [Fact]
    public async Task OnResizeAsync_CallsMeasureWidthAsync()
    {
        var seekBar = CreateSeekBar();
        var method = typeof(SeekBar).GetMethod("OnResizeAsync", BindingFlags.Instance | BindingFlags.Public);
        await (Task)method.Invoke(seekBar, null);
    }

    [Fact]
    public async Task MeasureWidthAsync_SetsContainerWidth()
    {
        var moduleMock = new Mock<IJSObjectReference>();
        moduleMock.Setup(m => m.InvokeAsync<double>("fluentCxSeekBar.getWidth", It.IsAny<object[]>()))
                  .ReturnsAsync(123.0);
        var seekBar = CreateSeekBar(moduleMock.Object);
        typeof(SeekBar).GetField("_module", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(seekBar, moduleMock.Object);

        var method = typeof(SeekBar).GetMethod("MeasureWidthAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, null);

        var width = (double)typeof(SeekBar).GetField("_containerWidth", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar);
        Assert.Equal(123.0, width);
    }

    [Fact]
    public async Task StartDragAsync_SetsIsDraggingAndCallsUpdateSeek()
    {
        var seekBar = CreateSeekBar();
        var pointerArgs = new PointerEventArgs { ClientX = 50 };
        var method = typeof(SeekBar).GetMethod("StartDragAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { pointerArgs });
        var isDragging = (bool)typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar);
        Assert.True(isDragging);
    }

    [Fact]
    public async Task OnDragAsync_WhenDragging_CallsUpdateSeek()
    {
        var seekBar = CreateSeekBar();
        typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, true);
        var pointerArgs = new PointerEventArgs { ClientX = 60 };
        var method = typeof(SeekBar).GetMethod("OnDragAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { pointerArgs });
    }

    [Fact]
    public async Task EndDragAsync_ResetsIsDraggingAndShowPreview()
    {
        var seekBar = CreateSeekBar();
        typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, true);
        var pointerArgs = new PointerEventArgs();
        var method = typeof(SeekBar).GetMethod("EndDragAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { pointerArgs });
        Assert.False((bool)typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
        Assert.False((bool)typeof(SeekBar).GetField("_showPreview", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
    }

    [Fact]
    public async Task StartTouchAsync_SetsIsDraggingAndCallsUpdateSeek()
    {
        var seekBar = CreateSeekBar();
        var touchArgs = new TouchEventArgs { Touches = new[] { new TouchPoint { ClientX = 70 } } };
        var method = typeof(SeekBar).GetMethod("StartTouchAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { touchArgs });
        Assert.True((bool)typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
    }

    [Fact]
    public async Task OnTouchMoveAsync_WhenDragging_CallsUpdateSeek()
    {
        var seekBar = CreateSeekBar();
        typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, true);
        var touchArgs = new TouchEventArgs { Touches = new[] { new TouchPoint { ClientX = 80 } } };
        var method = typeof(SeekBar).GetMethod("OnTouchMoveAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { touchArgs });
    }

    [Fact]
    public async Task EndTouchAsync_ResetsIsDraggingAndShowPreview()
    {
        var seekBar = CreateSeekBar();
        typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, true);
        var touchArgs = new TouchEventArgs { Touches = new[] { new TouchPoint { ClientX = 90 } } };
        var method = typeof(SeekBar).GetMethod("EndTouchAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { touchArgs });
        Assert.False((bool)typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
        Assert.False((bool)typeof(SeekBar).GetField("_showPreview", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
    }

    [Fact]
    public async Task OnKeyDownAsync_HandlesKeys()
    {
        var seekBar = CreateSeekBar();
        seekBar.Duration = 100;
        seekBar.CurrentTime = 50;
        seekBar.Step = 10;
        var keyArgs = new FluentKeyCodeEventArgs { Key = KeyCode.Left };
        var method = typeof(SeekBar).GetMethod("OnKeyDownAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { keyArgs });
        Assert.Equal(40, seekBar.CurrentTime);
    }

    [Fact]
    public async Task UpdateSeekAsync_ClampsPercentAndCallsSeekToAsync()
    {
        var seekBar = CreateSeekBar();
        seekBar.Duration = 100;
        typeof(SeekBar).GetField("_containerWidth", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, 100);
        var method = typeof(SeekBar).GetMethod("UpdateSeekAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { 50.0 });
        Assert.Equal(50, seekBar.CurrentTime);
    }

    [Fact]
    public async Task UpdateSeekFromClientXAsync_WithoutModule_DoesNothing()
    {
        var seekBar = CreateSeekBar(null);
        var method = typeof(SeekBar).GetMethod("UpdateSeekFromClientXAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { 100.0 });
    }

    [Fact]
    public async Task SeekToAsync_ClampsTimeAndSetsPreview()
    {
        var seekBar = CreateSeekBar();
        seekBar.Duration = 100;
        seekBar.Chapters = new List<Chapter> { new Chapter(0, 100, "Chapter 1", ChapterStatus.NotStarted) };
        var method = typeof(SeekBar).GetMethod("SeekToAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { 120.0, true });
        Assert.Equal(100, seekBar.CurrentTime);
        Assert.True((bool)typeof(SeekBar).GetField("_showPreview", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
    }

    [Fact]
    public async Task HidePreviewAfterDelayAsync_SetsShowPreviewFalse()
    {
        var seekBar = CreateSeekBar();
        typeof(SeekBar).GetField("_showPreview", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, true);
        var method = typeof(SeekBar).GetMethod("HidePreviewAfterDelayAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        var cts = new CancellationTokenSource();
        await (Task)method.Invoke(seekBar, new object[] { cts.Token });
        Assert.False((bool)typeof(SeekBar).GetField("_showPreview", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(seekBar));
    }

    [Fact]
    public async Task OnClickSeek_WhenDragging_DoesNothing()
    {
        var seekBar = CreateSeekBar();
        typeof(SeekBar).GetField("_isDragging", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, true);
        var mouseArgs = new MouseEventArgs { ClientX = 10 };
        var method = typeof(SeekBar).GetMethod("OnClickSeek", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)method.Invoke(seekBar, new object[] { mouseArgs });
    }

    [Fact]
    public void GetChapterColor_ReturnsExpectedGradient()
    {
        var method = typeof(SeekBar).GetMethod("GetChapterColor", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.Equal("linear-gradient(90deg, #87d068, #5cb85c)", method.Invoke(null, new object[] { ChapterStatus.Completed }));
        Assert.Equal("linear-gradient(90deg, #ffd773, #ffb900)", method.Invoke(null, new object[] { ChapterStatus.Current }));
        Assert.Equal("linear-gradient(90deg, #ff8a8a, #e81123)", method.Invoke(null, new object[] { ChapterStatus.NotStarted }));
        Assert.Equal("gray", method.Invoke(null, new object[] { ChapterStatus.Unspecified }));
    }

    [Fact]
    public void FormatTime_ReturnsFormattedString()
    {
        var method = typeof(SeekBar).GetMethod("FormatTime", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.Equal("01:40", method.Invoke(null, new object[] { 100.0 }));
    }

    [Fact]
    public async Task DisposeAsync_DisposesModuleAndDotNetRef()
    {
        var moduleMock = new Mock<IJSObjectReference>();
        moduleMock.Setup(m => m.InvokeVoidAsync("fluentCxSeekBar.dispose", It.IsAny<object[]>())).Returns(ValueTask.CompletedTask);
        moduleMock.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask);
        var seekBar = CreateSeekBar(moduleMock.Object);
        typeof(SeekBar).GetField("_module", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(seekBar, moduleMock.Object);
        var method = typeof(SeekBar).GetMethod("DisposeAsync", BindingFlags.Instance | BindingFlags.Public);
        await (ValueTask)method.Invoke(seekBar, null);
    }
   */
}
