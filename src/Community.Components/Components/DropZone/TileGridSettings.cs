using System.Runtime.CompilerServices;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

internal class TileGridSettings
    : ITileGridSettings
{
    public string ColumnWidth { get; set; } = "1fr";

    public string? MinimumColumnWidth { get; set; }

    public int? Columns { get; set; }

    public string RowHeight { get; set; } = "1fr";

    public DropZoneDisplay Display { get; } = DropZoneDisplay.Grid;

    public string? Width { get; set; } = "100%";

    public string? Height { get; set; } = "100%";

    public string? MinimumRowHeight { get; set; }

    private string GetRows()
    {
        DefaultInterpolatedStringHandler handler = new();

        // Rows
        handler.AppendLiteral("minmax(");

        if (string.IsNullOrEmpty(MinimumRowHeight))
        {
            handler.AppendLiteral("0px, ");
        }
        else
        {
            handler.AppendFormatted(MinimumRowHeight);
            handler.AppendLiteral(", ");
        }

        handler.AppendFormatted(RowHeight);
        handler.AppendLiteral(");");

        return handler.ToString();
    }

    private string GetColumns()
    {
        DefaultInterpolatedStringHandler handler = new();

        // Columns
        handler.AppendLiteral("repeat(");

        if (Columns.HasValue)
        {
            handler.AppendFormatted(Columns.GetValueOrDefault());
        }
        else
        {
            handler.AppendLiteral("auto-fit");
        }

        handler.AppendLiteral(", minmax(");

        if (string.IsNullOrEmpty(MinimumColumnWidth))
        {
            handler.AppendLiteral("0px, ");
        }
        else
        {
            handler.AppendFormatted(MinimumColumnWidth);
            handler.AppendLiteral(", ");
        }

        handler.AppendFormatted(ColumnWidth);
        handler.AppendLiteral("));");

        return handler.ToString();
    }

    public override string? ToString()
    {
        return new StyleBuilder()
            .AddStyle("display", Display.ToAttributeValue())
            .AddStyle("grid-template-columns", GetColumns())
            .AddStyle("grid-auto-rows", GetRows())
            .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
            .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
            .Build();
    }
}
