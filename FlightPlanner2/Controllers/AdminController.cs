using FlightPlanner2.Models;
using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanner2.Controllers
{
    [EnableCors]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly FlightPlannerDBContext _context;

        public AdminController(FlightPlannerDBContext context)
        {
            _context = context;
        }

        [EnableCors]
        [Authorize]
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            lock (FlightStorage._lock)
            {
                var flight = _context.Flights
                    .Include(f => f.To)
                    .Include(f => f.From)
                    .SingleOrDefault(f => f.Id == id);
                if (flight != null)
                {
                    return Ok(flight);
                }

                return NotFound($"Flight with id: {id} not found");
            }
        }

        [EnableCors]
        [Authorize]
        [HttpPut]
        [Route("flights")]
        public IActionResult PutFlight(AddFlightRequest request)
        {
            lock (FlightStorage._lock)
            {
                var flight = FlightStorage.ConvertRequestToFlight(request);

                if (FlightStorage.IsDublicate(request, _context))
                {
                    return Conflict();
                }

                if (!FlightStorage.IsValid(flight) || !FlightStorage.IsValidTimeframe(flight))
                {
                    return BadRequest();
                }

                _context.Flights.Add(flight);
                _context.SaveChanges();
                return Created("", flight);
            }
        }

        [EnableCors]
        [Authorize]
        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (FlightStorage._lock)
            {
                var flight = _context.Flights
                    .Include(f => f.To)
                    .Include(f => f.From)
                    .SingleOrDefault(f => f.Id == id);

                if(flight == null)
                {
                    return Ok();
                }

                _context.Flights.Remove(flight);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
}
