using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Xtramile.Calendar.Web.Models
{
    public class DateRangeViewModel
    {
        [Display(Name = "From Date")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

    }
}
