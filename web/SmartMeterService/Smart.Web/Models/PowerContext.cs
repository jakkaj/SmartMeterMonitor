using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Smart.Web.Models
{
    public class PowerContext : DbContext
    {
        public PowerContext(DbContextOptions<PowerContext> options)
            : base(options)
        { }

        public DbSet<PowerReading> PowerReadings { get; set; }
    }

    public class PowerReading
    {
        public int PowerReadingId { get; set; }
        public double KilowattHours { get; set; }
        public double DailyKilowattHours { get; set; }
        public DateTime MeasureTime { get; set; }
        
    }

}
