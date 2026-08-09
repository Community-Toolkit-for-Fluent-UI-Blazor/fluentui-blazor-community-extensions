namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for motion layout components that support animation or transitions within a user interface.
/// </summary>
/// <remarks>Implement this interface to provide custom motion or layout behaviors in UI components. The specific
/// methods and properties required by implementations are determined by the interface's extensions.</remarks>
public interface IMotionLayout
{
    /// <summary>
    /// Calculates and applies the layout for the specified collection of motion items.
    /// </summary>
    /// <param name="items">The list of motion items to arrange. Cannot be null.</param>
    void ComputeLayout(IReadOnlyList<MotionItem> items);

    /// <summary>
    /// Sets the width and height dimensions for the current object.
    /// </summary>
    /// <param name="width">The width value to set. Must be a non-negative number representing the object's width.</param>
    /// <param name="height">The height value to set. Must be a non-negative number representing the object's height.</param>
    void SetDimensions(double width, double height);
}
