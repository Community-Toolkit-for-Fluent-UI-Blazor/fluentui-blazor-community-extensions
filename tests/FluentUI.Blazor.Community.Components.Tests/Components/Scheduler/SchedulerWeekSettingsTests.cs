using System.Linq;
using System.Reflection;
using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Xunit;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerWeekViewSettingsTests : TestBase
{
    public SchedulerWeekViewSettingsTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;

        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void OnInitialized_Throws_When_NoParentProvided()
    {
        Assert.Throws<InvalidOperationException>(() => RenderComponent<SchedulerWeekViewSettings<object>>());
    }

    [Fact]
    public void Registers_With_Parent_OnInitialized()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowWeekSettings, true));

        var cut = RenderComponent<SchedulerWeekViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));

        var registered = GetPrivateField<object?>(parent.Instance, "_weekViewSettingsMenu");
        Assert.NotNull(registered);
    }

    [Fact]
    public void Renders_Button_When_ParentViewIsWeek_And_ShowWeekSettingsTrue()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowWeekSettings, true));

        SetPrivateField(parent.Instance, "_currentView", SchedulerView.Week);

        var cut = RenderComponent<SchedulerWeekViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));

        var buttons = cut.FindAll("fluent-button");
        Assert.NotEmpty(buttons);
        Assert.Contains(parent.Instance.Labels.WeekSettings, cut.Markup);
    }

    [Fact]
    public void OnValueChanged_Updates_Parent_WeekSettings()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p =>
            p.Add(p => p.WeekSubdivisions, 4)
             .Add(p => p.WeekSlotHeight, 60)
             .Add(p => p.ShowWeekSettings, true));

        var cut = RenderComponent<SchedulerWeekViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        SetPrivateField(instance, "_weekSubdivisions", 2);
        SetPrivateField(instance, "_weekSlotHeight", 40);

        var method = instance.GetType().GetMethod("OnValueChanged", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);
        method!.Invoke(instance, Array.Empty<object>());

        Assert.Equal(2, parent.Instance.WeekSubdivisions);
        Assert.Equal(40, parent.Instance.WeekSlotHeight);
    }

    [Fact]
    public void Dispose_Unregisters_FromParent()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowWeekSettings, true));
        var cut = RenderComponent<SchedulerWeekViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        var before = GetPrivateField<object?>(parent.Instance, "_weekViewSettingsMenu");
        Assert.NotNull(before);

        instance.Dispose();
        var after = GetPrivateField<object?>(parent.Instance, "_weekViewSettingsMenu");
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
