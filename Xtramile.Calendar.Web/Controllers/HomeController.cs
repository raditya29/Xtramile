using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xtramile.Calendar.Web.Models;
using Xtramile.DotNetTime;

namespace Xtramile.Calendar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IYearlyHolidayCollectionFactory holidaysFactory;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
            holidaysFactory = new YearlyDynamicHolidayCollectionFactory(); // should be resolved by dependency injection
        }

        public IActionResult Index()
        {
            return View(new DateRangeViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.NextWeekDay() });
        }

        // POST: Home/Calculate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate([Bind("FromDate,ToDate")] DateRangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // arrange
                var fromDate = model.FromDate;
                var endDate = model.ToDate;
                int[] yearsRange = Enumerable.Range(fromDate.Year, (endDate.Year - fromDate.Year + 1)).ToArray(); // years included in date range
                var publicHolidays = yearsRange.SelectMany(year => holidaysFactory.GenerateHolidaysFor(year))
                                        .Where(holiday => fromDate < holiday.ActualDate && holiday.ActualDate < endDate); // public holidays in between time range
                // act
                int businessDays = fromDate.RemainingBusinessDays(endDate, publicHolidays);
                // return
                return Json(new { businessDays, publicHolidays = publicHolidays.Select(day => day.ActualDate.ToShortDateString()) });
            }

            return RedirectToAction(nameof(Index), model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
