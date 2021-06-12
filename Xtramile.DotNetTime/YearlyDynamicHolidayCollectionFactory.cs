using System;
using System.Collections.Generic;
using System.Linq;

namespace Xtramile.DotNetTime
{
    public interface IYearlyHolidayCollectionFactory
    {
        IEnumerable<IHoliday> GenerateHolidaysFor(int year);
    }

    public class YearlyDynamicHolidayCollectionFactory : IYearlyHolidayCollectionFactory
    {
        private static IEnumerable<YearlyHolidayRecord> yearlyHolidayRecords() 
        {
            yield return new YearlyHolidayRecord(1, 1, HolidayType.SameDayIfNotWeekendHoliday); // New Year
            yield return new YearlyHolidayRecord(25, 4, HolidayType.AlwaysSameDayHoliday); // Anzac Day
            yield return new YearlyHolidayRecord(1, 6, HolidayType.CertainDayInMonthHoliday, DayOfWeek.Monday, 2); // Queen's Birthday
        }

        public IEnumerable<IHoliday> GenerateHolidaysFor(int year) =>
            yearlyHolidayRecords().Select(record => DynamicHolidayConstructor(record, year));

        private IHoliday DynamicHolidayConstructor(YearlyHolidayRecord record, int year)
        {
            switch (record.Type)
            {
                case HolidayType.AlwaysSameDayHoliday:
                    return new AlwaysSameDayHoliday(new DateTime(year, record.Month, record.Day));
                case HolidayType.SameDayIfNotWeekendHoliday:
                    return new SameDayIfNotWeekendHoliday(new DateTime(year, record.Month, record.Day));
                case HolidayType.CertainDayInMonthHoliday:
                    return new CertainDayInMonthHoliday(record.DayOfWeek, record.Occurence, record.Month);
                default:
                    throw new InvalidOperationException(nameof(record.Type));
            }
        }
    }
}
