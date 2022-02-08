using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner2.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights();
            return Ok();
        }
    }
}
