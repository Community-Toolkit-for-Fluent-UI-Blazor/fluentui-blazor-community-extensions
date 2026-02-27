namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for an eraser component that provides erasing functionality.
/// </summary>
public interface IEraser
{
    /// <summary>
    /// Begins processing the specified pointer sample.
    /// </summary>
    /// <param name="sample">The pointer sample to process. Cannot be null.</param>
    void Begin(PointerSample sample);

    /// <summary>
    /// Updates the eraser state based on the specified pointer sample.
    /// </summary>
    /// <param name="sample">The pointer sample to process. Cannot be null.</param>
    void Update(PointerSample sample);

    /// <summary>
    /// Marks the end of a pointer interaction using the specified sample.
    /// </summary>
    /// <param name="sample">The pointer sample that represents the final state of the interaction. Cannot be null.</param>
    void End(PointerSample sample);
}
