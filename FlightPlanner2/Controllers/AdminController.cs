using FlightPlanner2.Models;
using FlightPlanner2.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace FlightPlanner2.Controllers
{
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly object storage = new object();

        [Authorize]
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            lock (storage)
            {
                Flight flight = FlightStorage.GetById(id);
                if (flight != null)
                {
                    return Ok(flight);
                }

                return NotFound($"Flight with id: {id} not found");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("flights")]
        public IActionResult PutFlight(Flight flight)
        {
            lock (storage)
            {
                if (FlightStorage.IsDuplicate(flight))
                {
                    return Conflict();
                }

                if (!FlightStorage.IsValid(flight) || !FlightStorage.IsValidTimeframe(flight))
                {
                    return BadRequest();
                }

                FlightStorage.AddFlight(flight);
                return Created("", flight);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (storage)
            {
                FlightStorage.DeleteFlight(id);
                return Ok();
            }
        }
    }
}
