using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dialog component for displaying and editing scheduler events for items of the specified type.
/// </summary>
/// <remarks>Use this component to present event details and enable user interaction for scheduling scenarios. The
/// generic parameter allows integration with custom event data models.</remarks>
/// <typeparam name="TItem">The type of the item associated with the scheduler event.</typeparam>
public partial class SchedulerEventDialog<TItem> : FluentDialogInstance
{
    /// <summary>
    /// Represents the scheduler item associated with this dialog instance.
    /// </summary>
    private EditSchedulerItem<TItem>? _item;

    /// <summary>
    /// Gets or sets the template used to render the content of a scheduler event for the specified item type.
    /// </summary>
    /// <remarks>Set this property to customize how event details are displayed within the scheduler. The
    /// template receives the event data of type <typeparamref name="TItem"/> and allows for flexible presentation of
    /// event information.</remarks>
    [Parameter]
    public SchedulerEventContent<TItem> Content { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _item = new EditSchedulerItem<TItem>()
        {
            Data = Content.Item.Data,
            Start = Content.Item.Start,
            End = Content.Item.End,
            StartTime = Content.Item.Start,
            EndTime = Content.Item.End,
            Recurrence = Content.Item.Recurrence,
            Description = Content.Item.Description,
            Exceptions = [.. Content.Item.Exceptions],
            Id = Content.Item.Id,
            Title = Content.Item.Title,
        };

        ValidatePrimaryAction();
    }

    /// <summary>
    /// Validates whether the primary action in the dialog footer should be enabled based on the current item state.
    /// </summary>
    /// <remarks>The primary action is disabled if the item's title is null or empty, or if the item's start
    /// value is not set. This method should be called whenever the relevant item properties change to ensure the
    /// dialog's action state remains consistent.</remarks>
    private void ValidatePrimaryAction()
    {
        DialogInstance.Options.Footer.PrimaryAction.Disabled = string.IsNullOrEmpty(_item?.Title) ||
           (!_item?.Start.HasValue ?? false);
    }

    /// <summary>
    /// Determines the visual appearance based on the specified recurrence frequency and the item's recurrence settings.
    /// </summary>
    /// <param name="recurrenceFrequency">The recurrence frequency to compare with the item's recurrence. If null, the method returns an accent
    /// appearance.</param>
    /// <returns>An Accent appearance if the recurrence frequency matches the item's recurrence or is null; otherwise, a Neutral
    /// appearance.</returns>
    private ButtonAppearance GetAppearanceFromRecurrence(RecurrenceFrequency? recurrenceFrequency = null)
    {
        if (_item?.Recurrence is null &&
            recurrenceFrequency is null)
        {
            return ButtonAppearance.Primary;
        }

        if (_item?.Recurrence is not null &&
            recurrenceFrequency == _item.Recurrence.Frequency)
        {
            return ButtonAppearance.Primary;
        }

        return ButtonAppearance.Default;
    }

    /// <summary>
    /// Sets the recurrence rule for the current item based on the specified frequency.
    /// </summary>
    /// <remarks>This method updates the item's recurrence rule to use the provided frequency with a default
    /// interval of 1. If no frequency is specified, the recurrence rule is cleared.</remarks>
    /// <param name="recurrenceFrequency">The recurrence frequency to apply to the item. If null, any existing recurrence rule is removed.</param>
    private void OnSetRecurrence(RecurrenceFrequency? recurrenceFrequency = null)
    {
        if (_item is null)
        {
            return;
        }

        if (recurrenceFrequency is null)
        {
            _item!.Recurrence = null;
        }
        else
        {
            _item.Recurrence = new RecurrenceRule()
            {
                Frequency = recurrenceFrequency.Value,
                Interval = 1,
            };
        }
    }

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary && _item is not null)
        {
            var defaultStart = _item.Start.GetValueOrDefault();
            var defaultTimeStart = _item.StartTime.GetValueOrDefault();
            var defaultEnd = _item.End.GetValueOrDefault();
            var defaultTimeEnd = _item.EndTime.GetValueOrDefault();

            await DialogInstance.CloseAsync(new SchedulerItem<TItem>()
            {
                Data = _item!.Data,
                Start = new DateTime(defaultStart.Year, defaultStart.Month, defaultStart.Day, defaultTimeStart.Hour, defaultTimeStart.Minute, defaultTimeStart.Second),
                End = new DateTime(defaultEnd.Year, defaultEnd.Month, defaultEnd.Day, defaultTimeEnd.Hour, defaultTimeEnd.Minute, defaultTimeEnd.Second),
                Recurrence = _item.Recurrence,
                Description = _item.Description,
                Exceptions = [.. _item.Exceptions],
                Id = _item.Id,
                Title = _item.Title,
            });
        }
        else
        {
            await DialogInstance.CancelAsync();
        }
    }
}
