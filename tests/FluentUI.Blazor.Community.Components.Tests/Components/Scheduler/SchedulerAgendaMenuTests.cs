using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerAgendaMenuTests : TestBase
{
    public SchedulerAgendaMenuTests()
    {
        Services.AddFluentUIComponents();
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;

        var mockDialogService = new Mock<IDialogService>();
        var mockDialogReference = new Mock<IDialogReference>();
        mockDialogReference.SetupGet(r => r.Result).Returns(Task.FromResult(DialogResult.Ok(true)));
        mockDialogService
            .Setup(s => s.ShowDialogAsync<SchedulerEventDialog<object>>(It.IsAny<object>(), It.IsAny<DialogParameters>()))
            .ReturnsAsync(mockDialogReference.Object);
        Services.AddScoped(_ => mockDialogService.Object);
    }

    [Fact]
    public void OnInitialized_Throws_When_NoParentProvided()
    {
        Assert.Throws<InvalidOperationException>(() =>
            RenderComponent<SchedulerAgendaMenu<object>>());
    }

    [Fact]
    public async Task OnValueChanged_Calls_Parent_Setters()
    {
        // Render parent scheduler et préparer l'état attendu
        var parentRendered = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowAgendaSettings, true)
         .Add(p => p.NumberOfDays, 5)
         .Add(p => p.HideEmptyDays, false));

        SetPrivateField(parentRendered.Instance, "_currentView", SchedulerView.Agenda);

        var cut = RenderComponent<SchedulerAgendaMenu<object>>(ps =>
            ps.AddCascadingValue(parentRendered.Instance));

        var instance = cut.Instance;

        // Modifier les champs privés liés au binding dans le composant
        SetPrivateField(instance, "_numberOfDays", 3);
        SetPrivateField(instance, "_hideEmptyDays", true);

        var method = instance.GetType().GetMethod("OnValueChanged", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);

        var task = (Task?)method!.Invoke(instance, Array.Empty<object>());
        Assert.NotNull(task);
        await task!;

        // Vérifier que le parent a reçu les nouvelles valeurs
        var numberOfDays = (int)GetInternalProperty(parentRendered.Instance, "NumberOfDays")!;
        var hideEmpty = (bool)GetInternalProperty(parentRendered.Instance, "HideEmptyDays")!;

        Assert.Equal(3, numberOfDays);
        Assert.True(hideEmpty);
    }

    [Fact]
    public void Dispose_Unregisters_FromParent()
    {
        var parentRendered = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowAgendaSettings, true));
        SetPrivateField(parentRendered.Instance, "_currentView", SchedulerView.Agenda);

        var cut = RenderComponent<SchedulerAgendaMenu<object>>(ps =>
            ps.AddCascadingValue(parentRendered.Instance));

        var instance = cut.Instance;

        // Vérifier que le parent référence bien le menu après OnInitialized
        var before = GetPrivateField<object?>(parentRendered.Instance, "_agendaMenu");
        Assert.NotNull(before);

        // Appel Dispose sur l'instance du menu
        instance.Dispose();

        // Le parent doit maintenant ne plus référencer le menu
        var after = GetPrivateField<object?>(parentRendered.Instance, "_agendaMenu");
        Assert.Null(after);
    }

    [Fact]
    public void Renders_Button_When_ParentViewIsAgenda_And_ShowAgendaSettingsTrue()
    {
        // Render le scheduler parent afin d'obtenir une instance correctement initialisée
        var parentRendered = RenderComponent<FluentCxScheduler<object>>(p => p.Add(p => p.ShowAgendaSettings, true));

        // Forcer la vue en Agenda (champ privé)
        SetPrivateField(parentRendered.Instance, "_currentView", SchedulerView.Agenda);

        // Render du menu en fournissant l'instance du parent en tant que CascadingValue
        var cut = RenderComponent<SchedulerAgendaMenu<object>>(ps =>
            ps.AddCascadingValue(parentRendered.Instance));

        var buttons = cut.FindAll("fluent-button");
        Assert.NotEmpty(buttons);
        Assert.Contains(parentRendered.Instance.Labels.AgendaSettings, cut.Markup);
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

    private static void SetInternalProperty(object target, string propertyName, object? value)
    {
        var t = target.GetType();
        var p = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (p == null)
        {
            throw new InvalidOperationException($"Property '{propertyName}' not found on {t.FullName}.");
        }

        p.SetValue(target, value);
    }

    private static object? GetInternalProperty(object target, string propertyName)
    {
        var t = target.GetType();
        var p = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (p == null)
        {
            throw new InvalidOperationException($"Property '{propertyName}' not found on {t.FullName}.");
        }

        return p.GetValue(target);
    }
}
