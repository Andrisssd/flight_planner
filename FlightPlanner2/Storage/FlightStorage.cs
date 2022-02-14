using FlightPlanner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner2.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flights= new List<Flight>();
        private static int _id = 0;
        public static readonly object _lock = new object();

        public static Flight GetById(int id)
        {
            lock (_lock)
            {
                try
                {
                    return _flights.SingleOrDefault(flight => flight.Id == id);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static Airport[] GetAirportByKeyword(string keyword)
        {
            lock (_lock)
            {
                var cleanKeyword = keyword.Trim().ToUpper();
                var airports = _flights
                    .Select(a => a.From)
                    .Where(a => a.AirportName.ToUpper().StartsWith(cleanKeyword) ||
                                a.City.ToUpper().StartsWith(cleanKeyword) ||
                                a.Country.ToUpper().StartsWith(cleanKeyword));

                return airports.ToArray();
            }
        }

        public static void ClearFlights()
        {
            lock (_lock)
            {
                _flights.Clear();
                _id = 0;
            }
        }

        public static Flight AddFlight(Flight flight)
        {
            lock (_lock)
            {
                flight.Id = ++_id;
                _flights.Add(flight);
                return flight;
            }
        }

        public static bool IsDuplicate(Flight flight)
        {
            lock (_lock)
            {
                if (_flights.Count() == 0)
                {
                    return false;
                }

                var e = _flights.Last();
                //return _flights.ToArray().Last().Equals(flight);
                return _flights.Where(f => f.Equals(flight)).Count() > 0;
               
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

        public static void DeleteFlight(int id)
        {
            lock (_lock)
            {
                var flight = GetById(id);
                _flights.Remove(flight);
            }
        }

        public static SearchFlightResult SearchByParams(string from, string to, string date)
        {
            lock (_lock)
            {
                var filteredFlight = _flights.Where(f => f.From.AirportName == from || f.To.AirportName == to || f.DepartureTime == date).ToArray();

                return new SearchFlightResult(filteredFlight);
            }
        }
    }
}

