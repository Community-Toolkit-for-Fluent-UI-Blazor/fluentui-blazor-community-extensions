using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.FileManager;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;
using System.Linq;
using Moq;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.FileManager;

public class FileManagerTests : TestBase
{
    private class TestRenamable : IRenamable
    {
        public bool IsRenamable => true;
    }

    private class TestDeletable : IDeletable
    {
        public bool IsDeleteable => true;
    }

    public FileManagerTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<FileManagerState>();
        Services.AddScoped<DeviceInfoState>();
        Services.AddFluentUIComponents();
    }

    #region Helper Methods

    private IRenderedFragment RenderFileManager(Action<ComponentParameterCollectionBuilder<FluentCxFileManager<object>>>? configureParameters = null)
    {
        return Render(b =>
        {
            b.OpenComponent<FluentMenuProvider>(0);
            b.CloseComponent();
            b.OpenComponent<FluentCxFileManager<object>>(1);
            if (configureParameters != null)
            {
                var parameters = new ComponentParameterCollectionBuilder<FluentCxFileManager<object>>();
                configureParameters(parameters);
                foreach (var param in parameters.Build())
                {
                    b.AddAttribute(2, param.Name, param.Value);
                }
            }
            b.CloseComponent();
        });
    }

    private IRenderedComponent<FluentCxFileManager<T>> RenderFileManagerComponent<T>(Action<ComponentParameterCollectionBuilder<FluentCxFileManager<T>>>? configureParameters = null) where T : class, new()
    {
        RenderComponent<FluentMenuProvider>();
        var parameters = new ComponentParameterCollectionBuilder<FluentCxFileManager<T>>();
        configureParameters?.Invoke(parameters);
        return RenderComponent<FluentCxFileManager<T>>(parameters.Build().ToArray());
    }

    private FileManagerEntry<object> CreateTestFileEntry(string fileName = "file.txt", int size = 123)
    {
        var root = FileManagerEntry<object>.Home;
        var fileEntry = root.CreateDefaultFileEntryWithData(new object(), new byte[0], fileName, size, DateTime.Now, DateTime.Now);
        root.AddRange(fileEntry);
        return root;
    }

    #endregion

    [Fact]
    public void FluentCxFileManager_RendersCoreElements_WithDefaults()
    {
        // Arrange & Act
        var cut = RenderFileManager(p=> p.Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert: Core elements are present
        Assert.NotNull(cut.Find("fluent-toolbar"));
        Assert.Contains("New Folder", cut.Markup);
        Assert.Contains("Upload", cut.Markup);
        Assert.Contains("View", cut.Markup);
        Assert.Contains("Sort", cut.Markup);
        Assert.Contains("Rename", cut.Markup);
        Assert.Contains("Download", cut.Markup);
        Assert.Contains("Delete", cut.Markup);
        Assert.NotNull(cut.Find("fluent-search"));
        Assert.NotNull(cut.Find("fluent-tree-view"));
        Assert.NotNull(cut.Find(".fluent-inputfile-container"));
    }

    [Fact]
    public void FluentCxFileManager_AppliesWidthParameter()
    {
        // Arrange
        var width = "420px";

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.Width, width)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        var splitter = cut.Find(".fluent-multi-splitter");
        var style = splitter.GetAttribute("style");
        Assert.Contains($"width: {width}", style);
    }

    [Fact]
    public void FluentCxFileManager_AppliesHeightParameter()
    {
        // Arrange
        var height = "333px";

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.Height, height)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        var splitter = cut.Find(".fluent-multi-splitter");
        var style = splitter.GetAttribute("style");
        Assert.Contains($"height: {height}", style);
    }

    [Fact]
    public void FluentCxFileManager_HidesCreateFolderButton_WhenSetToFalse()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.ShowCreateFolderButton, false)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.DoesNotContain("New Folder", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_HidesUploadButton_WhenSetToFalse()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.ShowUploadButton, false)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.DoesNotContain("Upload", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_HidesViewMenu_WhenSetToFalse()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.ShowViewButton, false)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.DoesNotContain("View", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_HidesSortMenu_WhenSetToFalse()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.ShowSortButton, false)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.DoesNotContain("Sort", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_HidesPropertiesButtons_WhenSetToFalse()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.ShowPropertiesButton, false)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.DoesNotContain("Rename", cut.Markup);
        Assert.DoesNotContain("Download", cut.Markup);
        Assert.DoesNotContain("Delete", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_HidesDetailsSwitch_WhenSetToFalse()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.ShowDetailsButton, false)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.DoesNotContain("Show details", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_ShowsBusyIndicator_WhenIsBusyIsTrue()
    {
        // Act
        var cut = RenderFileManager(p => p.Add(x => x.IsBusy, true)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.Contains("progress", cut.Markup, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FluentCxFileManager_RendersMobileSpecificElements_WhenViewIsMobile()
    {
        // Act
        var cut = RenderFileManager(p => p
            .Add(x => x.View, FileManagerView.Mobile)
            .Add(x => x.ShowDetailsButton, true)
            .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        var detailsButton = cut.FindAll("fluent-button[title='Show details'], fluent-button[aria-label='Show details']");
        Assert.NotEmpty(detailsButton);
        Assert.Empty(cut.FindAll("fluent-switch"));
        Assert.DoesNotContain("fluent-tree-view", cut.Markup, System.StringComparison.OrdinalIgnoreCase);
        var typeLabels = cut.FindAll("fluent-label").Where(l => l.TextContent.Contains("Type", System.StringComparison.OrdinalIgnoreCase));
        Assert.Empty(typeLabels);
    }

    [Fact]
    public void FluentCxFileManager_RendersDesktopSpecificElements_WhenViewIsDesktop()
    {
        // Act
        var cut = RenderFileManager(p => p
            .Add(x => x.View, FileManagerView.Desktop)
            .Add(x => x.ShowDetailsButton, true)
            .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.Contains("Move to", cut.Markup);
        var detailsSwitch = cut.FindAll("fluent-switch");
        Assert.NotEmpty(detailsSwitch);
        Assert.Contains("Show details", detailsSwitch[0].OuterHtml);
        Assert.Empty(cut.FindAll("fluent-button[title='Show details'], fluent-button[aria-label='Show details']"));
        Assert.Contains("fluent-tree-view", cut.Markup, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Details", cut.Markup, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FluentCxFileManager_AppliesAcceptParameter()
    {
        // Arrange
        var accept = ".pdf,.docx";

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.Accept, accept)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        var input = cut.Find("input[type='file']");
        Assert.Equal(accept, input.GetAttribute("accept"));
    }

    [Fact]
    public void FluentCxFileManager_ExceedsMaximumFileCount_ShowsErrorDialog()
    {
        // Arrange
        var mockDialogService = new Mock<IDialogService>();
        Services.AddScoped(_ => mockDialogService.Object);

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.MaximumFileCount, 1)
        .Add(x => x.Root, FileManagerEntry<object>.Home));
        var inputFile = cut.FindComponent<InputFile>();
        var files = new[]
        {
            Bunit.InputFileContent.CreateFromText("file1 content", "file1.txt"),
            Bunit.InputFileContent.CreateFromText("file2 content", "file2.txt")
        };
        inputFile.UploadFiles(files);

        // Assert
        mockDialogService.Verify(s => s.ShowErrorAsync(
            Moq.It.IsAny<string>(),
            Moq.It.IsAny<string>(),
            Moq.It.IsAny<string>()),
            Moq.Times.AtLeastOnce());
    }

    [Fact]
    public void FluentCxFileManager_FileLargerThanMaximumFileSize_IsNotUploaded()
    {
        // Arrange
        var mockDialogService = new Mock<IDialogService>();
        Services.AddScoped(_ => mockDialogService.Object);

        var onFileUploadedCalled = false;
        void OnFileUploadedHandler(FileManagerEntry<object> _) => onFileUploadedCalled = true;

        // Act
        var cut = RenderFileManager(p => p
            .Add(x => x.MaximumFileSize, 10L)
            .Add(x => x.Root, FileManagerEntry<object>.Home)
            .Add(x => x.OnFileUploaded, EventCallback.Factory.Create<FileManagerEntry<object>>(this, (Action<FileManagerEntry<object>>)OnFileUploadedHandler)));

        var inputFile = cut.FindComponent<InputFile>();
        var files = new[]
        {
            Bunit.InputFileContent.CreateFromText(new string('a', 20), "largefile.txt")
        };
        inputFile.UploadFiles(files);

        // Assert
        Assert.False(onFileUploadedCalled);
    }

    [Fact]
    public void FluentCxFileManager_AcceptsCustomBufferSize()
    {
        // Arrange
        var mockDialogService = new Mock<IDialogService>();
        Services.AddScoped(_ => mockDialogService.Object);

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.BufferSize, 4096u)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.NotNull(cut);
    }

    [Theory]
    [InlineData(AcceptFile.Audio, "audio/*")]
    [InlineData(AcceptFile.Image, "image/*")]
    [InlineData(AcceptFile.Video, "video/*")]
    [InlineData(AcceptFile.Pdf, ".pdf")]
    [InlineData(AcceptFile.Excel, ".xls, .xlsx")]
    [InlineData(AcceptFile.Word, ".doc, .docx")]
    [InlineData(AcceptFile.Powerpoint, ".ppt, .pptx")]
    [InlineData(AcceptFile.Document, ".xls, .xlsx, .doc, .docx, .ppt, .pptx, .pdf")]
    [InlineData(AcceptFile.Audio | AcceptFile.Image, "audio/*, image/*")]
    public void FluentCxFileManager_AcceptFiles_SetsCorrectAcceptAttribute(AcceptFile acceptFile, string expectedAccept)
    {
        // Arrange
        var mockDialogService = new Mock<IDialogService>();
        Services.AddScoped(_ => mockDialogService.Object);

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.AcceptFiles, acceptFile)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        var input = cut.Find("input[type='file']");
        var actualAccept = input.GetAttribute("accept");
        var expectedSet = expectedAccept.Split(",").Select(s => s.Trim()).ToHashSet();
        var actualSet = actualAccept.Split(",").Select(s => s.Trim()).ToHashSet();
        Assert.Equal(expectedSet, actualSet);
    }

    [Fact]
    public void FluentCxFileManager_OnFolderCreated_IsInvoked_WhenNewFolderCreated()
    {
        // Arrange
        var folderName = "TestFolder";
        var callbackInvoked = false;
        CreateFileManagerEntryEventArgs<object>? receivedArgs = null;

        var mockDialogService = new Mock<IDialogService>();
        var mockDialogReference = new Mock<IDialogReference>();
        mockDialogReference.SetupGet(r => r.Result).Returns(System.Threading.Tasks.Task.FromResult(DialogResult.Ok(folderName)));
        mockDialogService
            .Setup(s => s.ShowDialogAsync<FileManagerDialog>(It.IsAny<object>(), It.IsAny<DialogParameters>()))
            .ReturnsAsync(mockDialogReference.Object);
        Services.AddScoped(_ => mockDialogService.Object);

        var root = FileManagerEntry<object>.Home;

        // Act
        var cut = RenderFileManager(p => p
            .Add(x => x.Root, root)
            .Add(x => x.OnFolderCreated, EventCallback.Factory.Create<CreateFileManagerEntryEventArgs<object>>(this, (Action<CreateFileManagerEntryEventArgs<object>>)(args => { callbackInvoked = true; receivedArgs = args; }))));

        var newFolderButton = cut.FindAll("button, fluent-button").FirstOrDefault(b => b.TextContent.Contains("New Folder"));
        Assert.NotNull(newFolderButton);
        newFolderButton.Click();

        // Assert
        Assert.True(callbackInvoked);
        Assert.NotNull(receivedArgs);
        Assert.Equal(folderName, receivedArgs!.Entry.Name);
    }

    [Fact]
    public void FluentCxFileManager_OnRename_IsInvoked_WhenFileRenamed()
    {
        // Arrange
        var originalName = "file1.txt";
        var newName = "renamed";
        var callbackInvoked = false;
        FileManagerEntry<TestRenamable>? renamedEntry = null;

        var mockDialogService = new Mock<IDialogService>();
        var mockDialogReference = new Mock<IDialogReference>();
        mockDialogReference.SetupGet(r => r.Result).Returns(System.Threading.Tasks.Task.FromResult<DialogResult>(DialogResult.Ok(newName)));
        mockDialogService
            .Setup(s => s.ShowDialogAsync<FileManagerDialog>(It.IsAny<object>(), It.IsAny<DialogParameters>()))
            .ReturnsAsync(mockDialogReference.Object);
        Services.AddScoped(_ => mockDialogService.Object);

        var root = FileManagerEntry<TestRenamable>.Home;
        var fileEntry = root.CreateDefaultFileEntryWithData(new TestRenamable(), new byte[0], originalName, 0, DateTime.Now, DateTime.Now);

        // Act
        var cut = RenderFileManagerComponent<TestRenamable>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.OnRename, EventCallback.Factory.Create<FileManagerEntry<TestRenamable>>(this, (Action<FileManagerEntry<TestRenamable>>)(entry => { callbackInvoked = true; renamedEntry = entry; }))));

        cut.Instance.GetType().GetField("_currentSelectedItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(cut.Instance, new[] { fileEntry });
        cut.Render();

        var renameButton = cut.FindAll("button, fluent-button").FirstOrDefault(b => b.GetAttribute("title") == "Rename");
        Assert.NotNull(renameButton);
        Assert.False(renameButton.HasAttribute("disabled"));
        renameButton.Click();

        // Assert
        Assert.True(callbackInvoked);
        Assert.NotNull(renamedEntry);
        Assert.Equal(newName + ".txt", renamedEntry!.Name);
    }

    [Fact]
    public void FluentCxFileManager_OnDelete_IsInvoked_WhenFileDeleted()
    {
        // Arrange
        var fileName = "file1.txt";
        var callbackInvoked = false;
        DeleteFileManagerEntryEventArgs<TestDeletable>? deleteArgs = null;

        var mockDialogService = new Mock<IDialogService>();
        var mockDialogReference = new Mock<IDialogReference>();
        mockDialogReference.SetupGet(r => r.Result).Returns(System.Threading.Tasks.Task.FromResult<DialogResult>(DialogResult.Ok(true)));
        mockDialogService
            .Setup(s => s.ShowConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(mockDialogReference.Object);
        Services.AddScoped(_ => mockDialogService.Object);

        var root = FileManagerEntry<TestDeletable>.Home;
        var fileEntry = root.CreateDefaultFileEntryWithData(new TestDeletable(), new byte[0], fileName, 0, DateTime.Now, DateTime.Now);

        // Act
        var cut = RenderFileManagerComponent<TestDeletable>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.OnDelete, EventCallback.Factory.Create<DeleteFileManagerEntryEventArgs<TestDeletable>>(this, (Action<DeleteFileManagerEntryEventArgs<TestDeletable>>)(args => { callbackInvoked = true; deleteArgs = args; }))));

        cut.Instance.GetType().GetField("_currentSelectedItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(cut.Instance, new[] { fileEntry });
        cut.Render();

        var deleteButton = cut.FindAll("button, fluent-button").FirstOrDefault(b => b.GetAttribute("title") == "Delete");
        Assert.NotNull(deleteButton);
        Assert.False(deleteButton.HasAttribute("disabled"));
        deleteButton.Click();

        // Assert
        Assert.True(callbackInvoked);
        Assert.NotNull(deleteArgs);
        Assert.Contains(fileEntry, deleteArgs!.Entries);
    }

    [Fact]
    public void FluentCxFileManager_OnFileUploaded_IsInvoked_WhenFileUploaded()
    {
        // Arrange
        var fileName = "uploaded.txt";
        var callbackInvoked = false;
        FileManagerEntry<object>? uploadedEntry = null;

        var root = FileManagerEntry<object>.Home;

        // Act
        var cut = RenderFileManagerComponent<object>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.OnFileUploaded, EventCallback.Factory.Create<FileManagerEntry<object>>(this, (Action<FileManagerEntry<object>>)(entry => { callbackInvoked = true; uploadedEntry = entry; }))));

        var inputFile = cut.FindComponent<InputFile>();
        var files = new[]
        {
            Bunit.InputFileContent.CreateFromText("file content", fileName)
        };
        inputFile.UploadFiles(files);

        // Assert
        Assert.True(callbackInvoked);
        Assert.NotNull(uploadedEntry);
        Assert.Equal(fileName, uploadedEntry!.Name);
    }

    [Fact]
    public void FluentCxFileManager_UsesCustomFileManagerLabels()
    {
        // Arrange
        var customLabels = new FileManagerLabels
        {
            NewFolderLabel = "Custom New Folder",
            UploadLabel = "Custom Upload",
            ViewLabel = "Custom View",
            SortLabel = "Custom Sort",
            RenameLabel = "Custom Rename",
            DownloadLabel = "Custom Download",
            DeleteLabel = "Custom Delete",
            ShowDetailsLabel = "Custom Show Details",
            SearchPlaceholder = "Custom Search..."
        };

        // Act
        var cut = RenderFileManager(p => p.Add(x => x.FileManagerLabels, customLabels)
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.Contains("Custom New Folder", cut.Markup);
        Assert.Contains("Custom Upload", cut.Markup);
        Assert.Contains("Custom View", cut.Markup);
        Assert.Contains("Custom Sort", cut.Markup);
        Assert.Contains("Custom Rename", cut.Markup);
        Assert.Contains("Custom Download", cut.Markup);
        Assert.Contains("Custom Delete", cut.Markup);
        Assert.Contains("Custom Show Details", cut.Markup);
        Assert.Contains("Custom Search...", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_UsesCustomDetailsLabels()
    {
        // Arrange
        var customDetailsLabels = new FileManagerDetailsLabels
        {
            Details = "Custom Details Label",
            Type = "Custom Type Label",
            Size = "Custom Size Label"
        };

        var root = CreateTestFileEntry();

        // Act
        var cut = RenderFileManagerComponent<object>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.DetailsLabels, customDetailsLabels)
            .Add(x => x.ShowDetailsButton, true)
            .Add(x => x.View, FileManagerView.Desktop));

        var field = cut.Instance.GetType().GetField("_showDetails", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field!.SetValue(cut.Instance, true);
        cut.Render();

        // Assert
        Assert.Contains("Custom Size Label", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_UsesCustomColumnLabels()
    {
        // Arrange
        var customColumnLabels = FileListDataGridColumnLabels.Default with
        {
            Name = "Custom Name Label",
            Size = "Custom Size Label",
            CreatedDate = "Custom Date Label"
        };

        var root = CreateTestFileEntry();

        // Act
        var cut = RenderFileManagerComponent<object>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.ColumnLabels, customColumnLabels)
            .Add(x => x.ShowDetailsButton, true)
            .Add(x => x.View, FileManagerView.Desktop));

        var field = cut.Instance.GetType().GetField("_showDetails", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field!.SetValue(cut.Instance, true);
        cut.Render();

        // Assert
        Assert.Contains("Custom Name Label", cut.Markup);
    }

    [Fact]
    public void FluentCxFileManager_RendersCustomToolbarItems()
    {
        // Arrange
        var customContent = "<span id='custom-toolbar-item'>Custom Toolbar Content</span>";

        // Act
        var cut = RenderFileManagerComponent<object>(p => p.Add(x => x.ToolbarItems, (RenderFragment)(builder =>
        {
            builder.AddMarkupContent(0, customContent);
        }))
        .Add(x => x.Root, FileManagerEntry<object>.Home));

        // Assert
        Assert.Contains("Custom Toolbar Content", cut.Markup);
        Assert.NotNull(cut.Find("#custom-toolbar-item"));
    }

    [Fact]
    public void FluentCxFileManager_FileStructureView_RendersCorrectly()
    {
        // Arrange
        var root = CreateTestFileEntry();

        // Act & Assert: Hierarchical view should show the tree view
        var cutHierarchical = RenderFileManagerComponent<object>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.FileStructureView, FileStructureView.Hierarchical)
            .Add(x => x.View, FileManagerView.Desktop));
        Assert.Contains("fluent-tree-view", cutHierarchical.Markup);

        // Act & Assert: Flat view should NOT show the tree view
        var cutFlat = RenderFileManagerComponent<object>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.FileStructureView, FileStructureView.Flat)
            .Add(x => x.View, FileManagerView.Desktop));
        Assert.DoesNotContain("fluent-tree-view", cutFlat.Markup);
    }

    [Fact]
    public void FluentCxFileManager_MovedCallback_IsInvoked_WhenFileMoved()
    {
        // Arrange
        var callbackInvoked = false;
        FileManagerEntriesMovedEventArgs<object>? receivedArgs = null;

        var mockDialogService = new Mock<IDialogService>();
        var mockDialogReference = new Mock<IDialogReference>();

        var root = FileManagerEntry<object>.Home;
        var destinationFolder = root.CreateDirectory("DestFolder");
        var fileEntry = root.CreateDefaultFileEntryWithData(new object(), new byte[0], "file.txt", 123, DateTime.Now, DateTime.Now);
        root.AddRange(fileEntry);
        root.AddRange(destinationFolder);

        mockDialogReference.SetupGet(r => r.Result).Returns(System.Threading.Tasks.Task.FromResult(DialogResult.Ok(destinationFolder)));
        mockDialogService
            .Setup(s => s.ShowDialogAsync<FileMoverDialog<object>>(Moq.It.IsAny<object>(), Moq.It.IsAny<DialogParameters>()))
            .ReturnsAsync(mockDialogReference.Object);
        Services.AddScoped(_ => mockDialogService.Object);

        // Act
        var cut = RenderFileManagerComponent<object>(p => p
            .Add(x => x.Root, root)
            .Add(x => x.Moved, EventCallback.Factory.Create<FileManagerEntriesMovedEventArgs<object>>(this, (Action<FileManagerEntriesMovedEventArgs<object>>)(args => { callbackInvoked = true; receivedArgs = args; }))));

        cut.Instance.GetType().GetField("_currentSelectedItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(cut.Instance, new[] { fileEntry });
        cut.Render();

        var moveButton = cut.FindAll("button, fluent-button").FirstOrDefault(b => b.GetAttribute("title") == "Move to");
        Assert.NotNull(moveButton);
        Assert.False(moveButton.HasAttribute("disabled"));
        moveButton.Click();

        // Assert
        Assert.True(callbackInvoked);
        Assert.NotNull(receivedArgs);
        Assert.Equal(destinationFolder, receivedArgs!.DestinationFolder);
        Assert.Contains(fileEntry, receivedArgs!.MovedEntries);
    }
} 
