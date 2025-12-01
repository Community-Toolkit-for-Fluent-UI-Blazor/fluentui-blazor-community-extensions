namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for generating date occurrences based on recurrence rules.
/// </summary>
/// <remarks>The RecurrenceEngine class is a utility for calculating recurring dates according to specified rules,
/// such as daily, weekly, monthly, or yearly patterns. All members are static and thread-safe. This class is typically
/// used in scheduling applications to determine event dates that repeat over time.</remarks>
public static class RecurrenceEngine
{
    /// <summary>
    /// Generates a sequence of dates representing the occurrences of a recurrence pattern within a specified date
    /// range.
    /// </summary>
    /// <remarks>Occurrences are generated according to the specified frequency and interval in <paramref
    /// name="rule"/>. The sequence respects any count or end date limits defined in the rule. The method does not
    /// guarantee that the returned dates are sorted if the recurrence rule produces non-monotonic sequences.</remarks>
    /// <param name="rule">The recurrence rule that defines the frequency, interval, and limits for generating occurrences.</param>
    /// <param name="start">The initial date of the recurrence pattern. The first occurrence will be on or after this date.</param>
    /// <param name="from">The start of the date range to include occurrences. Only occurrences on or after this date are returned.</param>
    /// <param name="to">The end of the date range to include occurrences. Only occurrences on or before this date are returned.</param>
    /// <param name="exceptions">An optional collection of dates to exclude from the generated occurrences.</param>
    /// <returns>An enumerable collection of <see cref="DateTime"/> values representing each occurrence within the specified
    /// range. The collection will be empty if no occurrences fall within the range.</returns>
    public static IEnumerable<DateTime> GetOccurrences(
         RecurrenceRule rule,
         DateTime start,
         DateTime from,
         DateTime to,
         IEnumerable<DateTime>? exceptions = null)
    {
        // Normalize all dates to Unspecified kind to avoid timezone issues
        start = DateTime.SpecifyKind(start, DateTimeKind.Unspecified);
        from = DateTime.SpecifyKind(from, DateTimeKind.Unspecified);
        to = DateTime.SpecifyKind(to, DateTimeKind.Unspecified);

        var ex = (exceptions ?? [])
                 .Select(d => d.Date)
                 .ToHashSet();

        var until = rule.Until;
        var produced = 0;
        var maxCount = rule.Count ?? int.MaxValue;

        var stream = rule.Frequency switch
        {
            RecurrenceFrequency.Daily => GenerateDaily(start, from, to, rule.Interval, until),
            RecurrenceFrequency.Weekly => (rule.DaysOfWeek != null && rule.DaysOfWeek.Count > 0)
                ? GenerateWeeklyMultipleDays(start, from, to, rule.Interval, until, rule.DaysOfWeek)
                : GenerateWeeklySingleDay(start, from, to, rule.Interval, until),
            RecurrenceFrequency.Monthly => GenerateMonthly(start, from, to, rule.Interval, until, rule.DayOfMonth),
            RecurrenceFrequency.Yearly => GenerateYearly(start, from, to, rule.Interval, until, rule.Months),
            _ => []
        };

        foreach (var occ in stream)
        {
            if (rule.Count.HasValue && produced >= rule.Count.Value)
            {
                yield break;
            }

            if (!ex.Contains(occ.Date))
            {
                yield return occ;
                produced++;
            }
        }
    }

    /// <summary>
    /// Generates a sequence of dates occurring at daily intervals, starting from a specified date and time.
    /// </summary>
    /// <remarks>The time component of the start date is preserved for all generated dates. The sequence
    /// includes dates from the later of <paramref name="start"/> or <paramref name="from"/> up to <paramref name="to"/>
    /// and, if specified, <paramref name="until"/>.</remarks>
    /// <param name="start">The initial date and time from which the sequence begins.</param>
    /// <param name="from">The earliest date to include in the sequence. Dates before this value are excluded.</param>
    /// <param name="to">The latest date to include in the sequence. Dates after this value are excluded.</param>
    /// <param name="intervalDays">The number of days between each generated date. Must be greater than zero.</param>
    /// <param name="until">An optional upper bound for the sequence. If specified, no dates after this value are included.</param>
    /// <returns>An enumerable collection of DateTime values representing the generated dates at the specified interval. The
    /// collection is empty if no dates fall within the specified range.</returns>
    private static IEnumerable<DateTime> GenerateDaily(
        DateTime start, DateTime from, DateTime to, int intervalDays, DateTime? until)
    {
        var time = start.TimeOfDay;
        var candidate = start;

        if (from > start)
        {
            var days = (int)Math.Ceiling((from.Date - start.Date).TotalDays / intervalDays);
            candidate = SafeAddDays(start, days * intervalDays);
        }

        while (candidate <= to && (!until.HasValue || candidate <= until.Value))
        {
            yield return candidate.Date.Add(time);
            candidate = SafeAddDays(candidate, intervalDays);
        }
    }

