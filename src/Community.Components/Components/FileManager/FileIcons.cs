// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public static partial class FileIcons
{
    private static readonly Dictionary<string, Icon> _iconForDetailsPanel = new()
    {
        [".xls"] = new Size128.ExcelIcon(),
        [".xlsx"] = new Size128.ExcelIcon(),
        [".xlsm"] = new Size128.ExcelIcon(),
        [".xlsb"] = new Size128.ExcelIcon(),
        [".xlam"] = new Size128.ExcelIcon(),
        [".xltx"] = new Size128.ExcelIcon(),
        [".xltm"] = new Size128.ExcelIcon(),
        [".xla"] = new Size128.ExcelIcon(),
        [".xlm"] = new Size128.ExcelIcon(),
        [".xlw"] = new Size128.ExcelIcon(),
        [".odc"] = new Size128.ExcelIcon(),
        [".ods"] = new Size128.ExcelIcon(),
        [".csv"] = new Size128.ExcelIcon(),

        [".pptx"] = new Size128.PowerpointIcon(),
        [".pptm"] = new Size128.PowerpointIcon(),
        [".ppt"] = new Size128.PowerpointIcon(),
        [".potx"] = new Size128.PowerpointIcon(),
        [".potm"] = new Size128.PowerpointIcon(),
        [".pot"] = new Size128.PowerpointIcon(),
        [".ppsx"] = new Size128.PowerpointIcon(),
        [".ppsm"] = new Size128.PowerpointIcon(),
        [".pps"] = new Size128.PowerpointIcon(),
        [".ppam"] = new Size128.PowerpointIcon(),
        [".ppa"] = new Size128.PowerpointIcon(),
        [".odp"] = new Size128.PowerpointIcon(),

        [".docx"] = new Size128.WordIcon(),
        [".doc"] = new Size128.WordIcon(),
        [".docm"] = new Size128.WordIcon(),
        [".dotx"] = new Size128.WordIcon(),
        [".dotm"] = new Size128.WordIcon(),
        [".dot"] = new Size128.WordIcon(),
        [".rtf"] = new Size128.WordIcon(),
        [".odt"] = new Size128.WordIcon(),

        [".jpg"] = new Size128.ImageIcon(),
        [".jpeg"] = new Size128.ImageIcon(),
        [".jfif"] = new Size128.ImageIcon(),
        [".pjpeg"] = new Size128.ImageIcon(),
        [".pjpg"] = new Size128.ImageIcon(),
        [".png"] = new Size128.ImageIcon(),
        [".bmp"] = new Size128.ImageIcon(),
        [".apng"] = new Size128.ImageIcon(),
        [".avif"] = new Size128.ImageIcon(),
        [".gif"] = new Size128.ImageIcon(),
        [".svg"] = new Size128.ImageIcon(),
        [".webp"] = new Size128.ImageIcon(),
        [".ico"] = new Size128.ImageIcon(),
        [".cur"] = new Size128.ImageIcon(),
        [".tif"] = new Size128.ImageIcon(),
        [".tiff"] = new Size128.ImageIcon(),

        [".3gp"] = new Size128.MusicIcon(),
        [".aa"] = new Size128.MusicIcon(),
        [".aac"] = new Size128.MusicIcon(),
        [".aax"] = new Size128.MusicIcon(),
        [".act"] = new Size128.MusicIcon(),
        [".aiff"] = new Size128.MusicIcon(),
        [".alac"] = new Size128.MusicIcon(),
        [".amr"] = new Size128.MusicIcon(),
        [".ape"] = new Size128.MusicIcon(),
        [".au"] = new Size128.MusicIcon(),
        [".awb"] = new Size128.MusicIcon(),
        [".dss"] = new Size128.MusicIcon(),
        [".dvf"] = new Size128.MusicIcon(),
        [".flac"] = new Size128.MusicIcon(),
        [".gsm"] = new Size128.MusicIcon(),
        [".iklax"] = new Size128.MusicIcon(),
        [".ivs"] = new Size128.MusicIcon(),
        [".m4a"] = new Size128.MusicIcon(),
        [".m4b"] = new Size128.MusicIcon(),
        [".m4p"] = new Size128.MusicIcon(),
        [".mmf"] = new Size128.MusicIcon(),
        [".movpkg"] = new Size128.MusicIcon(),
        [".mp1"] = new Size128.MusicIcon(),
        [".mp2"] = new Size128.MusicIcon(),
        [".mp3"] = new Size128.MusicIcon(),
        [".mpc"] = new Size128.MusicIcon(),
        [".msv"] = new Size128.MusicIcon(),
        [".nmf"] = new Size128.MusicIcon(),
        [".ogg"] = new Size128.MusicIcon(),
        [".oga"] = new Size128.MusicIcon(),
        [".mogg"] = new Size128.MusicIcon(),
        [".opus"] = new Size128.MusicIcon(),
        [".ra"] = new Size128.MusicIcon(),
        [".rm"] = new Size128.MusicIcon(),
        [".raw"] = new Size128.MusicIcon(),
        [".rf64"] = new Size128.MusicIcon(),
        [".tta"] = new Size128.MusicIcon(),
        [".voc"] = new Size128.MusicIcon(),
        [".vox"] = new Size128.MusicIcon(),
        [".wav"] = new Size128.MusicIcon(),
        [".wma"] = new Size128.MusicIcon(),
        [".wv"] = new Size128.MusicIcon(),
        [".webm"] = new Size128.MusicIcon(),
        [".8svx"] = new Size128.MusicIcon(),
        [".cda"] = new Size128.MusicIcon(),

        [".webm"] = new Size128.VideoIcon(),
        [".mkv"] = new Size128.VideoIcon(),
        [".flv"] = new Size128.VideoIcon(),
        [".vob"] = new Size128.VideoIcon(),
        [".ogv"] = new Size128.VideoIcon(),
        [".drc"] = new Size128.VideoIcon(),
        [".gifv"] = new Size128.VideoIcon(),
        [".avi"] = new Size128.VideoIcon(),
        [".mts"] = new Size128.VideoIcon(),
        [".m2ts"] = new Size128.VideoIcon(),
        [".ts"] = new Size128.VideoIcon(),
        [".mov"] = new Size128.VideoIcon(),
        [".qt"] = new Size128.VideoIcon(),
        [".wmv"] = new Size128.VideoIcon(),
        [".yuv"] = new Size128.VideoIcon(),
        [".rmvb"] = new Size128.VideoIcon(),
        [".viv"] = new Size128.VideoIcon(),
        [".asf"] = new Size128.VideoIcon(),
        [".amv"] = new Size128.VideoIcon(),
        [".mp4"] = new Size128.VideoIcon(),
        [".m4p"] = new Size128.VideoIcon(),
        [".m4v"] = new Size128.VideoIcon(),
        [".mpv"] = new Size128.VideoIcon(),
        [".svi"] = new Size128.VideoIcon(),
        [".3g2"] = new Size128.VideoIcon(),
        [".mxf"] = new Size128.VideoIcon(),
        [".roq"] = new Size128.VideoIcon(),
        [".nsv"] = new Size128.VideoIcon(),

        [".pdf"] = new Size128.PdfIcon(),

        [".json"] = new Size128.JsonIcon(),

        [".pbix"] = new Size128.PowerBiIcon(),
        ["Default"] = new Size128.DefaultFileIcon(),
    };

    private static readonly Dictionary<string, Icon> _iconsFromExtension = new()
    {
        [".xls"] = new Size32.ExcelIcon(),
        [".xlsx"] = new Size32.ExcelIcon(),
        [".xlsm"] = new Size32.ExcelIcon(),
        [".xlsb"] = new Size32.ExcelIcon(),
        [".xlam"] = new Size32.ExcelIcon(),
        [".xltx"] = new Size32.ExcelIcon(),
        [".xltm"] = new Size32.ExcelIcon(),
        [".xla"] = new Size32.ExcelIcon(),
        [".xlm"] = new Size32.ExcelIcon(),
        [".xlw"] = new Size32.ExcelIcon(),
        [".odc"] = new Size32.ExcelIcon(),
        [".ods"] = new Size32.ExcelIcon(),
        [".csv"] = new Size32.ExcelIcon(),

        [".pptx"] = new Size32.PowerpointIcon(),
        [".pptm"] = new Size32.PowerpointIcon(),
        [".ppt"] = new Size32.PowerpointIcon(),
        [".potx"] = new Size32.PowerpointIcon(),
        [".potm"] = new Size32.PowerpointIcon(),
        [".pot"] = new Size32.PowerpointIcon(),
        [".ppsx"] = new Size32.PowerpointIcon(),
        [".ppsm"] = new Size32.PowerpointIcon(),
        [".pps"] = new Size32.PowerpointIcon(),
        [".ppam"] = new Size32.PowerpointIcon(),
        [".ppa"] = new Size32.PowerpointIcon(),
        [".odp"] = new Size32.PowerpointIcon(),

        [".docx"] = new Size32.WordIcon(),
        [".doc"] = new Size32.WordIcon(),
        [".docm"] = new Size32.WordIcon(),
        [".dotx"] = new Size32.WordIcon(),
        [".dotm"] = new Size32.WordIcon(),
        [".dot"] = new Size32.WordIcon(),
        [".rtf"] = new Size32.WordIcon(),
        [".odt"] = new Size32.WordIcon(),

        [".jpg"] = new Size32.ImageIcon(),
        [".jpeg"] = new Size32.ImageIcon(),
        [".jfif"] = new Size32.ImageIcon(),
        [".pjpeg"] = new Size32.ImageIcon(),
        [".pjpg"] = new Size32.ImageIcon(),
        [".png"] = new Size32.ImageIcon(),
        [".bmp"] = new Size32.ImageIcon(),
        [".apng"] = new Size32.ImageIcon(),
        [".avif"] = new Size32.ImageIcon(),
        [".gif"] = new Size32.ImageIcon(),
        [".svg"] = new Size32.ImageIcon(),
        [".webp"] = new Size32.ImageIcon(),
        [".ico"] = new Size32.ImageIcon(),
        [".cur"] = new Size32.ImageIcon(),
        [".tif"] = new Size32.ImageIcon(),
        [".tiff"] = new Size32.ImageIcon(),

        [".3gp"] = new Size32.MusicIcon(),
        [".aa"] = new Size32.MusicIcon(),
        [".aac"] = new Size32.MusicIcon(),
        [".aax"] = new Size32.MusicIcon(),
        [".act"] = new Size32.MusicIcon(),
        [".aiff"] = new Size32.MusicIcon(),
        [".alac"] = new Size32.MusicIcon(),
        [".amr"] = new Size32.MusicIcon(),
        [".ape"] = new Size32.MusicIcon(),
        [".au"] = new Size32.MusicIcon(),
        [".awb"] = new Size32.MusicIcon(),
        [".dss"] = new Size32.MusicIcon(),
        [".dvf"] = new Size32.MusicIcon(),
        [".flac"] = new Size32.MusicIcon(),
        [".gsm"] = new Size32.MusicIcon(),
        [".iklax"] = new Size32.MusicIcon(),
        [".ivs"] = new Size32.MusicIcon(),
        [".m4a"] = new Size32.MusicIcon(),
        [".m4b"] = new Size32.MusicIcon(),
        [".m4p"] = new Size32.MusicIcon(),
        [".mmf"] = new Size32.MusicIcon(),
        [".movpkg"] = new Size32.MusicIcon(),
        [".mp1"] = new Size32.MusicIcon(),
        [".mp2"] = new Size32.MusicIcon(),
        [".mp3"] = new Size32.MusicIcon(),
        [".mpc"] = new Size32.MusicIcon(),
        [".msv"] = new Size32.MusicIcon(),
        [".nmf"] = new Size32.MusicIcon(),
        [".ogg"] = new Size32.MusicIcon(),
        [".oga"] = new Size32.MusicIcon(),
        [".mogg"] = new Size32.MusicIcon(),
        [".opus"] = new Size32.MusicIcon(),
        [".ra"] = new Size32.MusicIcon(),
        [".rm"] = new Size32.MusicIcon(),
        [".raw"] = new Size32.MusicIcon(),
        [".rf64"] = new Size32.MusicIcon(),
        [".tta"] = new Size32.MusicIcon(),
        [".voc"] = new Size32.MusicIcon(),
        [".vox"] = new Size32.MusicIcon(),
        [".wav"] = new Size32.MusicIcon(),
        [".wma"] = new Size32.MusicIcon(),
        [".wv"] = new Size32.MusicIcon(),
        [".webm"] = new Size32.MusicIcon(),
        [".8svx"] = new Size32.MusicIcon(),
        [".cda"] = new Size32.MusicIcon(),

        [".webm"] = new Size32.VideoIcon(),
        [".mkv"] = new Size32.VideoIcon(),
        [".flv"] = new Size32.VideoIcon(),
        [".vob"] = new Size32.VideoIcon(),
        [".ogv"] = new Size32.VideoIcon(),
        [".drc"] = new Size32.VideoIcon(),
        [".gifv"] = new Size32.VideoIcon(),
        [".avi"] = new Size32.VideoIcon(),
        [".mts"] = new Size32.VideoIcon(),
        [".m2ts"] = new Size32.VideoIcon(),
        [".ts"] = new Size32.VideoIcon(),
        [".mov"] = new Size32.VideoIcon(),
        [".qt"] = new Size32.VideoIcon(),
        [".wmv"] = new Size32.VideoIcon(),
        [".yuv"] = new Size32.VideoIcon(),
        [".rmvb"] = new Size32.VideoIcon(),
        [".viv"] = new Size32.VideoIcon(),
        [".asf"] = new Size32.VideoIcon(),
        [".amv"] = new Size32.VideoIcon(),
        [".mp4"] = new Size32.VideoIcon(),
        [".m4p"] = new Size32.VideoIcon(),
        [".m4v"] = new Size32.VideoIcon(),
        [".mpv"] = new Size32.VideoIcon(),
        [".svi"] = new Size32.VideoIcon(),
        [".3g2"] = new Size32.VideoIcon(),
        [".mxf"] = new Size32.VideoIcon(),
        [".roq"] = new Size32.VideoIcon(),
        [".nsv"] = new Size32.VideoIcon(),

        [".pbix"] = new Size32.PowerBiIcon(),

        [".pdf"] = new Size32.PdfIcon(),
        [".json"] = new Size32.JsonIcon(),

        ["Default"] = new Size32.DefaultFileIcon(),
    };

    internal static Icon FromExtension(string? extension)
    {
        if (string.IsNullOrEmpty(extension))
        {
            return _iconsFromExtension["Default"];
        }

        if (_iconsFromExtension.TryGetValue(extension.ToLowerInvariant(), out var value) &&
            value is not null)
        {
            return value;
        }

        return _iconsFromExtension["Default"];
    }

    internal static Icon GetIconForDetails(string? extension, bool isDirectory)
    {
        if (isDirectory)
        {
            return new Size128.FolderIcon();
        }

        if (string.IsNullOrEmpty(extension))
        {
            return new Size128.DefaultFileIcon();
        }

        if (_iconForDetailsPanel.TryGetValue(extension.ToLowerInvariant(), out var value) &&
            value is not null)
        {
            return value;
        }

        return _iconForDetailsPanel["Default"];
    }

    public static string ToImageSource(this Icon icon)
    {
        return $"data:image/svg+xml;base64,{Convert.ToBase64String(Encoding.UTF8.GetBytes(icon.Content))}";
    }
}

