using System.Text.Json.Serialization;

namespace FlightPlanner2.Models
{
    public class Airport
    {
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
    }
}
