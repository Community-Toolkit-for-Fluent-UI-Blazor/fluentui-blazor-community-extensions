using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class FluentCxSleekDialTests
    : TestBase
{
    public FluentCxSleekDialTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void FluentCxSleekDial_CanBeInstantiated()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public async Task OnClickAsync_DoesNothing_WhenPopupNullOrOpensOnHoverOrDisabled()
    {
        var cut = RenderComponent<FluentCxSleekDial>(parameters => parameters
            .Add(p => p.OpensOnHover, true)
            .Add(p => p.Disabled, true)
        );
        // _popup est null, OpensOnHover et Disabled sont true
        await cut.Instance.OnClickAsync();
        // Pas d'exception attendue
    }

    [Fact]
    public async Task ShowOrHidePopupAsync_DoesNothing_IfAlreadyInDesiredState()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        // _isOpen == false par défaut
        await cut.Instance.ShowOrHidePopupAsync(false);
        // Pas d'exception attendue
    }

    [Fact]
    public async Task ShowOrHidePopupAsync_InvokesOpeningOrClosing()
    {
        bool openingCalled = false, closingCalled = false;
        var cut = RenderComponent<FluentCxSleekDial>(parameters => parameters
            .Add(p => p.Opening, EventCallback.Factory.Create(this, () => openingCalled = true))
            .Add(p => p.Closing, EventCallback.Factory.Create(this, () => closingCalled = true))
        );
        // Forcer _isOpen à false, on veut ouvrir
        await cut.Instance.ShowOrHidePopupAsync(true);
        Assert.True(openingCalled || closingCalled); // L'un des deux doit être appelé selon l'état
    }

    [Fact]
    public async Task OnKeyDownHandlerAsync_HandlesKeys()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        var instance = cut.Instance;
        // Ajouter un item pour tester l'action sur Enter/Space
        var item = new SleekDialItem();
        instance.InternalItems.Add(item);
        instance.FocusedIndex = 0;
        // Simuler l'ouverture
        typeof(FluentCxSleekDial).GetField("_isOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(instance, true);

        var args = new FluentKeyCodeEventArgs { Key = KeyCode.Enter };
        await InvokePrivateAsync(instance, "OnKeyDownHandlerAsync", args);
        // Pas d'exception attendue
    }

    [Fact]
    public async Task UpdatePopupPositionAsync_DoesNothing_IfLinearDirectionNotChangedOrPopupNull()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        await InvokePrivateAsync(cut.Instance, "UpdatePopupPositionAsync");
        // Pas d'exception attendue
    }

    [Fact]
    public async Task OnAnimationCompletedAsync_UpdatesIsOpen()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        await cut.Instance.OnAnimationCompletedAsync(true);
        Assert.True((bool)typeof(FluentCxSleekDial).GetField("_isOpen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(cut.Instance)!);
    }

    [Fact]
    public void AddChild_AddsItem()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        var item = new SleekDialItem();
        cut.Instance.AddChild(item);
        Assert.Contains(item, cut.Instance.InternalItems);
    }

    [Fact]
    public void RemoveChild_RemovesItem()
    {
        var cut = RenderComponent<FluentCxSleekDial>();
        var item = new SleekDialItem();
        cut.Instance.AddChild(item);
        cut.Instance.RemoveChild(item);
        Assert.DoesNotContain(item, cut.Instance.InternalItems);
    }

    [Fact]
    public async Task OnCreatedAsync_InvokesItemRendered()
    {
        bool called = false;
        var cut = RenderComponent<FluentCxSleekDial>(parameters => parameters
            .Add(p => p.ItemRendered, EventCallback.Factory.Create<SleekDialItem>(this, (i) => called = true))
        );
        await cut.Instance.OnCreatedAsync(new SleekDialItem());
        Assert.True(called);
    }

    // Utilitaire pour invoquer une méthode privée async
    private static async Task InvokePrivateAsync(object instance, string methodName, params object[]? parameters)
    {
        var method = instance.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task)method!.Invoke(instance, parameters)!;
        await task;
    }
}
