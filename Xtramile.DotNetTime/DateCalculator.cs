using System;
using System.Collections.Generic;
using System.Linq;

namespace Xtramile.DotNetTime
{
    public static class DateCalculator
    {
        public static bool IsWeekDay(DayOfWeek dayOfWeek) => dayOfWeek == DayOfWeek.Monday || dayOfWeek == DayOfWeek.Tuesday
            || dayOfWeek == DayOfWeek.Wednesday || dayOfWeek == DayOfWeek.Thursday || dayOfWeek == DayOfWeek.Friday;

        public static DateTime NextWeekDay(this DateTime fromDate)
        {
            var nextDate = fromDate.AddDays(1);
            while (!IsWeekDay(nextDate.DayOfWeek)) nextDate = nextDate.AddDays(1);

            return nextDate;
        }

        /// <summary>
        /// Calculates number of business days. Exclusive of <param name="fromDate"></param>, but by default inclusive of <param name="endDate"></param>
        /// Does not take into account public holidays
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="endDate"></param>
        /// <param name="includesEndDate"></param>
        /// <returns></returns>
        public static int RemainingWeekDays(this DateTime fromDate, DateTime endDate, bool includesEndDate = true)
        {
            if (!includesEndDate) endDate = endDate.AddDays(-1);
            TimeSpan span = endDate - fromDate;
            int totalDays = span.Days;
            int fullWeekCount = totalDays / 7;
            if (totalDays % 7 == 0) return fullWeekCount * 5;
            // find out if there are weekends during the time exceeding the full weeks
            int firstDayOfWeek = (int)fromDate.DayOfWeek;
            int lastDayOfWeek = ((int)endDate.DayOfWeek > firstDayOfWeek) ? (int)endDate.DayOfWeek : (int)endDate.DayOfWeek + 7; // if smaller than firstDayOfWeek we should add 7 days
            int firstToLastDayCount = lastDayOfWeek - firstDayOfWeek;
            if ((firstDayOfWeek < (int)DayOfWeek.Saturday) && ((int)DayOfWeek.Saturday) <= lastDayOfWeek) firstToLastDayCount--; // subtract saturday (6th day)
            if (firstDayOfWeek < 7 && 7 <= lastDayOfWeek) firstToLastDayCount--; // subtract sunday (7th day)
            
            return fullWeekCount * 5 + firstToLastDayCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="endDate"></param>
        /// <param name="publicHolidays"></param>
        /// <returns></returns>
        public static int RemainingBusinessDays(this DateTime fromDate, DateTime endDate, 
            IEnumerable<IHoliday> publicHolidays)
        {
            int businessDays = fromDate.RemainingWeekDays(endDate, false);
            // subtract the number of bank holidays during the time interval
            foreach (var holiday in publicHolidays) // flatten
            {
                if (fromDate < holiday.Date.Date && holiday.Date.Date < endDate)
                {
                    businessDays--;
                }
            }

            return businessDays;
        }
    }
}
