using System;
using System.Globalization;

namespace DateTimeExtensions
{
    public static class DayExtensions
    {
        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            DateTime first = current.AddDays(1 - current.Day);
            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.First();

            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.Next(dayOfWeek);
            }

            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            DateTime last = current.First().AddDays(daysInMonth - 1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.Last();

            last = last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek) * -1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;

            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }

            DateTime result = current.AddDays(offsetDays);
            return result;
        }

        /// <summary>
        /// Gets a DateTime representing the next unit of time, such as week or month
        /// </summary>
        /// <param name="current">The current date.</param>
        /// <param name="dateUnit">The date unit (week,month,year,day).</param>
        /// <returns></returns>
        public static DateTime Next(this DateTime current, DateUnit dateUnit)
        {
            switch (dateUnit)
            {
                case DateUnit.Day:
                    return current.AddDays(1);
                case DateUnit.Week:
                    return current.Next(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
                case DateUnit.Month:
                    return current.Last().AddDays(1);
                case DateUnit.Year:
                    return new DateTime(current.Year + 1,1,1);
            }
            return current;
        }

        /// <summary>
        /// Gets a DateTime representing the last unit of time, such as week or month.
        /// </summary>
        /// <param name="current">The current date.</param>
        /// <param name="dateUnit">The date unit.</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DateUnit dateUnit)
        {
            switch (dateUnit)
            {
                case DateUnit.Day:
                    return current.AddDays(-1);
                case DateUnit.Week:
                    return current.AddDays(-14).Next(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
                case DateUnit.Month:
                    return current.First().AddDays(-1).First();
                case DateUnit.Year:
                    return new DateTime(current.Year - 1,1,1);
            }
            return current;
        }

        /// <summary>
        /// Get a DateTime representing the current unit of time.
        /// </summary>
        /// <param name="current">The current date.</param>
        /// <param name="dateUnit">The date unit.</param>
        /// <returns></returns>
        public static DateTime This(this DateTime current, DateUnit dateUnit)
        {
            switch (dateUnit)
            {
                case DateUnit.Day:
                    return current.Midnight();
                case DateUnit.Week:
                    return current.AddWeeks(-1).Next(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
                case DateUnit.Month:
                    return current.First();
                case DateUnit.Year:
                    return new DateTime(current.Year,1,1);
            }
            return current;
        }

        /// <summary>
        /// Gets a DateTime that has the specified number of weeks added.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="weeks">The weeks.</param>
        /// <returns></returns>
        public static DateTime AddWeeks(this DateTime current,double weeks)
        {
            return current.AddDays(weeks*7);
        }
    }
}
