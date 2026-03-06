using Microsoft.FluentUI.AspNetCore.Components;
using static FluentUI.Blazor.Community.Components.FileIcons;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Creates icons based on a FileIconKey and a FileView.
/// Developers can override any icon for any view.
/// </summary>
public static class FileIconFactory
{
    /// <summary>
    /// Stores developer overrides: (key, view) → icon instance.
    /// </summary>
    private static readonly Dictionary<(string Key, FileView View), Icon> _overrides = new(FileIconKeyComparer.Instance);

    /// <summary>
    /// Stores the built‑in icon mappings: (key, view) → icon instance.
    /// This dictionary is fully initialized at startup.
    /// </summary>
    private static readonly Dictionary<(string Key, FileView View), Icon> _builtIn = new(FileIconKeyComparer.Instance)
    {
        [(FileIconKey.Excel.Key, FileView.SmallIcons)] = new Size24.ExcelIcon(),
        [(FileIconKey.Excel.Key, FileView.Mosaic)] = new Size32.ExcelIcon(),
        [(FileIconKey.Excel.Key, FileView.MediumIcons)] = new Size72.ExcelIcon(),
        [(FileIconKey.Excel.Key, FileView.LargeIcons)] = new Size96.ExcelIcon(),
        [(FileIconKey.Excel.Key, FileView.VeryLargeIcons)] = new Size128.ExcelIcon(),
        [(FileIconKey.Excel.Key, FileView.Details)] = new Size32.ExcelIcon(),
        [(FileIconKey.Excel.Key, FileView.List)] = new Size32.ExcelIcon(),

        [(FileIconKey.Word.Key, FileView.SmallIcons)] = new Size24.WordIcon(),
        [(FileIconKey.Word.Key, FileView.Mosaic)] = new Size32.WordIcon(),
        [(FileIconKey.Word.Key, FileView.MediumIcons)] = new Size72.WordIcon(),
        [(FileIconKey.Word.Key, FileView.LargeIcons)] = new Size96.WordIcon(),
        [(FileIconKey.Word.Key, FileView.VeryLargeIcons)] = new Size128.WordIcon(),
        [(FileIconKey.Word.Key, FileView.Details)] = new Size32.WordIcon(),
        [(FileIconKey.Word.Key, FileView.List)] = new Size32.WordIcon(),

        [(FileIconKey.PowerPoint.Key, FileView.SmallIcons)] = new Size24.PowerpointIcon(),
        [(FileIconKey.PowerPoint.Key, FileView.Mosaic)] = new Size32.PowerpointIcon(),
        [(FileIconKey.PowerPoint.Key, FileView.MediumIcons)] = new Size72.PowerpointIcon(),
        [(FileIconKey.PowerPoint.Key, FileView.LargeIcons)] = new Size96.PowerpointIcon(),
        [(FileIconKey.PowerPoint.Key, FileView.VeryLargeIcons)] = new Size128.PowerpointIcon(),
        [(FileIconKey.PowerPoint.Key, FileView.Details)] = new Size32.PowerpointIcon(),
        [(FileIconKey.PowerPoint.Key, FileView.List)] = new Size32.PowerpointIcon(),

        [(FileIconKey.Image.Key, FileView.SmallIcons)] = new Size24.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.Mosaic)] = new Size32.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.MediumIcons)] = new Size72.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.LargeIcons)] = new Size96.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.LargeIcons)] = new Size96.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.VeryLargeIcons)] = new Size128.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.Details)] = new Size32.ImageIcon(),
        [(FileIconKey.Image.Key, FileView.List)] = new Size32.ImageIcon(),

        [(FileIconKey.Audio.Key, FileView.SmallIcons)] = new Size24.MusicIcon(),
        [(FileIconKey.Audio.Key, FileView.Mosaic)] = new Size32.MusicIcon(),
        [(FileIconKey.Audio.Key, FileView.MediumIcons)] = new Size72.MusicIcon(),
        [(FileIconKey.Audio.Key, FileView.LargeIcons)] = new Size96.MusicIcon(),
        [(FileIconKey.Audio.Key, FileView.VeryLargeIcons)] = new Size128.MusicIcon(),
        [(FileIconKey.Audio.Key, FileView.Details)] = new Size32.MusicIcon(),
        [(FileIconKey.Audio.Key, FileView.List)] = new Size32.MusicIcon(),

        [(FileIconKey.Video.Key, FileView.SmallIcons)] = new Size24.VideoIcon(),
        [(FileIconKey.Video.Key, FileView.Mosaic)] = new Size32.VideoIcon(),
        [(FileIconKey.Video.Key, FileView.MediumIcons)] = new Size72.VideoIcon(),
        [(FileIconKey.Video.Key, FileView.LargeIcons)] = new Size96.VideoIcon(),
        [(FileIconKey.Video.Key, FileView.VeryLargeIcons)] = new Size128.VideoIcon(),
        [(FileIconKey.Video.Key, FileView.Details)] = new Size32.VideoIcon(),
        [(FileIconKey.Video.Key, FileView.List)] = new Size32.VideoIcon(),

        [(FileIconKey.Pdf.Key, FileView.SmallIcons)] = new Size24.PdfIcon(),
        [(FileIconKey.Pdf.Key, FileView.Mosaic)] = new Size32.PdfIcon(),
        [(FileIconKey.Pdf.Key, FileView.MediumIcons)] = new Size72.PdfIcon(),
        [(FileIconKey.Pdf.Key, FileView.LargeIcons)] = new Size96.PdfIcon(),
        [(FileIconKey.Pdf.Key, FileView.VeryLargeIcons)] = new Size128.PdfIcon(),
        [(FileIconKey.Pdf.Key, FileView.Details)] = new Size32.PdfIcon(),
        [(FileIconKey.Pdf.Key, FileView.List)] = new Size32.PdfIcon(),

        [(FileIconKey.Json.Key, FileView.SmallIcons)] = new Size24.JsonIcon(),
        [(FileIconKey.Json.Key, FileView.Mosaic)] = new Size32.JsonIcon(),
        [(FileIconKey.Json.Key, FileView.MediumIcons)] = new Size72.JsonIcon(),
        [(FileIconKey.Json.Key, FileView.LargeIcons)] = new Size96.JsonIcon(),
        [(FileIconKey.Json.Key, FileView.VeryLargeIcons)] = new Size128.JsonIcon(),
        [(FileIconKey.Json.Key, FileView.Details)] = new Size32.JsonIcon(),
        [(FileIconKey.Json.Key, FileView.List)] = new Size32.JsonIcon(),

        [(FileIconKey.Program.Key, FileView.SmallIcons)] = new Size24.ProgramIcon(),
        [(FileIconKey.Program.Key, FileView.Mosaic)] = new Size32.ProgramIcon(),
        [(FileIconKey.Program.Key, FileView.MediumIcons)] = new Size72.ProgramIcon(),
        [(FileIconKey.Program.Key, FileView.LargeIcons)] = new Size96.ProgramIcon(),
        [(FileIconKey.Program.Key, FileView.VeryLargeIcons)] = new Size128.ProgramIcon(),
        [(FileIconKey.Program.Key, FileView.Details)] = new Size32.ProgramIcon(),
        [(FileIconKey.Program.Key, FileView.List)] = new Size32.ProgramIcon(),

        [(FileIconKey.PowerBi.Key, FileView.SmallIcons)] = new Size24.PowerBiIcon(),
        [(FileIconKey.PowerBi.Key, FileView.Mosaic)] = new Size32.PowerBiIcon(),
        [(FileIconKey.PowerBi.Key, FileView.MediumIcons)] = new Size72.PowerBiIcon(),
        [(FileIconKey.PowerBi.Key, FileView.LargeIcons)] = new Size96.PowerBiIcon(),
        [(FileIconKey.PowerBi.Key, FileView.VeryLargeIcons)] = new Size128.PowerBiIcon(),
        [(FileIconKey.PowerBi.Key, FileView.Details)] = new Size32.PowerBiIcon(),
        [(FileIconKey.PowerBi.Key, FileView.List)] = new Size32.PowerBiIcon(),

        [(FileIconKey.Folder.Key, FileView.SmallIcons)] = new Size24.FolderIcon(),
        [(FileIconKey.Folder.Key, FileView.Mosaic)] = new Size32.FolderIcon(),
        [(FileIconKey.Folder.Key, FileView.MediumIcons)] = new Size72.FolderIcon(),
        [(FileIconKey.Folder.Key, FileView.LargeIcons)] = new Size96.FolderIcon(),
        [(FileIconKey.Folder.Key, FileView.VeryLargeIcons)] = new Size128.FolderIcon(),
        [(FileIconKey.Folder.Key, FileView.Details)] = new Size32.FolderIcon(),
        [(FileIconKey.Folder.Key, FileView.List)] = new Size32.FolderIcon(),

        [(FileIconKey.MultiSelection.Key, FileView.SmallIcons)] = new Size24.MultiSelectionIcon(),
        [(FileIconKey.MultiSelection.Key, FileView.Mosaic)] = new Size32.MultiSelectionIcon(),
        [(FileIconKey.MultiSelection.Key, FileView.MediumIcons)] = new Size72.MultiSelectionIcon(),
        [(FileIconKey.MultiSelection.Key, FileView.LargeIcons)] = new Size96.MultiSelectionIcon(),
        [(FileIconKey.MultiSelection.Key, FileView.VeryLargeIcons)] = new Size128.MultiSelectionIcon(),
        [(FileIconKey.MultiSelection.Key, FileView.Details)] = new Size32.MultiSelectionIcon(),
        [(FileIconKey.MultiSelection.Key, FileView.List)] = new Size32.MultiSelectionIcon(),
    };

    /// <summary>
    /// Allows developers to override the icon for a specific key and view.
    /// </summary>
    public static void Override(FileIconKey key, FileView view, Icon icon)
        => _overrides[(key.Key, view)] = icon;

    /// <summary>
    /// Internal creation logic. Always uses overrides first, then built‑ins, then fallback.
    /// </summary>
    public static Icon Get(FileIconKey key, FileView view)
    {
        if (_overrides.TryGetValue((key.Key, view), out var custom))
        {
            return custom;
        }

        if (_builtIn.TryGetValue((key.Key, view), out var builtin))
        {
            return builtin;
        }

        return view switch
        {
            FileView.SmallIcons => new Size24.DefaultFileIcon(),
            FileView.Mosaic => new Size32.DefaultFileIcon(),
            FileView.MediumIcons => new Size72.DefaultFileIcon(),
            FileView.LargeIcons => new Size96.DefaultFileIcon(),
            FileView.VeryLargeIcons => new Size128.DefaultFileIcon(),
            FileView.List => new Size32.DefaultFileIcon(),
            FileView.Details => new Size32.DefaultFileIcon(),
            _ => new Size32.DefaultFileIcon()
        };
    }
}
