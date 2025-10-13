using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class AnimatedElementGroupTests
{
    [Fact]
    public void ApplyLayout_UsesProvidedStrategy_WhenNotNull()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        var strategyMock = new Mock<ILayoutStrategy>();
        group.SetMaxDisplayedItems(null);
        group.AnimatedElements.Add(new AnimatedElement { Id = "1" });
        group.SetMaxDisplayedItems(1);

        // Act
        group.ApplyLayout(strategyMock.Object);

        // Assert
        strategyMock.Verify(s => s.ApplyLayout(It.IsAny<List<AnimatedElement>>()), Times.Once);
        Assert.Equal(strategyMock.Object, group.LayoutStrategy);
    }

    [Fact]
    public void ApplyLayout_UsesDefaultStrategy_WhenNull()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        group.AnimatedElements.Add(new AnimatedElement { Id = "1" });
        group.SetMaxDisplayedItems(1);

        // Act
        group.ApplyLayout(null);

        // Assert
        Assert.NotNull(group.LayoutStrategy);
        Assert.IsType<BindStackLayout>(group.LayoutStrategy);
    }

    [Fact]
    public void ApplyStartTime_DelegatesToLayoutStrategy()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        var strategyMock = new Mock<ILayoutStrategy>();
        group.SetLayoutStrategy(strategyMock.Object);
        var now = DateTime.Now;

        // Act
        group.ApplyStartTime(now);

        // Assert
        strategyMock.Verify(s => s.ApplyStartTime(now), Times.Once);
    }

    [Fact]
    public void GetDiff_EnqueuesDiffsAndUpdatesPreviousElements()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        var element = new AnimatedElement { Id = "1", OffsetX = 0 };
        group.AnimatedElements.Add(element);
        group.SetMaxDisplayedItems(1);

        var previous = new AnimatedElement { Id = "1", OffsetX = 1 };
        var previousElements = new Dictionary<string, AnimatedElement> { { "1", previous } };
        var queue = new ConcurrentQueue<JsonAnimatedElement>();
        var now = DateTime.Now;

        // Act
        group.GetDiff(now, queue, previousElements);

        // Assert
        Assert.Single(queue);
        var json = queue.TryDequeue(out var result) ? result : null;
        Assert.NotNull(json);
        Assert.Equal("1", json.Id);
        Assert.NotEqual(previous.OffsetX, previousElements["1"].OffsetX);
    }

    [Fact]
    public void SetLayoutStrategy_SetsStrategy()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        var strategyMock = new Mock<ILayoutStrategy>();

        // Act
        group.SetLayoutStrategy(strategyMock.Object);

        // Assert
        Assert.Equal(strategyMock.Object, group.LayoutStrategy);
    }

    [Fact]
    public void SetMaxDisplayedItems_LimitsDisplayedItems()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        group.AnimatedElements.Add(new AnimatedElement { Id = "1" });
        group.AnimatedElements.Add(new AnimatedElement { Id = "2" });

        // Act
        group.SetMaxDisplayedItems(1);

        // Assert
        // _displayedItem is private, but indirectly tested via ApplyLayout
        var strategyMock = new Mock<ILayoutStrategy>();
        group.ApplyLayout(strategyMock.Object);
        strategyMock.Verify(s => s.ApplyLayout(It.Is<List<AnimatedElement>>(l => l.Count == 1)), Times.Once);
    }

    [Fact]
    public void SetMaxDisplayedItems_Null_DisplaysAll()
    {
        // Arrange
        var group = new AnimatedElementGroup();
        group.AnimatedElements.Add(new AnimatedElement { Id = "1" });
        group.AnimatedElements.Add(new AnimatedElement { Id = "2" });

        // Act
        group.SetMaxDisplayedItems(null);

        // Assert
        var strategyMock = new Mock<ILayoutStrategy>();
        group.ApplyLayout(strategyMock.Object);
        strategyMock.Verify(s => s.ApplyLayout(It.Is<List<AnimatedElement>>(l => l.Count == 2)), Times.Once);
    }
}
