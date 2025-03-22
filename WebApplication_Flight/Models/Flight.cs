namespace WebApplication_Flight.Models;

public class Flight
{
    public int Id { get; set; }
    public int FlightNumber { get; set; }
    public string AirlineName { get; set; } = string.Empty;
    public string DepertureAirportCode { get; set; } = string.Empty;
    public string ArrivingAirportCode { get; set; } = string.Empty;
    public DateTime DepertureTime { get; set; }
    public DateTime ArrivingTime { get; set; }
    public int PassangerCapacity { get; set; }

    public string AircraftModel { get; set; }
}
