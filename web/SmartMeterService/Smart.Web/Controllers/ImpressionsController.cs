using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smart.Helpers;

namespace Smart.Web.Controllers
{

    [Route("impress")]
    public class ImpressionsController : Controller
    {
        public async Task<IActionResult> Index(int imp, int time)
        {
            Console.WriteLine($"Impressions: {imp} -> time: {time}");

            var result = KWHelper.CalcKWH(imp, time);
            var rString = string.Format("{0:0.00}", result);
            Console.WriteLine($"{rString}kwh");

            await PowerBIHelper.Push(result);

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