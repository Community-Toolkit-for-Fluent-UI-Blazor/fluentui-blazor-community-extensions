using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the image inside a <see cref="FluentCxSlideshow{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class SlideshowImage<TItem>
    : FluentComponentBase, IDisposable
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SlideshowImage{TItem}"/> class.
    /// </summary>
    public SlideshowImage()
    {
        Id = Identifier.NewId();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the parent of the component.
    /// </summary>
    [CascadingParameter]
    private FluentCxSlideshow<TItem>? Parent { get; set; }

    /// <summary>
    /// Gets or sets the source of the image.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the alternate text of the image.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; }

    /// <summary>
    /// Gets or sets the title of the image.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    #endregion Properties

    #region Methods

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("The SlideshowImage can only be used only inside a FluentCxSlideshow component");
        }

        Parent.Add(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.Remove(this);

        GC.SuppressFinalize(this);
    }

    #endregion Methods
}
