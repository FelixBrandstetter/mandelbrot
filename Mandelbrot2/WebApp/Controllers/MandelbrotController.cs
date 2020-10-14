using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MandelbrotLibrary;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    [ApiController]
    public class MandelbrotController : ControllerBase
    {

        // GET: api/mandelbrot
        [HttpGet]
        [Route("api/mandelbrot")]
        public IEnumerable<string> Get()
        {
            Calculator calc = new Calculator();
            calc.CalculateArea();
            List<Result> results = calc.CalculatePixels(1, 2);
            return new string[] { "value1", "value2" };
        }

        // POST api/<MandelbrotController>
        // public async Task<List<Result>> Post([FromBody] string value)
        [HttpPost]
        [Route("api/mandelbrot")]
        public async Task<List<Result>> Post([FromBody] CalcServerInfo serverInfo)
        {
            Calculator calc = new Calculator();
            var results = await calc.CalculateArea();
            return results;
        }
    }
}
