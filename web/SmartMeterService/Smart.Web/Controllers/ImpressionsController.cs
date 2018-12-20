using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Smart.Web.Controllers
{
    [Route("impress")]
    public class ImpressionsController : Controller
    {
        public IActionResult Index(int imp, int time)
        {
            Console.WriteLine($"Impressions: {imp} -> time: {time}");
            return Ok("Good");
        }
    }
}