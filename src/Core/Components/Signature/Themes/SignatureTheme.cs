namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available visual themes for signature components.
/// </summary>
/// <remarks>Use this enumeration to select a predefined style for rendering signature elements. Each theme
/// provides a distinct appearance suitable for different contexts, such as engineering diagrams or notebook
/// backgrounds.</remarks>
public enum SignatureTheme
{
    /// <summary>
    /// Represents the default value or option for the associated enumeration or property.
    /// </summary>
    Default,

    /// <summary>
    /// Represents a blueprint-style theme.
    /// </summary>
    Blueprint,

    /// <summary>
    /// Represents an engineering theme.
    /// </summary>
    Engineering,

    /// <summary>
    /// Represents a notebook theme.
    /// </summary>
    Notebook,

    /// <summary>
    /// Represents a notebook with dotted pages theme.
    /// </summary>
    DottedNotebook,

    /// <summary>
    /// Represents a dark grid theme.
    /// </summary>
    DarkGrid,

    /// <summary>
    /// Represents a minimalist style or theme.
    /// </summary>
    Minimalist,

    /// <summary>
    /// Represents a dark-themed blueprint style with neon accents.
    /// </summary>
    DarkBlueprintNeon,

    /// <summary>
    /// Represents a component that displays a grid resembling graph paper, typically used for drawing, plotting, or
    /// layout purposes.
    /// </summary>
    GraphPaper,

    /// <summary>
    /// Represents a component that displays a grid resembling French ruled paper, which is commonly used for handwriting.
    /// </summary>
    FrenchNotebook,

    /// <summary>
    /// Represents a type of paper commonly used for engineering drawings, diagrams, or technical documentation.
    /// </summary>
    EngineeringPaper,

    /// <summary>
    /// Represents the Blueprint Dark theme variant.
    /// </summary>
    BlueprintDark
}
