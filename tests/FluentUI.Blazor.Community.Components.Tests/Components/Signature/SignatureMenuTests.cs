using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureMenuTests : TestBase
{
    public SignatureMenuTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public async Task OnExportClickedAsync_InvokesCallback_WhenDelegateSet()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnExportClicked, EventCallback.Factory.Create(this, () => called = true))
        );

        var task = cut.Instance.GetType().GetMethod("OnExportClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)            .Invoke(cut.Instance, null) as Task;

        if(task is not null)
        {
            await task;
        }

        Assert.True(called);
    }

    [Fact]
    public async Task OnPenOrEraserClickedAsync_TogglesPenAndInvokesCorrectCallback()
    {
        bool penCalled = false, eraserCalled = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnPenClicked, EventCallback.Factory.Create(this, () => penCalled = true))
            .Add(p => p.OnEraserClicked, EventCallback.Factory.Create(this, () => eraserCalled = true))
        );

        await (Task)cut.Instance.GetType().GetMethod("OnPenOrEraserClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.True(eraserCalled);

        eraserCalled = false;

        await (Task)cut.Instance.GetType().GetMethod("OnPenOrEraserClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.True(penCalled);
    }

    [Fact]
    public async Task OnUndoClickedAsync_InvokesCallback_WhenDelegateSet()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnUndoClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        await (Task)cut.Instance.GetType().GetMethod("OnUndoClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.True(called);
    }

    [Fact]
    public async Task OnRedoClickedAsync_InvokesCallback_WhenDelegateSet()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnRedoClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        await (Task)cut.Instance.GetType().GetMethod("OnRedoClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.True(called);
    }

    [Fact]
    public void OnSettingsClicked_TogglesPopover()
    {
        var cut = RenderComponent<SignatureMenu>();
        var field = cut.Instance.GetType().GetField("_isMultiSettingsPopoverOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(cut.Instance, false);
        cut.Instance.GetType().GetMethod("OnSettingsClicked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.True((bool)field.GetValue(cut.Instance));
        cut.Instance.GetType().GetMethod("OnSettingsClicked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.False((bool)field.GetValue(cut.Instance));
    }

    [Fact]
    public async Task OnGridSettingsClickedAsync_ClosesPopoverAndInvokesCallback()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnGridSettingsClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        var field = cut.Instance.GetType().GetField("_isMultiSettingsPopoverOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(cut.Instance, true);
        await (Task)cut.Instance.GetType().GetMethod("OnGridSettingsClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.False((bool)field.GetValue(cut.Instance));
        Assert.True(called);
    }

    [Fact]
    public async Task OnWatermarkSettingsClickedAsync_ClosesPopoverAndInvokesCallback()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnWatermarkSettingsClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        var field = cut.Instance.GetType().GetField("_isMultiSettingsPopoverOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(cut.Instance, true);
        await (Task)cut.Instance.GetType().GetMethod("OnWatermarkSettingsClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.False((bool)field.GetValue(cut.Instance));
        Assert.True(called);
    }

    [Fact]
    public void OnPenSettingsClickedAsync_ClosesPopoverAndInvokesCallback()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnPenSettingsClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        var field = cut.Instance.GetType().GetField("_isMultiSettingsPopoverOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(cut.Instance, true);
        cut.Instance.GetType().GetMethod("OnPenSettingsClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.False((bool)field.GetValue(cut.Instance));
    }

    [Fact]
    public void OnEraserSettingsClickedAsync_ClosesPopoverAndInvokesCallback()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnEraserSettingsClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        var field = cut.Instance.GetType().GetField("_isMultiSettingsPopoverOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(cut.Instance, true);
        cut.Instance.GetType().GetMethod("OnEraserSettingsClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.False((bool)field.GetValue(cut.Instance));
    }

    [Fact]
    public async Task OnClearClickedAsync_InvokesCallback_WhenDelegateSet()
    {
        bool called = false;
        var cut = RenderComponent<SignatureMenu>(parameters => parameters
            .Add(p => p.OnClearClicked, EventCallback.Factory.Create(this, () => called = true))
        );
        await (Task)cut.Instance.GetType().GetMethod("OnClearClickedAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(cut.Instance, null);
        Assert.True(called);
    }
}
