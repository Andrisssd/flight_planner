﻿using System.Text.Json.Serialization;

namespace FlightPlanner2.Models
{
    public class Airport
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        [JsonPropertyName("airport")]
        public string AirportName { get; set; }

        public bool Equals(Airport airportToCompare)
        {
            var originAirportCode = AirportName.Replace(" ", "").ToLower();
            var destinationAirportCode = airportToCompare.AirportName.Replace(" ", "").ToLower();
            return originAirportCode == destinationAirportCode;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Country) &&
                   !string.IsNullOrEmpty(City) &&
                   !string.IsNullOrEmpty(AirportName);
        }

        public override string ToString()
        {
            return $"country: {this.Country}\ncity: {this.City}\nairport name: {this.AirportName}";
        }
    }
}
