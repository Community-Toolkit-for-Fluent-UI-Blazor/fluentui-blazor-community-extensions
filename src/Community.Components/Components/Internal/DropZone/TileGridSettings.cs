using System.Runtime.CompilerServices;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Internal;

/// <summary>
/// Represents the settings of the <see cref="FluentCxTileGrid{TItem}"/>.
/// </summary>
internal class TileGridSettings
    : ITileGridSettings
{
    /// <inheritdoc />
    public string ColumnWidth { get; set; } = "1fr";

    /// <inheritdoc />
    public string? MinimumColumnWidth { get; set; }

    /// <inheritdoc />
    public int? Columns { get; set; }

    /// <inheritdoc />
    public string RowHeight { get; set; } = "1fr";

    /// <inheritdoc />
    public string? Width { get; set; } = "100%";

    /// <inheritdoc />
    public string? Height { get; set; } = "100%";

    /// <inheritdoc />
    public string? MinimumRowHeight { get; set; }

    /// <summary>
    /// Gets the style of the rows of the <see cref="FluentCxTileGrid{TItem}"/>.
    /// </summary>
    /// <returns>Returns the style of the rows of the <see cref="FluentCxTileGrid{TItem}"/>.</returns>
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

    /// <summary>
    /// Gets the style of the columns of the <see cref="FluentCxTileGrid{TItem}"/>.
    /// </summary>
    /// <returns>Returns the style of the rows of the <see cref="FluentCxTileGrid{TItem}"/>.</returns>
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

    /// <inheritdoc />
    public override string? ToString()
    {
        return new StyleBuilder()
            .AddStyle("display", "grid")
            .AddStyle("grid-template-columns", GetColumns())
            .AddStyle("grid-auto-rows", GetRows())
            .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
            .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
            .Build();
    }
}
