namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Repreesnts the content for the <see cref="FileUploaderDialog"/>.
/// </summary>
/// <param name="DragDropFileLabel">Label for the dragdrop zone.</param>
/// <param name="Completed">Represents the label when the operation is completed.</param>
/// <param name="Progression">Represents the label for the progressive operation.</param>
public record FileUploaderContent(
    string DragDropFileLabel,
    string Completed,
    string Progression)
{
}
