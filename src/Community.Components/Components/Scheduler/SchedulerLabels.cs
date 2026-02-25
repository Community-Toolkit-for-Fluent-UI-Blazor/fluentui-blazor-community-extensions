namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of localized labels for scheduler user interface elements, such as navigation buttons and view
/// names.
/// </summary>
/// <remarks>Use this record to customize or localize the text displayed in scheduler controls. Predefined
/// instances, such as <see cref="French"/>, provide common localizations, or you can create a custom instance by
/// setting the desired properties. All properties are immutable and can be initialized using object initializer
/// syntax.</remarks>
public record SchedulerLabels
{
    /// <summary>
    /// Gets the default set of scheduler labels.
    /// </summary>
    public static SchedulerLabels Default => new();

    /// <summary>
    /// Gets a set of scheduler labels localized in French for use in calendar and scheduling interfaces.
    /// </summary>
    /// <remarks>Use this property to display French language labels for common scheduler views and navigation
    /// actions, such as 'Next', 'Previous', 'Today', and various calendar views. This is useful for applications that
    /// require French localization in their scheduling components.</remarks>
    public static SchedulerLabels French => new()
    {
        Next = "Suivant",
        Previous = "Précédent",
        Today = "Aujourd'hui",
        AgendaView = "Agenda",
        DayView = "Jour",
        MonthView = "Mois",
        WeekView = "Hebdomadaire",
        YearView = "Année",
        TimelineView = "Chronologie",
        Date = "Date",
        Time = "Heure",
        Events = "Événements",
        ShowAllDay = "Afficher toute la journée",
        ShowBusinessHours = "Afficher les heures de bureau",
        HideEmptyDays = "Masquer les jours vides",
        NumberOfDays = "Nombre de jours",
        AgendaSettings = "Paramètres de l'agenda",
        EditEventTitle = "Modifier l'événement",
        CloseButtonTitle = "Fermer",
        Cancel = "Annuler",
        OK = "Valider",
        CreateEventTitle = "Créer un événement",
        EventTitle = "Événement",
        Daily = "Quotidien",
        End = "Fin",
        Monthly = "Mensuel",
        NoRecurrence = "Aucune",
        Recurrence = "Répétition",
        Start = "Début",
        Weekly = "Hebdomadaire",
        Yearly = "Annuel",
        RepeatEveryXDays = "Répéter tous les {0} jours",
        EndOfRecurrence = "Fin de la récurrence",
        Never = "Jamais",
        Until = "Jusqu'à",
        AfterXOccurrences = "Après {0} occurrences",
        DateExceptions = "Dates exclues :",
        Add = "Ajouter",
        RemoveDateExceptions = "Supprimer",
        RepeatEveryXWeeks = "Répéter toutes les {0} semaines",
        SelectDaysOfWeek = "Sélectionner les jours de la semaine",
        RepeatEveryXMonths = "Répéter tous les {0} mois",
        DayOfMonth = "Jour du mois",
        RepeatEveryXYears = "Répéter tous les {0} ans",
        SelectMonths = "Sélectionner les mois",
        DaySettings = "Paramètres du jour",
        DaySlotHeight = "Hauteur de la plage horaire",
        NumberOfSubdivisions = "Nombre de subdivisions",
        DeleteCurrentOccurrence = "Supprimer l'occurrence actuelle",
        DeleteEventTitle = "Supprimer l'événement",
        DeleteEventMessage = "Êtes-vous sûr de vouloir supprimer cet événement ?",
        Yes = "Oui",
        No = "Non",
        DeleteRecurrenceEvent = "Supprimer l'événement récurrent",
        DeleteWholeSeries = "Supprimer la série entière",
        DeleteRecurrenceMessage = "Voulez-vous supprimer la série entière ou seulement cette occurrence ?",
        WeekSlotHeight = "Hauteur de la plage hebdomadaire",
        WeekSettings = "Paramètres de la semaine",
        Loading = "Chargement ..."
    };

    /// <summary>
    /// Gets the string value representing the next item or action in a sequence.
    /// </summary>
    public string Next { get; init; } = "Next";

    /// <summary>
    /// Gets the value representing the previous item or state.
    /// </summary>
    public string Previous { get; init; } = "Previous";

    /// <summary>
    /// Gets the string value representing today's label or identifier.
    /// </summary>
    public string Today { get; init; } = "Today";

    /// <summary>
    /// Gets the display label used for the day view in the calendar interface.
    /// </summary>
    public string DayView { get; init; } = "Day";

