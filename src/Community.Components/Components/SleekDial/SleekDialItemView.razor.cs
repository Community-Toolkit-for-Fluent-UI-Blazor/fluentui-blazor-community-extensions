using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class SleekDialItemView
    : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Represents the render fragment to render the text.
    /// </summary>
    private readonly RenderFragment _renderText;

    /// <summary>
    /// Gets or sets the index of the item.
    /// </summary>
    internal int Index { get; set; }

    /// <summary>
    /// Gets or sets the parent of the item.
    /// </summary>
    [CascadingParameter]
    private FluentCxSleekDial? Parent { get; set; }

    /// <summary>
    /// Gets or sets the item to render.
    /// </summary>
    [Parameter]
    public SleekDialItem? Item { get; set; }

    /// <summary>
    /// Gets the css of the item.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("sleekdialitem-radial", Parent?.Mode == SleekDialMode.Radial)
        .AddClass("sleekdialitem-radial-center", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter) ?? false))
        .AddClass("sleekdialitem-radial-middle", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight) ?? false))
        .AddClass("sleekdialitem-radial-top", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight) ?? false))
        .AddClass("sleekdialitem-radial-right", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight) ?? false))
        .AddClass("sleekdialitem-radial-bottom", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight) ?? false))
        .Build();

    /// <summary>
    /// Gets the style of the item.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--sleekdial-radial-angle", GetAngle(), Parent?.Mode == SleekDialMode.Radial && Item is not null)
        .AddStyle("display", "inline-flex")
        .AddStyle("z-index", "100001")
        .Build();

    /// <summary>
    /// Gets the style of the button.
    /// </summary>
    private string? InternalButtonStyle => new StyleBuilder(Style)
        .AddStyle("border-radius", "9999px")
        .AddStyle("width", "40px")
        .AddStyle("height", "40px")
        .AddStyle("position", "relative", Parent?.Mode == SleekDialMode.Linear)
        .AddStyle("box-shadow", "0 8px 16px 0 rgba(0, 0, 0, 0.14), 0 0 2px 0 rgba(0, 0, 0, 0.12)")
        .Build();

    /// <summary>
    /// Update the additional attributes.
    /// </summary>
    private void UpdateAdditionalAttributes()
    {
        var additionalAttributes = new Dictionary<string, object>();

        if (AdditionalAttributes != null)
        {
            foreach (var kvp in AdditionalAttributes)
            {
                additionalAttributes.TryAdd(kvp.Key, kvp.Value);
            }
        }

        additionalAttributes.TryAdd("tabindex", Index + 1);

        AdditionalAttributes = additionalAttributes;
    }

    /// <summary>
    /// Occurs when the item is clicked.
    /// </summary>
    /// <returns>Returns a task which invoke the <see cref="SleekDialItem.OnClick"/> callback when completed.</returns>
    private async Task OnClickAsync()
    {
        if (Parent is not null &&
            Item is not null)
        {
            Parent.FocusedIndex = Index;
            await Item.OnClickAsync();
        }
    }

    /// <summary>
    /// Gets the angle of the item.
    /// </summary>
    /// <returns>Returns the angle of the item in degrees.</returns>
    private string GetAngle()
    {
        if (Parent is null ||
            Parent.CorrectRadialSettings is null ||
            Item is null)
        {
            return string.Empty;
        }

        var radialSettings = Parent.CorrectRadialSettings;
        var count = Parent.InternalItems.Count;
        var delta = Math.Abs(radialSettings.EndAngle - radialSettings.StartAngle);
        var itemCount = delta == 360 || delta == 0 || count == 1 ? count : count - 1;
        var stepAngle = delta / itemCount;
        var itemAngle = stepAngle * Item.Index;
        var angle = radialSettings.Direction == SleekDialRadialDirection.Clockwise ? radialSettings.StartAngle + itemAngle : radialSettings.StartAngle - itemAngle;
        var finalAngle = angle % 360;

        return $"{finalAngle}deg";
    }

    /// <summary>
    /// Occurs when a settings changed.
    /// </summary>
    /// <param name="sender">Object which invokes the method.</param>
    /// <param name="e">Event assoc</param>
    private void OnRadialSettingsChanged(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Item is null)
        {
            throw new InvalidOperationException("The Item parameter must be set.");
        }

        if (Parent is null)
        {
            throw new InvalidOperationException("A parent of type FluentCxSleekDial must be set.");
        }

        Parent.RadialSettingsChanged += OnRadialSettingsChanged;

        UpdateAdditionalAttributes();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (Parent is not null)
        {
            Parent.RadialSettingsChanged -= OnRadialSettingsChanged;
        }

        GC.SuppressFinalize(this);
    }
}
