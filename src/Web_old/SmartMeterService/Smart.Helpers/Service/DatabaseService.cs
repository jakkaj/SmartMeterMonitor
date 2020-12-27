using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Smart.Helpers.Contracts;
using Smart.Helpers.DB;

namespace Smart.Helpers.Service
{
    public class DatabaseService : IDatabaseService
    {
        private readonly PowerContext _context;

        public DatabaseService(PowerContext context)
        {
            _context = context;
        }

        public async Task<double> AverageSoFarToday()
        {
            var average = await _context.PowerReadings.Where(_ => _.MeasureTime > DateTime.UtcNow.Date).DefaultIfEmpty().Select(_ => _.DailyKilowattHours).AverageAsync();
            return average;
        }

        public async Task<double> AverageSoFarYesterday()
        {
            var yesterday = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
            var dayBefore = DateTime.UtcNow.Subtract(TimeSpan.FromDays(2));
            var average = await _context.PowerReadings.Where(_ => _.MeasureTime > dayBefore && _.MeasureTime < yesterday).DefaultIfEmpty().Select(_ => _.DailyKilowattHours).AverageAsync();
            return average;
        }

        public async Task<double> AverageYesterday()
        {
            var yesterday = DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(1));
            var today = DateTime.UtcNow.Date;
            var average = await _context.PowerReadings.Where(_ => _.MeasureTime > yesterday && _.MeasureTime < today).DefaultIfEmpty().Select(_ => _.DailyKilowattHours).AverageAsync();
            return average;
        }

        public async Task<double> AverageLast24Hours()
        {
            var yesterday = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
            var average = await _context.PowerReadings.Where(_ => _.MeasureTime > yesterday).DefaultIfEmpty().Select(_ => _.DailyKilowattHours).AverageAsync();
            return average;
        }
    }
}
