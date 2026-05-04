using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a 'tspan' element in an SVG document, used to define a sub-string of text within an SVG 'text' element.
/// </summary>
public sealed class SvgTSpan : SvgElement
{
    /// <inheritdoc />
    public override string TagName => "tspan";

    /// <summary>
    /// Gets or sets the text content associated with this instance.
    /// </summary>
    public string? Text { get; set; }

    /// <inheritdoc />
    public override void WriteTo(StringBuilder sb)
    {
        sb.Append('<').Append(TagName);

        foreach (var (Key, Value) in Attributes)
        {
            sb.Append(' ');
            sb.Append(Key);
            sb.Append("=\"");
            sb.Append(Value);
            sb.Append('"');
        }

        if (Children.Count == 0 && string.IsNullOrEmpty(Text))
        {
            sb.Append(" />");
            return;
        }

        sb.Append('>');

        if (!string.IsNullOrEmpty(Text))
        {
            sb.Append(Text);
        }

        foreach (var child in Children)
        {
            child.WriteTo(sb);
        }

        sb.Append("</");
        sb.Append(TagName);
        sb.Append('>');
    }
}