    /// <summary>
    /// Generates a sequence of dates representing weekly occurrences on a single day, starting from a specified date
    /// and time, within a given range and interval.
    /// </summary>
    /// <remarks>The generated dates will always fall on the same day of the week and time as the <paramref
    /// name="start"/> parameter. The sequence includes only those occurrences that are on or after <paramref
    /// name="start"/> and within the specified range. If <paramref name="until"/> is provided, it further restricts the
    /// end of the sequence.</remarks>
    /// <param name="start">The initial date and time of the first occurrence. The day of week and time component are used for all generated
    /// dates.</param>
    /// <param name="from">The earliest date to consider for generating occurrences. Occurrences before this date are excluded.</param>
    /// <param name="to">The latest date to consider for generating occurrences. Occurrences after this date are excluded.</param>
    /// <param name="intervalWeeks">The number of weeks between each occurrence. Must be greater than zero.</param>
    /// <param name="until">An optional date that limits the last possible occurrence. If specified, no occurrences will be generated after
    /// this date.</param>
    /// <returns>An enumerable collection of DateTime values representing each occurrence that matches the specified criteria.</returns>
    private static IEnumerable<DateTime> GenerateWeeklySingleDay(
        DateTime start,
        DateTime from,
        DateTime to,
        int intervalWeeks,
        DateTime? until)
    {
        var time = start.TimeOfDay;
        var startDow = start.DayOfWeek;

        var candidateDate = from;
        var delta = ((int)startDow - (int)candidateDate.DayOfWeek + 7) % 7;
        candidateDate = candidateDate.AddDays(delta);
        candidateDate = candidateDate.Date.Add(time);

        var weeksBetween = (int)Math.Floor((candidateDate.Date - start.Date).TotalDays / 7.0);

        if (weeksBetween < 0)
        {
            weeksBetween = 0;
        }

        var remainder = weeksBetween % intervalWeeks;

        if (remainder != 0)
        {
            candidateDate = candidateDate.AddDays((intervalWeeks - remainder) * 7);
        }

        while (candidateDate <= to && (!until.HasValue || candidateDate <= until.Value))
        {
            if (candidateDate >= start)
            {
                yield return candidateDate;
            }

            candidateDate = candidateDate.AddDays(7 * intervalWeeks);
        }
    }

    /// <summary>
    /// Generates a sequence of dates representing weekly occurrences on multiple specified days, starting from a given
    /// date and within a defined range.
    /// </summary>
    /// <remarks>Occurrences are generated for each specified day of the week, repeating every intervalWeeks
    /// weeks, and always use the time of day from the start parameter. The sequence includes only dates that fall
    /// within the from and to range, and, if specified, do not exceed the until date.</remarks>
    /// <param name="start">The initial date and time of the first occurrence. Determines the time of day for all generated dates.</param>
    /// <param name="from">The earliest date to include in the generated sequence. Occurrences before this date are excluded.</param>
    /// <param name="to">The latest date to include in the generated sequence. Occurrences after this date are excluded.</param>
    /// <param name="intervalWeeks">The number of weeks between each set of weekly occurrences. Must be greater than zero.</param>
    /// <param name="until">An optional date that limits the last possible occurrence. If specified, no dates after this value are included.</param>
    /// <param name="days">A read-only list of days of the week on which occurrences should be generated. Duplicate days are ignored.</param>
    /// <returns>An enumerable collection of DateTime values representing each occurrence that matches the specified criteria.
    /// The collection may be empty if no dates fall within the specified range.</returns>
    private static IEnumerable<DateTime> GenerateWeeklyMultipleDays(
        DateTime start, DateTime from, DateTime to,
        int intervalWeeks, DateTime? until, IReadOnlyList<DayOfWeek> days)
    {
        var time = start.TimeOfDay;
        var orderedDays = days.Distinct().OrderBy(d => d).ToList();

        var baseWeekStart = from.Date;
        var startWeekAnchor = start.Date;
        var weeksFromStartToFrom = (int)Math.Floor((baseWeekStart - startWeekAnchor).TotalDays / 7.0);

        if (weeksFromStartToFrom < 0)
        {
            weeksFromStartToFrom = 0;
        }

        var remainder = weeksFromStartToFrom % intervalWeeks;

        if (remainder != 0)
        {
            baseWeekStart = baseWeekStart.AddDays((intervalWeeks - remainder) * 7);
        }

        while (baseWeekStart <= to && (!until.HasValue || baseWeekStart <= until.Value))
        {
            foreach (var dow in orderedDays)
            {
                var delta = ((int)dow - (int)baseWeekStart.DayOfWeek + 7) % 7;
                var candidate = baseWeekStart.AddDays(delta).Add(time);

                if (candidate >= start &&
                    candidate >= from &&
                    candidate <= to && (!until.HasValue || candidate <= until.Value))
                {
                    yield return candidate;
                }
            }

            baseWeekStart = baseWeekStart.AddDays(7 * intervalWeeks);
        }
    }

