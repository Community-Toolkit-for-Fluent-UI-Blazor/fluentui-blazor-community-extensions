namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the type of change detected in a DOM mutation event, such as attribute modifications, changes to child
/// nodes, or updates to textual content.
/// </summary>
/// <remarks>Use this enumeration to determine the nature of a DOM mutation when handling mutation events. Each
/// value corresponds to a distinct category of change: attribute updates, additions or removals of child nodes, or
/// modifications to character data. This can be useful for filtering or responding to specific mutation types in event
/// handlers.</remarks>
public enum MutationType
{
    /// <summary>
    /// Gets the collection of attributes associated with the current object.
    /// </summary>
    /// <remarks>Use this property to access metadata or custom information attached to the object, such as
    /// data annotations or user-defined attributes. The returned collection may be empty if no attributes are
    /// present.</remarks>
    Attributes,

    /// <summary>
    /// Represents a collection of child elements associated with a parent object.
    /// </summary>
    /// <remarks>Use this collection to access, enumerate, or manage child elements in hierarchical data
    /// structures. The order and mutability of the collection may depend on the specific implementation.</remarks>
    ChildList,

    /// <summary>
    /// Represents the textual data contained within a character node, such as text, CDATA, or comments, in a document
    /// object model (DOM).
    /// </summary>
    /// <remarks>Use this type to access or manipulate the character data of nodes that store text content. It
    /// is commonly used when working with XML or HTML documents to retrieve or modify the value of text nodes, CDATA
    /// sections, or comment nodes. This type does not represent element nodes or attributes.</remarks>
    CharacterData
}
