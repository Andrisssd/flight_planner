using FlightPlanner2.Models;
using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner2.Controllers
{
    [EnableCors]
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [EnableCors]
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            var airportArray = FlightStorage.GetAirportByKeyword(search);
            return airportArray.Length == 0 ? Ok(search) : Ok(airportArray);
        }

        [EnableCors]
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetById(id);
            return flight == null ? NotFound() : Ok(flight);
        }

        [EnableCors]
        [HttpPost]
        [Route("flights/search/")]
        public IActionResult PostSearchFlights(SearchFlightRequest searchFlightRequest)
        {
            if (searchFlightRequest.From == searchFlightRequest.To)
            {
                return BadRequest();
            }

            var searchResults = FlightStorage.SearchByParams(searchFlightRequest.From, searchFlightRequest.To,
                searchFlightRequest.Date);
            return Ok(searchResults);
        }
    }
}

