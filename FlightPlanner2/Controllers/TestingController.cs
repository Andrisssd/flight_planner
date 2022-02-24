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
        private readonly FlightPlannerDBContext _context;

        public TestingController(FlightPlannerDBContext context)
        {
            _context = context;
        }

        [EnableCors]
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            return Ok();
        }
    }
}
