using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a base class for SVG elements, providing common functionality for managing attributes and child elements.
/// </summary>
/// <remarks>This abstract class is intended to be inherited by specific SVG element types. It provides
/// collections for attributes and child elements, as well as a method for serializing the element and its children to a
/// string builder in SVG format.</remarks>
public abstract class SvgElement
{
    /// <summary>
    /// Gets the collection of additional HTML attributes applied to the component.
    /// </summary>
    /// <remarks>Use this property to add custom attributes that are not explicitly defined on the component.
    /// These attributes will be rendered on the root element of the component.</remarks>
    internal protected List<(string Key, string Value)> Attributes { get; } = [];

    /// <summary>
    /// Gets the collection of child SVG elements contained within this element.
    /// </summary>
    /// <remarks>Use this property to access or enumerate the immediate child elements of the current SVG
    /// element. The collection is read-only; elements can be added or removed through the returned list, but the
    /// property itself cannot be reassigned.</remarks>
    public List<SvgElement> Children { get; } = [];

    /// <summary>
    /// Gets the HTML tag name represented by the component.
    /// </summary>
    public abstract string TagName { get; }

    /// <summary>
    /// Appends the HTML representation of this element and its children to the specified StringBuilder.
    /// </summary>
    /// <remarks>This method generates a self-closing tag if the element has no children. Otherwise, it writes
    /// the start tag, recursively writes all child elements, and then writes the end tag. The method does not clear or
    /// reset the StringBuilder; it appends to its existing content.</remarks>
    /// <param name="sb">The StringBuilder to which the HTML output is appended. Cannot be null.</param>
    public virtual void WriteTo(StringBuilder sb)
    {
        sb.Append('<');
        sb.Append(TagName);

        foreach (var (Key, Value) in Attributes)
        {
            sb.Append(' ');
            sb.Append(Key);
            sb.Append("=\"");
            sb.Append(Value);
            sb.Append('"');
        }

        if (Children.Count == 0)
        {
            sb.Append(" />");
            return;
        }

        sb.Append('>');

        foreach (var child in Children)
        {
            child.WriteTo(sb);
        }

        sb.Append("</");
        sb.Append(TagName);
        sb.Append('>');
    }

    /// <summary>
    /// Sets an attribute with the specified key and value, updating an existing attribute or adding a new one if not
    /// found.
    /// </summary>
    /// <param name="key">The key of the attribute to set.</param>
    /// <param name="value">The value to assign to the attribute.</param>
    public void SetAttribute(string key, string value)
    {
        for (var i = 0; i < Attributes.Count; i++)
        {
            if (string.Equals(Attributes[i].Key, key))
            {
                Attributes[i] = (key, value);
                return;
            }
        }

        Attributes.Add((key, value));
    }
}
