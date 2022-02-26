using FlightPlanner2.Migrations;
using FlightPlanner2.Models;
using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightPlanner2.Controllers
{
    [EnableCors]
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FlightPlannerDBContext _context;

        public CustomerController(FlightPlannerDBContext context)
        {
            _context = context;
        }

        [EnableCors]
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            lock (FlightStorage._lock)
            {
                var airportArray = FlightStorage.GetAirportByKeyword(search, _context);
                return airportArray.Length == 0 ? Ok(search) : Ok(airportArray);
            }
        }

        [EnableCors]
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
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
        [HttpPost]
        [Route("flights/search/")]
        public IActionResult PostSearchFlights(SearchFlightRequest searchFlightRequest)
        {
            lock (FlightStorage._lock)
            {
                if (searchFlightRequest.From == searchFlightRequest.To)
                {
                    return BadRequest();
                }

                var searchResults = FlightStorage.SearchByParams(searchFlightRequest.From, searchFlightRequest.To,
                    searchFlightRequest.Date, _context);
                return Ok(searchResults);
            }
        }
    }
}

