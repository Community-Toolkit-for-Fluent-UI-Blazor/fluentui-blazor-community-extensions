using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a mechanism for mapping a scheduler item to its visual position and size within a container.
/// </summary>
/// <remarks>Implementations of this interface are used to calculate layout coordinates for items in scheduling or
/// calendar views. The mapping typically determines where and how large an item appears based on its assigned slots and
/// the dimensions of the container.</remarks>
/// <typeparam name="TItem">The type of the data associated with each scheduler item.</typeparam>
public interface IPositionMapper<TItem>
{
    /// <summary>
    /// Maps the specified scheduler item to one or more rectangles within the container for the given date, based on
    /// the provided scheduler slots.
    /// </summary>
    /// <remarks>The returned rectangles correspond to the visual representation of the item within the
    /// scheduler's layout for the specified date. The mapping takes into account slot availability and container
    /// dimensions. If no valid mapping is possible, the method returns <see langword="null"/>.</remarks>
    /// <param name="slots">An array of scheduler slots that define the available time segments and layout constraints for mapping the item.</param>
    /// <param name="item">The scheduler item to be mapped. Represents the entity to be positioned within the container according to the
    /// slot configuration.</param>
    /// <param name="container">The dimensions of the container element in which the rectangles will be placed.</param>
    /// <param name="date">The date for which the mapping should be performed. Determines the temporal context for slot selection and item
    /// placement.</param>
    /// <returns>An enumerable collection of <see cref="RectangleF"/> objects representing the mapped regions for the item within
    /// the container. Returns <see langword="null"/> if the item cannot be mapped for the specified date.</returns>
    IEnumerable<MappedItemRect> Map(
        SchedulerSlot[] slots,
        SchedulerItem<TItem> item,
        ElementDimensions container,
        DateTime date);
}
