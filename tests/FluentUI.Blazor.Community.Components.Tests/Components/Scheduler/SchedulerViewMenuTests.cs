using System.Reflection;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerViewMenuTests : TestBase
{
    public SchedulerViewMenuTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;

        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void OnInitialized_Throws_When_NoParentProvided()
    {
        // Pas de CascadingValue parent -> OnInitialized doit lever
        Assert.Throws<InvalidOperationException>(() => RenderComponent<SchedulerViewMenu<object>>());
    }

    [Fact]
    public void Renders_Buttons_For_Each_AvailableView()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>();

        var cut = RenderComponent<SchedulerViewMenu<object>>(ps => ps.AddCascadingValue(parent.Instance));

        var buttons = cut.FindAll("fluent-button");
        Assert.Equal(parent.Instance.AvailableViews.Count, buttons.Count);
    }

    [Fact]
    public void Clicking_Button_Changes_Parent_View()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.CurrentDate, new DateTime(2025, 11, 20)));

        SetPrivateField(parent.Instance, "_currentView", SchedulerView.Day);

        var cut = RenderComponent<SchedulerViewMenu<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var buttons = cut.FindAll("fluent-button");

        var targetIndex = parent.Instance.AvailableViews.ToList().FindIndex(v => v == SchedulerView.Week);
        if (targetIndex < 0)
        {
            targetIndex = Math.Min(1, buttons.Count - 1);
        }

        var targetButton = buttons[targetIndex];

        targetButton.Click();

        var expectedView = parent.Instance.AvailableViews[targetIndex];
        Assert.Equal(expectedView, parent.Instance.View);
    }

    [Fact]
    public void Dispose_Unregisters_ViewMenu_FromParent()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>();
        var cut = RenderComponent<SchedulerViewMenu<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        var before = GetPrivateField<object?>(parent.Instance, "_viewMenu");
        Assert.NotNull(before);

        instance.Dispose();

        var after = GetPrivateField<object?>(parent.Instance, "_viewMenu");
        Assert.Null(after);
    }

    private static void SetPrivateField(object target, string fieldName, object? value)
    {
        var t = target.GetType();
        var f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (f == null)
        {
            throw new InvalidOperationException($"Field '{fieldName}' not found on {t.FullName}.");
        }

        f.SetValue(target, value);
    }

    private static T? GetPrivateField<T>(object target, string fieldName)
    {
        var t = target.GetType();
        var f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (f == null)
        {
            throw new InvalidOperationException($"Field '{fieldName}' not found on {t.FullName}.");
        }

        return (T?)f.GetValue(target);
    }
}
