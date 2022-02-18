using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner2.Controllers
{
    [EnableCors]
    [Route("testing-api")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        [EnableCors]
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights();
            return Ok();
        }
    }
}
