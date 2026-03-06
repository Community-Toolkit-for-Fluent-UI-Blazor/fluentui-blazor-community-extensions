namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a key that identifies a specific file icon, such as for a file type or folder.
/// </summary>
/// <remarks>Use the built-in static properties for common file types, or create a custom key using the From
/// method to represent additional file icons. Instances of this class are immutable.</remarks>
public sealed class FileIconKey
{
    /// <summary>
    /// Gets the unique identifier associated with this instance.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Initializes a new instance of the FileIconKey class with the specified key value.
    /// </summary>
    /// <param name="key">The string value that uniquely identifies the file icon key. Cannot be null.</param>
    private FileIconKey(string key) => Key = key;

    /// <summary>
    /// Creates a new instance of the <see cref="FileIconKey"/> class using the specified key.
    /// </summary>
    /// <param name="key">The string value that uniquely identifies the file icon. Cannot be null.</param>
    /// <returns>A new FileIconKey instance initialized with the specified key.</returns>
    public static FileIconKey Create(string key) => new(key);

    /// <summary>
    /// Represents the default file icon key used when no specific icon is associated with a file type.
    /// </summary>
    /// <remarks>Use this value to display a generic file icon when a more specific icon is not
    /// available.</remarks>
    public static readonly FileIconKey Default = new("default");

    /// <summary>
    /// Represents the key for the folder file icon.
    /// </summary>
    public static readonly FileIconKey Folder = new("folder");

    /// <summary>
    /// Represents the file icon key for Microsoft Excel files.
    /// </summary>
    public static readonly FileIconKey Excel = new("excel");

    /// <summary>
    /// Represents the file icon key for Microsoft Word documents.
    /// </summary>
    public static readonly FileIconKey Word = new("word");

    /// <summary>
    /// Represents the file icon key for Microsoft PowerPoint documents.
    /// </summary>
    public static readonly FileIconKey PowerPoint = new("powerpoint");

    /// <summary>
    /// Represents the file icon key for image files.
    /// </summary>
    public static readonly FileIconKey Image = new("image");

    /// <summary>
    /// Represents the file icon key for audio files.
    /// </summary>
    public static readonly FileIconKey Audio = new("audio");

    /// <summary>
    /// Represents the file icon key for video files.
    /// </summary>
    public static readonly FileIconKey Video = new("video");

    /// <summary>
    /// Represents the file icon key for PDF documents.
    /// </summary>
    public static readonly FileIconKey Pdf = new("pdf");

    /// <summary>
    /// Represents the file icon key for JSON files.
    /// </summary>
    public static readonly FileIconKey Json = new("json");

    /// <summary>
    /// Represents the icon key for a Power BI file type.
    /// </summary>
    public static readonly FileIconKey PowerBi = new("powerbi");

    /// <summary>
    /// Represents the file icon key for program files.
    /// </summary>
    public static readonly FileIconKey Program = new("program");

    /// <summary>
    /// Represents the file icon key for multiple selected items.
    /// </summary>
    public static readonly FileIconKey MultiSelection = new("multiselection");
}

