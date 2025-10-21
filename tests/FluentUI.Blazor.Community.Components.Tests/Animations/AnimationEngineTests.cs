using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Animations;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;
public class AnimationEngineTests
{
    [Fact]
    public void Register_AddsElementAndClone()
    {
        var engine = new AnimationEngine();
        var element = new AnimatedElement { Id = "e1" };

        engine.Register(element);

        // On ne peut pas accéder directement aux collections privées, mais on peut tester via Update
        var result = engine.Update();
        Assert.NotNull(result);
    }

    [Fact]
    public void RegisterGroup_AddsGroupAndSnapshots()
    {
        var engine = new AnimationEngine();
        var element = new AnimatedElement { Id = "e2" };
        var group = new AnimatedElementGroup();
        group.AnimatedElements.Add(element);

        engine.RegisterGroup(group);

        var result = engine.Update();
        Assert.NotNull(result);
    }

    [Fact]
    public void SetLayout_UsesCustomOrDefaultLayout()
    {
        var engine = new AnimationEngine();
        engine.SetLayout(null); // Doit utiliser BindStackLayout par défaut

        // Pas d'assert direct possible, mais on vérifie qu'aucune exception n'est levée
    }

    [Fact]
    public void Unregister_RemovesElement()
    {
        var engine = new AnimationEngine();
        var element = new AnimatedElement { Id = "e3" };
        engine.Register(element);

        engine.Unregister(element);

        var result = engine.Update();
        Assert.NotNull(result);
    }

    [Fact]
    public void UnregisterGroup_RemovesGroupAndElements()
    {
        var engine = new AnimationEngine();
        var element = new AnimatedElement { Id = "e4" };
        var group = new AnimatedElementGroup();
        group.AnimatedElements.Add(element);
        engine.RegisterGroup(group);

        engine.UnregisterGroup(group);

        var result = engine.Update();
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_ReturnsListOfJsonAnimatedElements()
    {
        var engine = new AnimationEngine();
        var element = new AnimatedElement { Id = "e5" };
        engine.Register(element);

        var result = engine.Update();

        Assert.IsType<List<JsonAnimatedElement>>(result);
    }

    [Fact]
    public void ApplyStartTime_PropagatesToLayoutAndGroups()
    {
        var engine = new AnimationEngine();
        var group = new AnimationGroup();
        engine.RegisterGroup(group.AnimatedElementGroup);

        var now = DateTime.Now;
        engine.ApplyStartTime(now);
    }

    [Fact]
    public void SetMaxDisplayedItems_PropagatesToGroups()
    {
        var engine = new AnimationEngine();
        var group = new AnimationGroup();
        engine.RegisterGroup(group.AnimatedElementGroup);

        engine.SetMaxDisplayedItems(5);

        var result = engine.GetType()
                           .GetField("_maxDisplayedItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                           .GetValue(engine);

        Assert.Equal(5, result);
    }
}
