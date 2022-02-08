namespace FlightPlanner2.Models
{
    public class AddFlightRequest
    {
        public Airport From { get; set; }
        public Airport To { get; set; }
        public string Carrier { get; set; }
        public string Departuretime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
