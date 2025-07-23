using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.FileManager;

public class FileManagerGridTests : TestBase
{
    private class TestItem
    {
        public string? Name { get; set; }
    }

    public FileManagerGridTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<FileManagerState>();
        Services.AddScoped<DeviceInfoState>();
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void FileManagerGrid_GetRowHeightFromGridViewOptions_MosaicView_Returns100px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetRowHeightFromGridViewOptions");
        SetParentWithView(grid, FileView.Mosaic);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("100px", result);
    }

    [Fact]
    public void FileManagerGrid_GetRowHeightFromGridViewOptions_VeryLargeIconsView_Returns250px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetRowHeightFromGridViewOptions");
        SetParentWithView(grid, FileView.VeryLargeIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("250px", result);
    }

    [Fact]
    public void FileManagerGrid_GetRowHeightFromGridViewOptions_LargeIconsView_Returns220px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetRowHeightFromGridViewOptions");
        SetParentWithView(grid, FileView.LargeIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("220px", result);
    }

    [Fact]
    public void FileManagerGrid_GetRowHeightFromGridViewOptions_MediumIconsView_Returns200px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetRowHeightFromGridViewOptions");
        SetParentWithView(grid, FileView.MediumIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("200px", result);
    }

    [Fact]
    public void FileManagerGrid_GetRowHeightFromGridViewOptions_DefaultView_Returns100px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetRowHeightFromGridViewOptions");
        SetParentWithView(grid, FileView.SmallIcons); // Any other view should default

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("100px", result);
    }

    [Fact]
    public void FileManagerGrid_GetColumnWidthFromGridViewOptions_MosaicView_Returns300px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetColumnWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.Mosaic);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("300px", result);
    }

    [Fact]
    public void FileManagerGrid_GetColumnWidthFromGridViewOptions_VeryLargeIconsView_Returns250px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetColumnWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.VeryLargeIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("250px", result);
    }

    [Fact]
    public void FileManagerGrid_GetColumnWidthFromGridViewOptions_LargeIconsView_Returns220px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetColumnWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.LargeIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("220px", result);
    }

    [Fact]
    public void FileManagerGrid_GetColumnWidthFromGridViewOptions_MediumIconsView_Returns200px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetColumnWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.MediumIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("200px", result);
    }

    [Fact]
    public void FileManagerGrid_GetColumnWidthFromGridViewOptions_DefaultView_Returns300px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetColumnWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.SmallIcons); // Any other view should default

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("300px", result);
    }

    [Fact]
    public void FileManagerGrid_GetIconSizeFromGridViewOptions_VeryLargeIconsView_Returns128px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetIconSizeFromGridViewOptions");
        SetParentWithView(grid, FileView.VeryLargeIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("128px", result);
    }

    [Fact]
    public void FileManagerGrid_GetIconSizeFromGridViewOptions_LargeIconsView_Returns96px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetIconSizeFromGridViewOptions");
        SetParentWithView(grid, FileView.LargeIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("96px", result);
    }

    [Fact]
    public void FileManagerGrid_GetIconSizeFromGridViewOptions_MediumIconsView_Returns72px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetIconSizeFromGridViewOptions");
        SetParentWithView(grid, FileView.MediumIcons);

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("72px", result);
    }

    [Fact]
    public void FileManagerGrid_GetIconSizeFromGridViewOptions_DefaultView_Returns128px()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetIconSizeFromGridViewOptions");
        SetParentWithView(grid, FileView.Mosaic); // Any other view should default

        // Act
        var result = method!.Invoke(grid, null) as string;

        // Assert
        Assert.Equal("128px", result);
    }

    [Fact]
    public void FileManagerGrid_GetMaxWidthFromGridViewOptions_VeryLargeIconsView_Returns184()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetMaxWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.VeryLargeIcons);

        // Act
        var result = method!.Invoke(grid, null);

        // Assert
        Assert.Equal(184, result);
    }

    [Fact]
    public void FileManagerGrid_GetMaxWidthFromGridViewOptions_LargeIconsView_Returns154()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetMaxWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.LargeIcons);

        // Act
        var result = method!.Invoke(grid, null);

        // Assert
        Assert.Equal(154, result);
    }

    [Fact]
    public void FileManagerGrid_GetMaxWidthFromGridViewOptions_MediumIconsView_Returns134()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetMaxWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.MediumIcons);

        // Act
        var result = method!.Invoke(grid, null);

        // Assert
        Assert.Equal(134, result);
    }

    [Fact]
    public void FileManagerGrid_GetMaxWidthFromGridViewOptions_DefaultView_Returns180()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "GetMaxWidthFromGridViewOptions");
        SetParentWithView(grid, FileView.Mosaic); // Any other view should default

        // Act
        var result = method!.Invoke(grid, null);

        // Assert
        Assert.Equal(180, result);
    }

    [Fact]
    public async Task FileManagerGrid_OnCheckedItemChangedAsync_AddingItem_UpdatesSelectedItems()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "OnCheckedItemChangedAsync");
        
        var selectedItems = new List<FileManagerEntry<TestItem>>();
        var selectedItemsProperty = typeof(FileManagerGrid<TestItem>).GetProperty("SelectedItems");
        selectedItemsProperty!.SetValue(grid, selectedItems);

        var testEntry = FileManagerEntry<TestItem>.Home.CreateDefaultFileEntryWithData(
            new TestItem(), new byte[100], "test.txt", 100, DateTime.Now, DateTime.Now);

        var callbackInvoked = false;
        var callbackProperty = typeof(FileManagerGrid<TestItem>).GetProperty("SelectedItemsChanged");
        var callback = EventCallback.Factory.Create<IEnumerable<FileManagerEntry<TestItem>>>(
            this, (items) => { callbackInvoked = true; });
        callbackProperty!.SetValue(grid, callback);

        // Act
        var task = method!.Invoke(grid, new object[] { testEntry, true }) as Task;
        await task!;

        // Assert
        var updatedItems = selectedItemsProperty.GetValue(grid) as IEnumerable<FileManagerEntry<TestItem>>;
        Assert.Contains(testEntry, updatedItems!);
        Assert.True(callbackInvoked);
    }

    [Fact]
    public async Task FileManagerGrid_OnCheckedItemChangedAsync_RemovingItem_UpdatesSelectedItems()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var method = GetPrivateMethod(grid, "OnCheckedItemChangedAsync");
        
        var testEntry = FileManagerEntry<TestItem>.Home.CreateDefaultFileEntryWithData(
            new TestItem(), new byte[100], "test.txt", 100, DateTime.Now, DateTime.Now);
        
        var selectedItems = new List<FileManagerEntry<TestItem>> { testEntry };
        var selectedItemsProperty = typeof(FileManagerGrid<TestItem>).GetProperty("SelectedItems");
        selectedItemsProperty!.SetValue(grid, selectedItems);

        var callbackInvoked = false;
        var callbackProperty = typeof(FileManagerGrid<TestItem>).GetProperty("SelectedItemsChanged");
        var callback = EventCallback.Factory.Create<IEnumerable<FileManagerEntry<TestItem>>>(
            this, (items) => { callbackInvoked = true; });
        callbackProperty!.SetValue(grid, callback);

        // Act
        var task = method!.Invoke(grid, new object[] { testEntry, false }) as Task;
        await task!;

        // Assert
        var updatedItems = selectedItemsProperty.GetValue(grid) as IEnumerable<FileManagerEntry<TestItem>>;
        Assert.DoesNotContain(testEntry, updatedItems!);
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void FileManagerGrid_ItemTemplate_PropertyCanBeSet()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();
        var template = new RenderFragment<FileManagerEntry<TestItem>>(entry => builder => { });

        // Act
        grid.ItemTemplate = template;

        // Assert
        Assert.Equal(template, grid.ItemTemplate);
    }

    [Fact]
    public void FileManagerGrid_ItemTemplate_CanBeNull()
    {
        // Arrange
        var grid = new FileManagerGrid<TestItem>();

        // Act
        grid.ItemTemplate = null;

        // Assert
        Assert.Null(grid.ItemTemplate);
    }

    #region Helper Methods

    private static MethodInfo? GetPrivateMethod(object obj, string methodName)
    {
        return obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private static void SetParentWithView<T>(FileManagerGrid<T> grid, FileView view) where T : class, new()
    {
        // Create a mock FileManager with State
        var fileManager = new FluentCxFileManager<T>();
        fileManager.State = new FileManagerState { View = view };
        
        // Use reflection to set the Parent on the FileManagerGrid (it's a private protected property)
        var parentProperty = typeof(FileManagerGrid<T>).BaseType?
            .GetProperty("Parent", BindingFlags.NonPublic | BindingFlags.Instance);
        parentProperty?.SetValue(grid, fileManager);
    }

    #endregion
}