using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class WeekGridPositionMapperTests
{
    [Fact]
    public void Map_ReturnsEmpty_When_SlotsIsNull()
    {
        // Récupère le type générique WeekGridPositionMapper<object> via réflexion
        var mapperGeneric = typeof(WeekGridPositionMapper<>);
        var mapperType = mapperGeneric.MakeGenericType(typeof(object));

        // Crée une instance non initialisée (sans appeler le constructeur)
        var mapperInstance = FormatterServices.GetUninitializedObject(mapperType);

        // Récupère la méthode Map
        var method = mapperType.GetMethod("Map");
        Assert.NotNull(method);

        // Appelle Map avec slots = null ; autres paramètres peuvent être null car le code
        // retourne immédiatement si slots est null/empty.
        var result = (IEnumerable)method!.Invoke(mapperInstance, new object?[] { null, null, null, DateTime.UtcNow })!;

        // Vérifie que la collection retournée est vide
        var enumerator = result.GetEnumerator();
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Map_ReturnsEmpty_When_SlotsIsEmpty()
    {
        var mapperGeneric = typeof(WeekGridPositionMapper<>);
        var mapperType = mapperGeneric.MakeGenericType(typeof(object));
        var mapperInstance = FormatterServices.GetUninitializedObject(mapperType);

        var method = mapperType.GetMethod("Map");
        Assert.NotNull(method);

        // Tente de trouver le type SchedulerSlot dans l'assembly ; si absent, on utilise object comme fallback.
        var schedulerSlotType = mapperType.Assembly.GetType("FluentUI.Blazor.Community.Components.SchedulerSlot") ?? typeof(object);
        var emptySlots = Array.CreateInstance(schedulerSlotType, 0);

        var result = (IEnumerable)method!.Invoke(mapperInstance, new object?[] { emptySlots, null, null, DateTime.UtcNow })!;

        var enumerator = result.GetEnumerator();
        Assert.False(enumerator.MoveNext());
    }
}