    /// <summary>
    /// Generates a sequence of dates representing monthly occurrences within a specified range, optionally constrained
    /// by an interval, end date, and day of month.
    /// </summary>
    /// <remarks>If the specified day of month does not exist in a given month (e.g., February 30), the
    /// occurrence is set to the last valid day of that month. The time component of each occurrence matches the time of
    /// <paramref name="start"/>.</remarks>
    /// <param name="start">The initial date of the recurrence series. The time component of this date is used for all generated
    /// occurrences.</param>
    /// <param name="from">The earliest date to include in the generated sequence. Occurrences before this date are excluded.</param>
    /// <param name="to">The latest date to include in the generated sequence. Occurrences after this date are excluded.</param>
    /// <param name="intervalMonths">The number of months between each occurrence. Must be greater than zero.</param>
    /// <param name="until">An optional date that limits the last possible occurrence. If specified, no occurrence will be generated after
    /// this date.</param>
    /// <param name="dayOfMonth">An optional day of the month for each occurrence. If null, the day from <paramref name="start"/> is used. Values
    /// outside the valid range for a given month are adjusted to the last valid day.</param>
    /// <returns>An enumerable collection of <see cref="DateTime"/> values representing each monthly occurrence within the
    /// specified constraints.</returns>
    private static IEnumerable<DateTime> GenerateMonthly(
        DateTime start,
        DateTime from,
        DateTime to,
        int intervalMonths,
        DateTime? until,
        int? dayOfMonth)
    {
        var time = start.TimeOfDay;
        var dom = dayOfMonth ?? start.Day;

        var candidate = new DateTime(start.Year, start.Month, 1);

        if (from > candidate)
        {
            var diffMonths = (from.Year - candidate.Year) * 12 + (from.Month - candidate.Month);

            if (diffMonths > 0)
            {
                candidate = SafeAddMonths(candidate, diffMonths);
            }
        }

        while (candidate <= to && (!until.HasValue || candidate <= until.Value))
        {
            var occ = SafeCreateDate(candidate.Year, candidate.Month, dom, time);

            if (occ >= start && occ >= from && occ <= to && (!until.HasValue || occ <= until.Value))
            {
                yield return occ;
            }

            candidate = SafeAddMonths(candidate, intervalMonths);
        }
    }

    /// <summary>
    /// Generates a sequence of yearly recurrence dates within the specified range, occurring in the given months and at
    /// the same time of day as the start date.
    /// </summary>
    /// <remarks>Duplicate months in the months list are ignored. The day component from the start date is
    /// used for all generated dates. If a generated date is invalid (e.g., February 30), it is skipped.</remarks>
    /// <param name="start">The initial date and time of the recurrence pattern. The time of day from this value is used for all generated
    /// dates.</param>
    /// <param name="from">The earliest date to include in the generated sequence. Only dates on or after this value are returned.</param>
    /// <param name="to">The latest date to include in the generated sequence. Only dates on or before this value are returned.</param>
    /// <param name="intervalYears">The number of years between each recurrence. Must be greater than zero.</param>
    /// <param name="until">An optional upper bound for the recurrence dates. If specified, no dates after this value are returned.</param>
    /// <param name="months">A list of months (as integers from 1 to 12) in which recurrences should occur. If null or empty, the month from
    /// the start date is used.</param>
    /// <returns>An enumerable collection of DateTime values representing the recurrence dates that match the specified criteria.
    /// The collection may be empty if no dates fall within the given range.</returns>
    private static IEnumerable<DateTime> GenerateYearly(
        DateTime start,
        DateTime from,
        DateTime to,
        int intervalYears,
        DateTime? until,
        List<int> months)
    {
        var time = start.TimeOfDay;

        var monthsList = (months != null && months.Count > 0)
            ? months.Distinct().OrderBy(m => m).ToList()
            : [start.Month];

        var year = start.Year;

        if (from > start)
        {
            year = from.Year;
        }

        while (year <= to.Year && (!until.HasValue || year <= until.Value.Year))
        {
            foreach (var m in monthsList)
            {
                var occ = SafeCreateDate(year, m, start.Day, time);

                if (occ >= start && occ >= from && occ <= to && (!until.HasValue || occ <= until.Value))
                {
                    yield return occ;
                }
            }

            year = SafeAddYearsYear(year, intervalYears);
        }
    }

