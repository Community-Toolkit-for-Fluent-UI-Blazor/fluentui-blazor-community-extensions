namespace FluentUI.Blazor.Community.Components;

public interface ITileGridSettings
    : IGridSettings
{
    string ColumnWidth { get; }

    string? MinimumColumnWidth { get; }

    int? Columns { get; }

    string RowHeight { get; }
}
