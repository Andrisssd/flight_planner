using FlightPlanner2.Migrations;
using FlightPlanner2.Models;
using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq;

namespace FlightPlanner2.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FlightPlannerDBContext _context;

        public CustomerController(FlightPlannerDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            var airportArray = FlightStorage.GetAirportByKeyword(search, _context);
            return airportArray.Length == 0 ? Ok(search) : Ok(airportArray);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            //var flight = _context.Flights
            //        .SingleOrDefault(f => f.Id == id);
            //if (flight != null)
            //{
            //    return Ok(flight);
            //}

            //return NotFound($"Flight with id: {id} not found");


            //var flight = _context.Flights
            //    .Include(f => f.To)
            //    .Include(f => f.From)
            //    .SingleOrDefault(f => f.Id == id);
            //System.Console.WriteLine(flight);

            //    if (flight == null)
            //    {
            //        return NotFound();
            //    }

            //    return Ok(flight);

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

        [HttpPost]
        [Route("flights/search/")]
        public IActionResult PostSearchFlights(SearchFlightRequest searchFlightRequest)
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

