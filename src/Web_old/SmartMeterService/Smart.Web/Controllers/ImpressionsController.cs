using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smart.Helpers;
using Smart.Helpers.Contracts;
using Smart.Helpers.DB;
using Smart.Web.Models;

namespace Smart.Web.Controllers
{

    [Route("impress")]
    public class ImpressionsController : Controller
    {
        private readonly IOptions<PowerSecrets> _powerOptions;
        private readonly PowerContext _context;
        private readonly IDatabaseService _databaseService;

        public ImpressionsController(IOptions<PowerSecrets> powerOptions, PowerContext context, 
            IDatabaseService databaseService)
        {
            _powerOptions = powerOptions;
            _context = context;
            _databaseService = databaseService;
        }

        [Route("impressimp")]
        public async Task<IActionResult> Index(int imp, int time)
        {
            var result = KWHelper.CalcKWH(imp, time);
            return await Index(result);
        }

        [Route("kwh")]
        public async Task<IActionResult> Index(double kwh)
        {
            var result = kwh;
            
            var resultDay = result * 24;
            if (resultDay < 0)
            {
                resultDay = 0;
            }
            decimal resultDollars = ((decimal)resultDay * 28.52M) / 100M;
            var rString = string.Format("{0:0.00}", result);
            var rStringDay = string.Format("{0:0.00}", resultDay);
            var rDollars = resultDollars.ToString("C0");

           
            Console.WriteLine($"{rString}kwh -> {rStringDay} kwh per day -> {rDollars} per day @ {DateTime.Now.ToString()}");

            

            await _context.PowerReadings.AddAsync(new PowerReading
            {
                KilowattHours = result,
                DailyKilowattHours = result * 24,
                MeasureTime = DateTime.Now
            });

            await _context.SaveChangesAsync();

            var averageSoFarYesterday = await _databaseService.AverageSoFarYesterday();
            var averageSoFarToday = await _databaseService.AverageSoFarToday();
            var averageLast24Hours = await _databaseService.AverageLast24Hours();
            var averageYesterday = await _databaseService.AverageYesterday();

            await PowerBIHelper.Push(result,
                averageSoFarToday, 
                averageSoFarYesterday,
                averageLast24Hours, 
                averageYesterday,
                resultDollars,
                _powerOptions.Value.PowerBiUrl);

            return Ok(result);
        }
        

        [Route("boot")]
        public async Task<IActionResult> BootTest()
        {
            Console.WriteLine($"Options: DB - ${_powerOptions.Value.DBConnectionString}, PowerBi - {_powerOptions.Value.PowerBiUrl}");
            await PowerBIHelper.Push("Booted",
                _powerOptions.Value.PowerBiUrl);
            Console.WriteLine("Booted");
            return Ok("Good");
        }

        [Route("debug")]
        public async Task<IActionResult> Debug(string debugString)
        {
            await PowerBIHelper.Push(debugString,
                _powerOptions.Value.PowerBiUrl);
            Console.WriteLine($"From Device Debug: {debugString}");
            return Ok("Good");
        }

        
    }
}