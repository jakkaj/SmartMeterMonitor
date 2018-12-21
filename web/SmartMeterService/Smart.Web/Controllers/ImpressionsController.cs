using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smart.Helpers;
using Smart.Web.Models;

namespace Smart.Web.Controllers
{

    [Route("impress")]
    public class ImpressionsController : Controller
    {
        private readonly IOptions<PowerSecrets> _powerOptions;
        private readonly PowerContext _context;

        public ImpressionsController(IOptions<PowerSecrets> powerOptions, PowerContext context)
        {
            _powerOptions = powerOptions;
            _context = context;
        }
        public async Task<IActionResult> Index(int imp, int time)
        {
            Console.WriteLine($"Impressions: {imp} -> time: {time}");

            var result = KWHelper.CalcKWH(imp, time);
            
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

            await PowerBIHelper.Push(result, _powerOptions.Value.PowerBiUrl);

            await _context.PowerReadings.AddAsync(new PowerReading
            {
                KilowattHours = result,
                DailyKilowattHours = result * 24,
                MeasureTime = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [Route("boot")]
        public IActionResult BootTest()
        {
            Console.WriteLine("Booted");
            return Ok("Good");
        }

        [Route("debug")]
        public IActionResult Debug(string debugString)
        {
            Console.WriteLine($"From Device Debug: {debugString}");
            return Ok("Good");
        }

        
    }
}