using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a grid item within a file manager component, providing a strongly typed context for displaying and
/// interacting with file or folder data.
/// </summary>
/// <typeparam name="TItem">The type of the data item represented by this grid item. Must be a reference type with a parameterless constructor.</typeparam>
public partial class FileManagerGridItem<TItem>
    : FluentComponentBase where TItem : class, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileManagerGridItem{TItem}"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration"></param>
    public FileManagerGridItem(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the parent file manager component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a parent FluentCxFileManager. It enables child components to access shared functionality or state
    /// from the parent file manager.</remarks>
    [CascadingParameter]
    public FluentCxFileManager<TItem>? Parent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is rendered in a smaller size variant.
    /// </summary>
    [Parameter]
    public bool IsSmall { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is checked.
    /// </summary>
    [Parameter]
    public bool IsChecked { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the checked state changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the checked state, such as when a user selects or
    /// deselects the associated control. The callback receives the new checked state as a parameter.</remarks>
    [Parameter]
    public EventCallback<bool> IsCheckedChanged { get; set; }

    /// <summary>
    /// Gets or sets the file entry associated with the current item.
    /// </summary>
    /// <remarks>Use this property to access or assign the file entry data for the component. The entry
    /// typically contains information such as the file's name, content, and metadata relevant to the generic item
    /// type.</remarks>
    [Parameter]
    public FileEntry<TItem> Entry { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when a context menu event occurs on the component.
    /// </summary>
    /// <remarks>Use this property to handle right-click or context menu interactions within the component.
    /// The event provides details about the mouse action through the <see cref="MouseEventArgs"/> parameter.</remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> OnContextMenu { get; set; }

    /// <summary>
    /// Gets the CSS size value for the icon based on the current file view mode.
    /// </summary>
    /// <remarks>The returned size corresponds to the selected file view and is used to render icons at an
    /// appropriate visual scale. If the parent or its state is not available, the default size is used.</remarks>
    private string IconSize => Parent?.State.View switch
    {
        FileView.VeryLargeIcons => "128px",
        FileView.LargeIcons => "96px",
        FileView.MediumIcons => "72px",
        FileView.SmallIcons => "32px",
        _ => "128px"
    };

    /// <summary>
    /// Gets the maximum width, in pixels, allowed for a label based on the current file view.
    /// </summary>
    /// <remarks>The maximum label width varies depending on the selected file view mode. This property is
    /// typically used to ensure consistent label sizing in different icon views.</remarks>
    private int MaxLabelWidth => Parent?.State.View switch
    {
        FileView.VeryLargeIcons => 184,
        FileView.LargeIcons => 154,
        FileView.MediumIcons => 134,
        FileView.SmallIcons => 120,
        _ => 180
    };

    /// <summary>
    /// Retrieves the appropriate icon for the specified file or directory entry based on its type and extension.
    /// </summary>
    /// <param name="item">The file or directory entry for which to obtain the icon. If the entry represents a directory, a folder icon is
    /// returned; otherwise, the icon is determined by the file's extension.</param>
    /// <returns>An icon representing the file or directory entry, selected according to its type and extension.</returns>
    private Icon GetIcon(FileEntry<TItem> item)
        => item.IsDirectory
            ? FileIconFactory.Get(FileIconKey.Folder, Parent!.State.View)
            : FileIconFactory.Get(FileIconRegistry.Resolve(item.Extension), Parent!.State.View);

}
