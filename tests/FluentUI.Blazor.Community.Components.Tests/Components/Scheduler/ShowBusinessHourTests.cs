using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerBusinessHoursTests : TestBase
{
    public SchedulerBusinessHoursTests()
    {
        // Autorise les appels JS import() non stubés pendant les tests
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;

        // Enregistreles services requis par les composants (IDialogService etc.)
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void OnInitialized_Throws_When_NoParentProvided()
    {
        // RenderComponent hérité de TestBase — doit lever car pas de parent en cascade
        Assert.Throws<InvalidOperationException>(() => RenderComponent<SchedulerBusinessHours<object>>());
    }

    [Fact]
    public void Registers_With_Parent_OnInitialized()
    {
        // Render du parent scheduler (avec paramètres si nécessaire)
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowBusinessHoursSettings, true));

        // Render du composant enfant en fournissant le parent en tant que CascadingValue
        var cut = RenderComponent<SchedulerBusinessHours<object>>(ps => ps.AddCascadingValue(parent.Instance));

        // Vérifie que le parent a bien enregistré l'instance via son champ privé _businessHours
        var bh = GetPrivateField<object?>(parent.Instance, "_businessHours");
        Assert.NotNull(bh);
    }

    [Fact]
    public void OnBusinessHoursToggle_Toggles_Parent_ShowNonWorkingHours()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowBusinessHoursSettings, true)
        .Add(p => p.ShowNonWorkingHours, false));

        var cut = RenderComponent<SchedulerBusinessHours<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        // Invoque la méthode privée OnBusinessHoursToggle via reflection
        var method = instance.GetType().GetMethod("OnBusinessHoursToggle", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);

        method!.Invoke(instance, Array.Empty<object>());

        // Vérifie que la valeur sur le parent a été basculée
        Assert.True(parent.Instance.ShowNonWorkingHours);
    }

    [Fact]
    public void Dispose_Unregisters_FromParent()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowBusinessHoursSettings, true));
        var cut = RenderComponent<SchedulerBusinessHours<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        // Avant Dispose, le parent référence le menu
        var before = GetPrivateField<object?>(parent.Instance, "_businessHours");
        Assert.NotNull(before);

        instance.Dispose();

        // Après Dispose, la référence doit être retirée
        var after = GetPrivateField<object?>(parent.Instance, "_businessHours");
        Assert.Null(after);
    }

    // --- Helper reflection util ---
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
