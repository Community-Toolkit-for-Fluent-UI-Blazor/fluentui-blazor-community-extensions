using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Gets or sets the options used to configure file search behavior.
/// </summary>
/// <remarks>Use this property to specify search parameters such as filters, search scope, or other criteria that
/// affect how files are located. Changing these options will alter the results returned by the file search
/// functionality.</remarks>
public partial class SearchOptionsDialog
    : FluentDialogInstance
{
    /// <summary>
    /// Represents a comma-separated list of file extensions to include in the search filter.
    /// </summary>
    private string? _extensions;

    /// <summary>
    /// Represents the minimum file size to include in the search filter, specified as a string (e.g., "1MB", "500KB").
    /// </summary>
    private string? _minSize;

    /// <summary>
    /// Represents the maximum file size to include in the search filter, specified as a string (e.g., "10MB", "500KB").
    /// </summary>
    private string? _maxSize;

    /// <summary>
    /// Gets or sets the options used to configure file search behavior.
    /// </summary>
    /// <remarks>Use this property to specify search parameters such as filters, search scope, or other
    /// criteria that affect how files are located. Changing these options will alter the results returned by the file
    /// search functionality.</remarks>
    [Parameter]
    public FileSearchOptions Options { get; set; } = new();

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _extensions = Options.Extensions is null ? null : string.Join(',', Options.Extensions);
        _maxSize = Options.MaxSize.HasValue ? ByteSize.FromBits(Options.MaxSize.Value).ToString() : null;
        _minSize = Options.MinSize.HasValue ? ByteSize.FromBits(Options.MinSize.Value).ToString() : null;
    }

    /// <inheritdoc/>
    protected override Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            Options.Extensions = string.IsNullOrWhiteSpace(_extensions)
                ? null
                : [.. _extensions.Split(',').Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e))];

            if (!string.IsNullOrEmpty(_minSize))
            {
                var minSizeParsed = ByteSize.Parse(_minSize, CultureInfo.CurrentCulture);
                Options.MinSize = minSizeParsed.Bits;
            }

            if (!string.IsNullOrEmpty(_maxSize))
            {
                var maxSizeParsed = ByteSize.Parse(_maxSize, CultureInfo.CurrentCulture);
                Options.MaxSize = maxSizeParsed.Bits;
            }

            return DialogInstance.CloseAsync(Options);
        }
        else
        {
            return DialogInstance.CancelAsync();
        }
    }
}
