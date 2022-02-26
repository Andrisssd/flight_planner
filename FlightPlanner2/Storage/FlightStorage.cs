using FlightPlanner2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner2.Storage
{
    public class FlightStorage
    {
        public static readonly object _lock = new object();

        public static bool IsDublicate(AddFlightRequest request, FlightPlannerDBContext context)
        {
            lock (FlightStorage._lock)
            {
                var currentFlight = ConvertRequestToFlight(request);
                foreach (var flight in context.Flights.Include(f => f.To).Include(f => f.From))
                {
                    if (flight.Equals(currentFlight))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static Flight ConvertRequestToFlight(AddFlightRequest request)
        {
            lock (_lock)
            {
                Flight flight = new Flight
                {
                    ArrivalTime = request.ArrivalTime,
                    DepartureTime = request.DepartureTime,
                    Carrier = request.Carrier,
                    From = request.From,
                    To = request.To,
                };
                return flight;
            }
        }

        public static Airport[] GetAirportByKeyword(string keyword, FlightPlannerDBContext _context)
        {
            lock (_lock)
            {
                var cleanKeyword = keyword.Trim().ToUpper();
                var airports = _context.Flights
                    .Select(a => a.From)
                    .Where(a => a.AirportName.ToUpper().StartsWith(cleanKeyword) ||
                                a.City.ToUpper().StartsWith(cleanKeyword) ||
                                a.Country.ToUpper().StartsWith(cleanKeyword));

                return airports.ToArray();
            }
        }

        public static bool IsValid(Flight flight)
        {
            lock (_lock)
            {
                return flight != null && flight.IsValid();
            }
        }

        public static bool IsValidTimeframe(Flight flight)
        {
            lock (_lock)
            {
                var departureTime = DateTime.Parse(flight.DepartureTime);
                var arrivalTime = DateTime.Parse(flight.ArrivalTime);

                return departureTime < arrivalTime;
            }
        }

        public static SearchFlightResult SearchByParams(string from, string to, string date, FlightPlannerDBContext _context)
        {
            lock (_lock)
            {
                var filteredFlight = _context.Flights.Where(f => f.From.AirportName == from || f.To.AirportName == to || f.DepartureTime == date).ToArray();

                return new SearchFlightResult(filteredFlight);
            }
        }
    }
}

