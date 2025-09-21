using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel for configuring the signature pen settings.
/// </summary>
/// <remarks>This component allows users to customize the appearance and behavior of the signature pen by setting
/// various options through the <see cref="Content"/> property.</remarks>
public partial class SignaturePenPanel
{
    /// <summary>
    /// Gets or sets the options for configuring the signature pen.
    /// </summary>
    [Parameter]
    public (SignatureLabels, SignaturePenOptions) Content { get; set; } = default!;

    /// <summary>
    /// Gets the format string for displaying value in the good format.
    /// </summary>
    /// <param name="format">Format of the value.</param>
    /// <param name="value">Value to format.</param>
    /// <returns>Returns the formatted value.</returns>
    private static string GetFormat(string format, double value)
    {
        return string.Format(CultureInfo.CurrentCulture, format, value);
    }
}
