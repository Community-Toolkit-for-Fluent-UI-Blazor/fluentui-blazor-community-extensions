using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a watermark that can be applied to a signature.
/// </summary>
public sealed class SignatureWatermarkSettings
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the text to be displayed as a watermark.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the image to be used as a watermark.
    /// </summary>
    [Parameter]
    public byte[]? Image { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the watermark. Default is 0.15 (15%).
    /// </summary>
    [Parameter]
    public float Opacity { get; set; } = 0.15f;

    /// <summary>
    /// Gets or sets the color of the watermark text. Default is black (#000000).
    /// </summary>
    [Parameter]
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the parent signature component.
    /// </summary>
    [CascadingParameter]
    private FluentCxSignature? Parent { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SignatureWatermarkSettings)} must be used within a {nameof(FluentCxSignature)} component.");
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.HasValueChanged(nameof(Text), Text) ||
            parameters.HasValueChanged(nameof(Image), Image) ||
            parameters.HasValueChanged(nameof(Opacity), Opacity))
        {
            Parent?.InvalidateRender();
        }

        return base.SetParametersAsync(parameters);
    }
}
