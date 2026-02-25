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

public class SchedulerDayViewSettingsTests : TestBase
{
    public SchedulerDayViewSettingsTests()
    {
        // Autoriser les appels JS import() sans stub explicite pendant les tests
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;

        // Enregistre les services requis par les composants (IDialogService, etc.)
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void OnInitialized_Throws_When_NoParentProvided()
    {
        // Utilise RenderComponent fourni par TestBase (pas de TestContext local)
        Assert.Throws<InvalidOperationException>(() => RenderComponent<SchedulerDayViewSettings<object>>());
    }

    [Fact]
    public void Registers_With_Parent_OnInitialized()
    {
        // Render du composant parent
        var parent = RenderComponent<FluentCxScheduler<object>>(p =>
            p.Add(p => p.DaySubdivisions, 4)
             .Add(p => p.DaySlotHeight, 60));

        // Render du composant enfant en fournissant le parent en tant que CascadingValue
        var cut = RenderComponent<SchedulerDayViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));

        // Vérifie que le parent a bien enregistré l'instance (champ privé _daysViewSettingsMenu)
        var registered = GetPrivateField<object?>(parent.Instance, "_daysViewSettingsMenu");
        Assert.NotNull(registered);
    }

    [Fact]
    public void OnValueChanged_Updates_Parent_DaySettings()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p =>
            p.Add(p => p.DaySubdivisions, 4)
             .Add(p => p.DaySlotHeight, 60));

        var cut = RenderComponent<SchedulerDayViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        // Modifier les champs privés du composant enfant (_subdivisions et _slotHeight)
        SetPrivateField(instance, "_subdivisions", 2);
        SetPrivateField(instance, "_slotHeight", 30);

        // Invoquer la méthode privée OnValueChanged via reflection
        var method = instance.GetType().GetMethod("OnValueChanged", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);
        method!.Invoke(instance, Array.Empty<object>());

        // Vérifier que le parent a bien reçu les nouvelles valeurs
        Assert.Equal(2, parent.Instance.DaySubdivisions);
        Assert.Equal(30, parent.Instance.DaySlotHeight);
    }

    [Fact]
    public void Dispose_Unregisters_FromParent()
    {
        var parent = RenderComponent<FluentCxScheduler<object>>(p =>
            p.Add(p => p.DaySubdivisions, 4)
             .Add(p => p.DaySlotHeight, 60));

        var cut = RenderComponent<SchedulerDayViewSettings<object>>(ps => ps.AddCascadingValue(parent.Instance));
        var instance = cut.Instance;

        // Avant Dispose, le parent référence le menu
        var before = GetPrivateField<object?>(parent.Instance, "_daysViewSettingsMenu");
        Assert.NotNull(before);

        instance.Dispose();

        // Après Dispose, la référence doit être retirée
        var after = GetPrivateField<object?>(parent.Instance, "_daysViewSettingsMenu");
        Assert.Null(after);
    }

    // --- Helpers réflexion ---

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
}
