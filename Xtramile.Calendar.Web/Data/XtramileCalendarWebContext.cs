using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xtramile.Calendar.Web.Models;

namespace Xtramile.Calendar.Web.Data
{
    public class XtramileCalendarWebContext : DbContext
    {
        public XtramileCalendarWebContext (DbContextOptions<XtramileCalendarWebContext> options)
            : base(options)
        {
        }

        public DbSet<Xtramile.Calendar.Web.Models.DateRangeViewModel> DateRangeViewModel { get; set; }
    }
}