    /// <summary>
    /// Gets the display label used for the week view in the calendar interface.
    /// </summary>
    public string WeekView { get; init; } = "Week";

    /// <summary>
    /// Gets the display label used for the month view in the calendar interface.
    /// </summary>
    public string MonthView { get; init; } = "Month";

    /// <summary>
    /// Gets the display label used for the year view in the calendar interface.
    /// </summary>
    public string YearView { get; init; } = "Year";

    /// <summary>
    /// Gets the name of the agenda view to be displayed.
    /// </summary>
    public string AgendaView { get; init; } = "Agenda";

    /// <summary>
    /// Gets the name of the timeline view to be displayed.
    /// </summary>
    public string TimelineView { get; init; } = "Timeline";

    /// <summary>
    /// Gets the label for the option to show business hours in the scheduler.
    /// </summary>
    public string ShowBusinessHours { get; init; } = "Show Business Hours";

    /// <summary>
    /// Gets the label for the option to show all day events in the scheduler.
    /// </summary>
    public string ShowAllDay { get; init; } = "Show All Day";

    /// <summary>
    /// Gets the label for the date column in the agenda view.
    /// </summary>
    public string Date { get; init; } = "Date";

    /// <summary>
    /// Gets the label for the time column in the agenda view.
    /// </summary>
    public string Time { get; init; } = "Time";

    /// <summary>
    /// Gets the label for the event column in the agenda view.
    /// </summary>
    public string Events { get; init; } = "Events";

    /// <summary>
    /// Gets the display label representing the number of days.
    /// </summary>
    public string NumberOfDays { get; init; } = "Number of Days";

    /// <summary>
    /// Gets the display text used to indicate that empty days should be hidden in the calendar view.
    /// </summary>
    public string HideEmptyDays { get; init; } = "Hide Empty Days";

    /// <summary>
    /// Gets the agenda settings configuration value.
    /// </summary>
    public string AgendaSettings { get; init; } = "Agenda Settings";

    /// <summary>
    /// Gets the default title text used for editing an event.
    /// </summary>
    public string EditEventTitle { get; init; } = "Edit Event";

    /// <summary>
    /// Gets the default title text used for creating a new event.
    /// </summary>
    public string CreateEventTitle { get; init; } = "Create Event";

    /// <summary>
    /// Gets the text displayed on the close button.
    /// </summary>
    public string CloseButtonTitle { get; init; } = "Close";

    /// <summary>
    /// Gets the display text for the cancel action.
    /// </summary>
    public string Cancel { get; init; } = "Cancel";

    /// <summary>
    /// Gets the status message indicating a successful operation.
    /// </summary>
    public string OK { get; init; } = "OK";

    /// <summary>
    /// Gets the title of the event.
    /// </summary>
    public string EventTitle { get; init; } = "Event";

    /// <summary>
    /// Gets the start label for the scheduler item.
    /// </summary>
    public string Start { get; init; } = "Start";

    /// <summary>
    /// Gets the end label for the scheduler item.
    /// </summary>
    public string End { get; init; } = "End";

    /// <summary>
    /// Gets the recurrence label for the scheduler item.
    /// </summary>
    public string Recurrence { get; init; } = "Recurrence";

    /// <summary>
    /// Gets the string value that represents the absence of recurrence in a schedule.
    /// </summary>
    public string NoRecurrence { get; init; } = "No Recurrence";

    /// <summary>
    /// Gets the string value representing the daily frequency option.
    /// </summary>
    public string Daily { get; init; } = "Daily";

    /// <summary>
    /// Gets the string value representing the weekly frequency option.
    /// </summary>
    public string Weekly { get; init; } = "Weekly";

    /// <summary>
    /// Gets the string value representing the monthly frequency option.
    /// </summary>
    public string Monthly { get; init; } = "Monthly";

    /// <summary>
    /// Gets the string value representing the yearly frequency option.
    /// </summary>
    public string Yearly { get; init; } = "Yearly";

    /// <summary>
    /// Gets the format string used to represent a recurring event that repeats every specified number of days.
    /// </summary>
    public string RepeatEveryXDays { get; init; } = "Repeat every {0} days";

    /// <summary>
    /// Gets the end date or condition for the recurrence pattern.
    /// </summary>
    public string EndOfRecurrence { get; init; } = "End of Recurrence";

    /// <summary>
    /// Gets the string value "Never", indicating a state or option that is never applicable.
    /// </summary>
    public string Never { get; init; } = "Never";

