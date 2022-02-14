using FlightPlanner2.Models;
using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner2.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
            var airportArray = FlightStorage.GetAirportByKeyword(search);
            return airportArray.Length == 0 ? Ok(search) : Ok(airportArray);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetById(id);
            return flight == null ? NotFound() : Ok(flight);
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
                searchFlightRequest.Date);
            return Ok(searchResults);
        }
    }
}

