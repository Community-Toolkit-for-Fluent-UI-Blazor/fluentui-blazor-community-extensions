using System.Reflection;
using System.Runtime.CompilerServices;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridDragHandlerTests
{
    [Fact]
    public void Start_SetsDragSourceAndActivatesGhostTile()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, out var state);
        var args = CreateDragEventArgs(items[0], items[1]);

        handler.Start(args);

        Assert.Equal(items[0], state.DragSource);
        Assert.True(items[0].IsDragging);
        Assert.Equal(items[0].Index, state.GhostTile.Index);
        Assert.Equal(items[0].RowSpan, state.GhostTile.RowSpan);
        Assert.Equal(items[0].ColumnSpan, state.GhostTile.ColumnSpan);
        Assert.True(state.GhostTile.IsActive);
    }

    [Fact]
    public void Enter_SetsPreviewTarget()
    {
        var items = CreateItems();
        items[0].IsPreviewTarget = true;
        var handler = CreateHandler(items, out var state);
        var args = CreateDragEventArgs(items[0], items[2]);

        handler.Enter(args);

        Assert.Equal(items[2], state.DragTarget);
        Assert.False(items[0].IsPreviewTarget);
        Assert.False(items[1].IsPreviewTarget);
        Assert.True(items[2].IsPreviewTarget);
    }

    [Fact]
    public void Leave_ClearsPreviewTarget()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, out var state);
        var args = CreateDragEventArgs(items[0], items[1]);

        handler.Enter(args);
        handler.Leave();

        Assert.Null(state.DragTarget);
        Assert.False(items[1].IsPreviewTarget);
    }

    [Fact]
    public void Drop_SwapsIndexesAndReordersItems()
    {
        var items = CreateItems();
        var source = items[0];
        var middle = items[1];
        var target = items[2];
        var handler = CreateHandler(items, out _);
        var args = CreateDragEventArgs(source, target);

        handler.Start(args);
        handler.Enter(args);
        handler.Drop();

        Assert.Equal(2, source.Index);
        Assert.Equal(0, target.Index);
        Assert.Equal([target, middle, source], items);
    }

    [Fact]
    public void End_ResetsDragStateAndAppliesLayout()
    {
        var items = CreateItems();
        items[0].Row = -1;
        items[0].Column = -1;
        items[1].Row = -1;
        items[1].Column = -1;
        items[2].Row = -1;
        items[2].Column = -1;

        var handler = CreateHandler(items, out var state);
        var args = CreateDragEventArgs(items[0], items[1]);

        handler.Start(args);
        handler.Enter(args);
        handler.End();

        Assert.Null(state.DragSource);
        Assert.Null(state.DragTarget);
        Assert.False(state.GhostTile.IsActive);
        Assert.All(items, item =>
        {
            Assert.False(item.IsPreviewTarget);
            Assert.False(item.IsDragging);
        });
        Assert.Equal(0, items[0].Row);
        Assert.Equal(0, items[0].Column);
        Assert.Equal(0, items[1].Row);
        Assert.Equal(1, items[1].Column);
        Assert.Equal(1, items[2].Row);
        Assert.Equal(0, items[2].Column);
    }

    private static TileGridDragHandler<TestTileGridItem> CreateHandler(
        List<TestTileGridItem> items,
        out TileGridState<TestTileGridItem> state)
    {
        state = new TileGridState<TestTileGridItem>();
        var tileGrid = new FluentCxTileGrid<TestTileGridItem>(new LibraryConfiguration());
        tileGrid.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            ["Columns"] = 2,
            ["Items"] = items
        })).GetAwaiter().GetResult();
        var layout = new TileGridLayoutEngine<TestTileGridItem>(tileGrid);

        return new TileGridDragHandler<TestTileGridItem>(state, layout, items);
    }

    private static List<TestTileGridItem> CreateItems()
    {
        return
        [
            new TestTileGridItem { Index = 0, RowSpan = 1, ColumnSpan = 1 },
            new TestTileGridItem { Index = 1, RowSpan = 1, ColumnSpan = 1 },
            new TestTileGridItem { Index = 2, RowSpan = 1, ColumnSpan = 1 }
        ];
    }

    private static FluentDragEventArgs<TestTileGridItem> CreateDragEventArgs(
        TestTileGridItem source,
        TestTileGridItem target)
    {
        var args = (FluentDragEventArgs<TestTileGridItem>)RuntimeHelpers.GetUninitializedObject(
            typeof(FluentDragEventArgs<TestTileGridItem>));

        SetValue(args, "Source", CreateDragItem(typeof(FluentDragEventArgs<TestTileGridItem>), "Source", source));
        SetValue(args, "Target", CreateDragItem(typeof(FluentDragEventArgs<TestTileGridItem>), "Target", target));

        return args;
    }

    private static object CreateDragItem(Type argsType, string propertyName, TestTileGridItem item)
    {
        var property = argsType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Property '{propertyName}' not found.");
        var itemType = property.PropertyType;
        var instance = RuntimeHelpers.GetUninitializedObject(itemType);

        SetValue(instance, "Item", item);

        return instance;
    }

    private static void SetValue(object target, string name, object value)
    {
        var targetType = target.GetType();
        var property = targetType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property?.CanWrite == true)
        {
            property.SetValue(target, value);
            return;
        }

        var field = targetType.GetField($"<{name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        if (field is null)
        {
            throw new InvalidOperationException($"Unable to set '{name}'.");
        }

        field.SetValue(target, value);
    }

    private sealed class TestTileGridItem : ITileGridItem
    {
        public string Key { get; } = "test-key";

        public long Index { get; set; }

        public int RowSpan { get; set; }

        public int ColumnSpan { get; set; }

        public bool IsDragging { get; set; }

        public bool IsPreviewTarget { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
