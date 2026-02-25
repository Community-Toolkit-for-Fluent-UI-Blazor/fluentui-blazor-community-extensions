using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class EditSchedulerItemTests
{
    private static Type GetEditSchedulerItemType()
    {
        // Récupère l'assembly contenant les composants publics (ex : SchedulerCalendar)
        var asm = typeof(SchedulerCalendar).Assembly;
        var raw = asm.GetType("FluentUI.Blazor.Community.Components.EditSchedulerItem`1", throwOnError: true)!;
        return raw;
    }

    [Fact]
    public void CanInstantiateAndDefaults_AreCorrect()
    {
        // Arrange
        var t = GetEditSchedulerItemType().MakeGenericType(typeof(string));
        var instance = Activator.CreateInstance(t)!;

        // Act - read properties via reflection
        var titleProp = t.GetProperty("Title")!;
        var exceptionsProp = t.GetProperty("Exceptions")!;
        var dataProp = t.GetProperty("Data")!;
        var startProp = t.GetProperty("Start")!;
        var startTimeProp = t.GetProperty("StartTime")!;
        var endProp = t.GetProperty("End")!;
        var endTimeProp = t.GetProperty("EndTime")!;

        var title = (string)titleProp.GetValue(instance)!;
        var exceptions = (IList<DateTime>)exceptionsProp.GetValue(instance)!;
        var data = dataProp.GetValue(instance);
        var start = startProp.GetValue(instance);
        var startTime = startTimeProp.GetValue(instance);
        var end = endProp.GetValue(instance);
        var endTime = endTimeProp.GetValue(instance);

        // Assert
        Assert.Equal(string.Empty, title);
        Assert.NotNull(exceptions);
        Assert.Empty(exceptions);
        Assert.Null(data);
        Assert.Null(start);
        Assert.Null(startTime);
        Assert.Null(end);
        Assert.Null(endTime);
    }

    [Fact]
    public void DataProperty_PreservesGenericType()
    {
        // Arrange
        var t = GetEditSchedulerItemType().MakeGenericType(typeof(string));
        var instance = Activator.CreateInstance(t)!;
        var dataProp = t.GetProperty("Data")!;

        // Act
        dataProp.SetValue(instance, "payload");
        var value = dataProp.GetValue(instance);

        // Assert
        Assert.IsType<string>(value);
        Assert.Equal("payload", value);
    }

    [Fact]
    public void Exceptions_AddAndRemove_WorksAsList()
    {
        // Arrange
        var t = GetEditSchedulerItemType().MakeGenericType(typeof(object));
        var instance = Activator.CreateInstance(t)!;
        var exceptionsProp = t.GetProperty("Exceptions")!;
        var exceptions = (List<DateTime>)exceptionsProp.GetValue(instance)!;

        var d = new DateTime(2026, 1, 1);

        // Act
        exceptions.Add(d);
        exceptions.Add(d); // allow duplicate as underlying type is List<T>
        exceptions.Remove(d);

        // Assert
        Assert.Contains(d, exceptions);
        Assert.Equal(1, exceptions.Count); // one duplicate removed, one remains
    }

    [Fact]
    public void Validation_Fails_WhenRequiredMissing_AndSucceeds_WhenPresent()
    {
        // Arrange
        var t = GetEditSchedulerItemType().MakeGenericType(typeof(object));
        var instance = Activator.CreateInstance(t)!;

        var ctx = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        // Act - validate with required fields missing
        var isValidBefore = Validator.TryValidateObject(instance, ctx, results, validateAllProperties: true);

        // Assert - should be invalid because Required properties are null/empty
        Assert.False(isValidBefore);
        Assert.NotEmpty(results);

        // Now set required properties: Title, Start, StartTime, End, EndTime
        t.GetProperty("Title")!.SetValue(instance, "My event");
        var now = DateTime.UtcNow;
        t.GetProperty("Start")!.SetValue(instance, now);
        t.GetProperty("StartTime")!.SetValue(instance, now);
        t.GetProperty("End")!.SetValue(instance, now.AddHours(1));
        t.GetProperty("EndTime")!.SetValue(instance, now.AddHours(1));

        // Revalidate
        results.Clear();
        ctx = new ValidationContext(instance);
        var isValidAfter = Validator.TryValidateObject(instance, ctx, results, validateAllProperties: true);

        Assert.True(isValidAfter);
        Assert.Empty(results);
    }
}