    /// <summary>
    /// Adds the specified number of days to the given date, returning a fallback value if the operation fails.
    /// </summary>
    /// <remarks>This method provides a safe way to add days to a date without throwing exceptions for
    /// out-of-range results. If the addition would result in a date outside the valid range of DateTime,
    /// DateTime.MaxValue is returned instead.</remarks>
    /// <param name="dt">The date to which days will be added.</param>
    /// <param name="days">The number of days to add to the specified date. Can be negative to subtract days.</param>
    /// <returns>A DateTime representing the result of adding the specified number of days to the input date. If the addition
    /// results in an invalid date, returns DateTime.MaxValue.</returns>
    private static DateTime SafeAddDays(DateTime dt, int days)
    {
        try
        {
            return dt.AddDays(days);
        }
        catch
        {
            return DateTime.MaxValue;
        }
    }

    /// <summary>
    /// Adds the specified number of months to the given date, returning a safe result if the operation would exceed
    /// supported date ranges.
    /// </summary>
    /// <remarks>This method prevents exceptions that may occur when adding months results in a date outside
    /// the valid range for DateTime. Use this method when you need to ensure that the operation does not throw and
    /// instead returns a sentinel value.</remarks>
    /// <param name="dt">The date to which months will be added.</param>
    /// <param name="months">The number of months to add to the date. Can be positive or negative.</param>
    /// <returns>A DateTime representing the result of adding the specified number of months to the input date. If the operation
    /// would result in a date outside the supported range, DateTime.MaxValue is returned.</returns>
    private static DateTime SafeAddMonths(DateTime dt, int months)
    {
        try
        {
            return dt.AddMonths(months);
        }
        catch
        {
            return DateTime.MaxValue;
        }
    }

    /// <summary>
    /// Safely adds a specified number of years to a given year, returning the maximum supported year if the result
    /// exceeds the valid range.
    /// </summary>
    /// <remarks>If the addition causes an overflow beyond the maximum supported year, the method returns <see
    /// cref="DateTime.MaxValue.Year"/> instead of throwing an exception.</remarks>
    /// <param name="year">The base year to which the additional years will be added.</param>
    /// <param name="years">The number of years to add to the base year. Can be negative to subtract years.</param>
    /// <returns>The resulting year after addition, or <see cref="DateTime.MaxValue.Year"/> if the calculation exceeds the
    /// supported range.</returns>
    private static int SafeAddYearsYear(int year, int years)
    {
        try
        {
            checked
            {
                return year + years;
            }
        }
        catch
        {
            return DateTime.MaxValue.Year;
        }
    }

    /// <summary>
    /// Creates a DateTime value from the specified year, month, day, and time, adjusting invalid date components to the
    /// nearest valid value.
    /// </summary>
    /// <remarks>If any of the provided date components are out of range, they are automatically corrected to
    /// valid values. This method does not throw exceptions for invalid input; instead, it returns DateTime.MaxValue if
    /// the date cannot be constructed.</remarks>
    /// <param name="year">The year component of the date. Must be within the range supported by DateTime.</param>
    /// <param name="month">The month component of the date. Values less than 1 are treated as 1; values greater than 12 are treated as 12.</param>
    /// <param name="day">The day component of the date. Values outside the valid range for the specified month and year are adjusted to
    /// the nearest valid day.</param>
    /// <param name="time">The time of day to add to the resulting date, represented as a TimeSpan.</param>
    /// <returns>A DateTime representing the adjusted date and time. Returns DateTime.MaxValue if the date cannot be created.</returns>
    private static DateTime SafeCreateDate(int year, int month, int day, TimeSpan time)
    {
        month = Math.Clamp(month, 1, 12);

        var daysInMonth = DateTime.DaysInMonth(Math.Clamp(year, DateTime.MinValue.Year, DateTime.MaxValue.Year), month);
        var safeDay = Math.Clamp(day, 1, daysInMonth);

        try
        {
            return new DateTime(year, month, safeDay).Add(time);
        }
        catch
        {
            return DateTime.MaxValue;
        }
    }
}

