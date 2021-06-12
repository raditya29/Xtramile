using System;
using Microsoft.AspNetCore.Mvc;
using Xtramile.Calendar.Web.Models;
using Xtramile.DotNetTime;

namespace Xtramile.Calendar.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new DateRangeViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.NextWeekDay() });
        }
    }
}
