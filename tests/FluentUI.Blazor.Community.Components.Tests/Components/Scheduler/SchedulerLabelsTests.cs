using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerLabelsTests
{
    [Fact]
    public void Default_HasExpectedValues()
    {
        var labels = SchedulerLabels.Default;

        Assert.Equal("Next", labels.Next);
        Assert.Equal("Previous", labels.Previous);
        Assert.Equal("Today", labels.Today);
        Assert.Equal("Day", labels.DayView);
        Assert.Equal("Week", labels.WeekView);
        Assert.Equal("Month", labels.MonthView);
        Assert.Equal("Agenda Settings", labels.AgendaSettings);
    }

    [Fact]
    public void French_StaticContainsTranslatedValues()
    {
        var fr = SchedulerLabels.French;

        Assert.Equal("Suivant", fr.Next);
        Assert.Equal("Précédent", fr.Previous);
        Assert.Equal("Aujourd'hui", fr.Today);
        Assert.Equal("Jour", fr.DayView);
        Assert.Equal("Hebdomadaire", fr.WeekView);
        Assert.Equal("Mois", fr.MonthView);
        Assert.Equal("Paramètres de la semaine", fr.WeekSettings);
        Assert.Equal("Paramètres de l'agenda", fr.AgendaSettings);
    }

    [Fact]
    public void Default_And_French_Are_Different()
    {
        var def = SchedulerLabels.Default;
        var fr = SchedulerLabels.French;

        // Vérifie qu'au moins une propriété diffère
        Assert.NotEqual(def.Next, fr.Next);
        Assert.NotEqual(def.DayView, fr.DayView);
    }

    [Fact]
    public void Can_Create_Custom_Labels_With_ObjectInitializer()
    {
        var custom = new SchedulerLabels
        {
            Next = "Suiv",
            Today = "Aujourd'hui modifié"
        };

        Assert.Equal("Suiv", custom.Next);
        Assert.Equal("Aujourd'hui modifié", custom.Today);

        // Les autres valeurs conservent leurs valeurs par défaut si non initialisées
        Assert.Equal("Previous", custom.Previous);
        Assert.Equal("Day", custom.DayView);
    }

    [Fact]
    public void ToString_Includes_Some_Properties()
    {
        var labels = SchedulerLabels.Default;
        var s = labels.ToString();

        Assert.Contains("Next", s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(labels.Next, s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("DayView", s, StringComparison.OrdinalIgnoreCase);
    }
}
