namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides options for exporting data from the console application to various file formats.
/// </summary>
/// <remarks>This class enables users to specify which file formats to export data in, including JSON, CSV, TXT,
/// XML, and Markdown. All export options are enabled by default, allowing for simultaneous export to multiple formats.
/// Use the individual properties to control which formats are included in the export operation.</remarks>
public sealed class ConsoleExportFileOptions
{
    private sealed record ExportOption(string Name, Func<bool> Getter);

    private IEnumerable<ExportOption> AllExportOptions => [
        new("json", () => ExportJson),
        new("csv", () => ExportCsv),
        new("txt", () => ExportTxt),
        new("xml", () => ExportXml),
        new("markdown", () => ExportMarkdown)
    ];

    /// <summary>
    /// Gets or sets a value indicating whether data should be exported in JSON format.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the export operation outputs data as JSON. This property
    /// is useful for configuring the format of exported files.</remarks>
    public bool ExportJson { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether data should be exported in CSV format.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the export operation will generate a CSV file. This
    /// property is useful for producing reports or data exports in a widely supported format.</remarks>
    public bool ExportCsv { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether data should be exported in plain text format.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the export operation produces a text file containing the
    /// data. If <see langword="false"/>, the export will not include a plain text file.</remarks>
    public bool ExportTxt { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether data should be exported in xml format.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the export operation will generate a XML file. This
    /// property is useful for producing reports or data exports in a widely supported format.</remarks>
    public bool ExportXml { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether data should be exported in markdown format.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the export operation will generate a Markdown file. This
    /// property is useful for producing reports or data exports in a widely supported format.</remarks>
    public bool ExportMarkdown { get; set; } = true;

    /// <summary>
    /// Gets the collection of export format names that are currently selected for export.
    /// </summary>
    /// <remarks>The returned collection reflects the export formats that meet the selection criteria at the
    /// time of access. The set of selected formats may change if the underlying export options are modified.</remarks>
    public IReadOnlyList<string> SelectedFormats => [.. AllExportOptions.Where(o => o.Getter()).Select(o => o.Name)];

    /// <summary>
    /// Gets a value indicating whether multiple export formats are currently selected.
    /// </summary>
    /// <remarks>Use this property to determine if a multi-format export operation can be performed. This is
    /// useful when enabling or disabling UI elements or logic that depend on the selection of more than one export
    /// format.</remarks>
    public bool IsMultiExportSelected => SelectedFormats.Skip(1).Any();

    /// <summary>
    /// Gets the selected format if exactly one format is selected; otherwise, returns null.
    /// </summary>
    /// <remarks>This property evaluates the collection of selected formats and returns the format only when a
    /// single format is selected. If no formats or multiple formats are selected, the property returns null to indicate
    /// that no single format is currently selected.</remarks>
    public string? SelectedFormat
    {
        get
        {
            using var e = SelectedFormats.GetEnumerator();

            if (!e.MoveNext())
            {
                return null;
            }

            var first = e.Current;

            if (e.MoveNext())
            {
                return null;
            }

            return first;
        }
    }
}
