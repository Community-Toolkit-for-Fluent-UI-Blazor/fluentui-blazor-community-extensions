namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the direction in which a staggered animation sequence progresses.
/// </summary>
/// <remarks>Use this enumeration to control whether staggered animations proceed in a forward or backward order.
/// This can be useful when animating collections or sequences where the order of appearance or disappearance
/// matters.</remarks>
public enum StaggerDirection
{
    /// <summary>
    /// Specifies the backward direction, typically used to indicate reverse movement or navigation.
    /// </summary>
    Backward = -1,

    /// <summary>
    /// Specifies that the direction is forward.
    /// </summary>
    Forward = 1
}