    /// <summary>
    /// Gets the value that specifies the end condition or limit for an operation.
    /// </summary>
    public string Until { get; init; } = "Until";

    /// <summary>
    /// Gets the format string used to display a message after a specified number of occurrences.
    /// </summary>
    /// <remarks>The format string should include a placeholder, such as "{0}", which will be replaced with
    /// the actual occurrence count when generating the message.</remarks>
    public string AfterXOccurrences { get; init; } = "After {0} occurrences";

    /// <summary>
    /// Gets the string that represents date exceptions for the current instance.
    /// </summary>
    public string DateExceptions { get; init; } = "Date exceptions :";

    /// <summary>
    /// Gets the string value representing the 'Add' operation or label.
    /// </summary>
    public string Add { get; init; } = "Add";

    /// <summary>
    /// Gets the action keyword used to indicate that date exceptions should be removed.
    /// </summary>
    public string RemoveDateExceptions { get; init; } = "Remove";

    /// <summary>
    /// Gets the format string used to display a recurring event that repeats every specified number of weeks.
    /// </summary>
    public string RepeatEveryXWeeks { get; init; } = "Repeat every {0} weeks";

    /// <summary>
    /// Gets the display text prompting users to select days of the week.
    /// </summary>
    public string SelectDaysOfWeek { get; init; } = "Select days of the week";

    /// <summary>
    /// Gets the format string used to represent a recurring event that repeats every specified number of months.
    /// </summary>
    public string RepeatEveryXMonths { get; init; } = "Repeat every {0} months";

    /// <summary>
    /// Gets the display name representing the day of the month.
    /// </summary>
    public string DayOfMonth { get; init; } = "Day of Month";

    /// <summary>
    /// Gets the format string used to specify a recurring event every set number of years.
    /// </summary>
    public string RepeatEveryXYears { get; init; } = "Repeat every {0} years";

    /// <summary>
    /// Gets the display text used for the month selection prompt.
    /// </summary>
    public string SelectMonths { get; init; } = "Select Months";

    /// <summary>
    /// Gets the day settings configuration as a string.
    /// </summary>
    public string DaySettings { get; init; } = "Day Settings";

    /// <summary>
    /// Gets the label for the number of subdivisions.
    /// </summary>
    public string NumberOfSubdivisions { get; init; } = "Number of Subdivisions";

    /// <summary>
    /// Gets the height of a single day slot, typically used for layout or rendering purposes.
    /// </summary>
    public string DaySlotHeight { get; init; } = "Day Slot Height";

    /// <summary>
    /// Gets the localized title text used for the delete event action.
    /// </summary>
    public string DeleteEventTitle { get; init; } = "Delete event";

    /// <summary>
    /// Gets the default confirmation message displayed when prompting the user to delete an event.
    /// </summary>
    public string DeleteEventMessage { get; init; } = "Are you sure you want to delete this event ?";

    /// <summary>
    /// Gets the string value representing an affirmative response.
    /// </summary>
    public string Yes { get; init; } = "Yes";

    /// <summary>
    /// Gets the string value representing "No".
    /// </summary>
    public string No { get; init; } = "No";

    /// <summary>
    /// Gets the display text used for the action to delete a recurring event.
    /// </summary>
    public string DeleteRecurrenceEvent { get; init; } = "Delete Recurring Event";

    /// <summary>
    /// Gets the display text used to indicate the option to delete an entire series of items.
    /// </summary>
    public string DeleteWholeSeries { get; init; } = "Delete Whole Series";

    /// <summary>
    /// Gets the display text for the action to delete the current occurrence in a recurring series.
    /// </summary>
    public string DeleteCurrentOccurrence { get; init; } = "Delete Current Occurrence";

    /// <summary>
    /// Gets the message displayed when prompting the user to delete a recurring event.
    /// </summary>
    public string DeleteRecurrenceMessage { get; init; } = "Do you want to delete the whole series or just this occurrence ?";

    /// <summary>
    /// Gets the height of a week slot as a string value.
    /// </summary>
    public string WeekSlotHeight { get; init; } = "Week Slot Height";

    /// <summary>
    /// Gets the configuration string that defines the settings for the week.
    /// </summary>
    public string WeekSettings { get; init; } = "Week Settings";

    /// <summary>
    /// Gets the loading message displayed while an operation is in progress.
    /// </summary>
    public string Loading { get; init; } = "Loading";
}
