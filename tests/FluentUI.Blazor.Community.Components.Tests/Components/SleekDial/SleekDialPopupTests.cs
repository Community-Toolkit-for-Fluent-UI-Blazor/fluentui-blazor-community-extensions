using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialPopupTests
: TestBase
{
    public SleekDialPopupTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void SleekDialPopup_CanBeInstantiated()
    {
        var cut = RenderComponent<SleekDialPopup>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public async Task OnAnimationCompletedAsync_InvokesCallback()
    {
        bool called = false;
        var cut = RenderComponent<SleekDialPopup>(parameters => parameters
            .Add(p => p.OnAnimationCompleted, EventCallback.Factory.Create<bool>(this, (b) => called = b))
        );
        await cut.Instance.OnAnimationCompletedAsync(true);
        Assert.True(called);
    }

    [Fact]
    public void RadialPositionUpdated_UpdatesOffsets()
    {
        var cut = RenderComponent<SleekDialPopup>();
        var instance = cut.Instance;
        var rect = new System.Drawing.RectangleF(1, 2, 3, 4);

        instance.RadialPositionUpdated(rect);

        Assert.Equal(1, GetField(instance, "_xOffset"));
        Assert.Equal(2, GetField(instance, "_yOffset"));
        Assert.Equal(3, GetField(instance, "_width"));
        Assert.Equal(4, GetField(instance, "_height"));

        static float GetField(SleekDialPopup instance, string fieldName)
        {
            var field = typeof(SleekDialPopup).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (float)field!.GetValue(instance)!;
        }
    }

    [Fact]
    public async Task UpdatePositionAsync_CallsInvokeScriptAsync()
    {
        var cut = RenderComponent<SleekDialPopup>();

        // Méthode interne, on vérifie qu'elle ne lève pas d'exception
        await cut.Instance.UpdatePositionAsync();
    }

    [Fact]
    public void CheckAngleRange_And_CheckAngle_WorkAsExpected()
    {
        var method = typeof(SleekDialPopup).GetMethod("CheckAngleRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var settings = new SleekDialRadialSettings();
        method.Invoke(null, new object[] { 25, 80, settings, true, 0, 90, false });

        Assert.Equal(25, settings.StartAngle);
        Assert.Equal(80, settings.EndAngle);
    }
}
