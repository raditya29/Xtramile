using System;
using System.Collections.Generic;
using System.Linq;

namespace Xtramile.DotNetTime
{
    public interface IYearlyHolidayCollectionFactory
    {
        IEnumerable<IDynamicHoliday> GenerateHolidaysFor(int year);
    }

    public class YearlyDynamicHolidayCollectionFactory : IYearlyHolidayCollectionFactory
    {
        private static IEnumerable<YearlyHolidayRecord> yearlyHolidayRecords() 
        {
            yield return new YearlyHolidayRecord(1, 1, DynamicHolidayType.SameDayIfNotWeekendHoliday); // New Year
            yield return new YearlyHolidayRecord(25, 4, DynamicHolidayType.AlwaysSameDayHoliday); // Anzac Day
            yield return new YearlyHolidayRecord(1, 6, DynamicHolidayType.CertainDayInMonthHoliday, DayOfWeek.Monday, 2); // Queen's Birthday
        }

        public IEnumerable<IDynamicHoliday> GenerateHolidaysFor(int year) =>
            yearlyHolidayRecords().Select(record => DynamicHolidayConstructor(record, year));

        private IDynamicHoliday DynamicHolidayConstructor(YearlyHolidayRecord record, int year)
        {
            switch (record.Type)
            {
                case DynamicHolidayType.AlwaysSameDayHoliday:
                    return new AlwaysSameDayHoliday(new DateTime(year, record.Month, record.Day));
                case DynamicHolidayType.SameDayIfNotWeekendHoliday:
                    return new SameDayIfNotWeekendHoliday(new DateTime(year, record.Month, record.Day));
                case DynamicHolidayType.CertainDayInMonthHoliday:
                    return new CertainDayInMonthHoliday(record.DayOfWeek, record.Occurence, record.Month);
                default:
                    throw new InvalidOperationException(nameof(record.Type));
            }
        }
    }
}
