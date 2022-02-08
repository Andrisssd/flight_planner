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
        private static readonly object obj = new object();

        public static Flight GetById(int id)
        {
            lock (obj)
            {
                return _flights.SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static Airport[] GetAirportByKeyword(string keyword)
        {
            lock (obj)
            {
                var cleanKeyword = keyword.Replace(" ", "").ToUpper();
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
            lock (obj)
            {
                _flights.Clear();
            }
        }

        public static Flight AddFlight(Flight flight)
        {
            lock (obj)
            {
                flight.Id = ++_id;
                _flights.Add(flight);
                return flight;
            }
        }

        public static bool IsDuplicate(Flight flight)
        {
            lock (obj)
            {
                if (_flights.Count() == 0)
                {
                    return false;
                }

                return _flights.ToArray().Last().Equals(flight);
            }
        }

        public static bool IsValid(Flight flight)
        {
            lock (obj)
            {
                return flight != null && flight.IsValid();
            }
        }

        public static bool IsValidTimeframe(Flight flight)
        {
            lock (obj)
            {
                var departureTime = DateTime.Parse(flight.DepartureTime);
                var arrivalTime = DateTime.Parse(flight.ArrivalTime);

                return departureTime < arrivalTime;
            }
        }

        public static void DeleteFlight(int id)
        {
            lock (obj)
            {
                var flight = GetById(id);
                _flights.Remove(flight);
            }
        }

        public static SearchFlightResult SearchByParams(string from, string to, string date)
        {
            lock (obj)
            {
                var filteredFlight = _flights.Where(f => f.From.AirportName == from || f.To.AirportName == to || f.DepartureTime == date).ToArray();

                return new SearchFlightResult(filteredFlight);
            }
        }
    }
}

