namespace FlightPlanner2.Models
{
    public class Flight
    {
        public int? Id { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public bool IsValid()
        {
            return From != null &&
                   From.IsValid() &&
                   To != null &&
                   To.IsValid() &&
                   !From.Equals(To) &&
                   !string.IsNullOrEmpty(Carrier) &&
                   !string.IsNullOrEmpty(DepartureTime) &&
                   !string.IsNullOrEmpty(ArrivalTime);
        }

        public bool Equals(Flight otherFlight)
        {
            return Carrier.Equals(otherFlight.Carrier) &&
                   From.Equals(otherFlight.From) &&
                   To.Equals(otherFlight.To) &&
                   DepartureTime.Equals(otherFlight.DepartureTime) &&
                   ArrivalTime.Equals(otherFlight.ArrivalTime);
        }
    }
}
