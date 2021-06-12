using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtramile.Calendar.Web.Models;
using Xtramile.DotNetTime;

namespace Xtramile.Calendar.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateRangeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private readonly IYearlyHolidayCollectionFactory holidaysFactory;

        public DateRangeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
            holidaysFactory = new YearlyDynamicHolidayCollectionFactory(); // should be resolved by dependency injection
        }

        // POST: api/DateRange
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<object> PostDateRangeViewModel([Bind("FromDate, ToDate")]DateRangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // arrange
                var fromDate = model.FromDate;
                var endDate = model.ToDate;
                int[] yearsRange = Enumerable.Range(fromDate.Year, (endDate.Year - fromDate.Year + 1)).ToArray(); // years included in date range
                var publicHolidays = yearsRange.SelectMany(year => holidaysFactory.GenerateHolidaysFor(year))
                                        .Where(holiday => fromDate < holiday.Date && holiday.Date < endDate); // public holidays in between time range
                // act
                int businessDays = fromDate.RemainingBusinessDays(endDate, publicHolidays);
                // return
                return new { businessDays, publicHolidays = publicHolidays.Select(day => day.Date.ToShortDateString()) };
            }

            return BadRequest();
        }
    }
}
