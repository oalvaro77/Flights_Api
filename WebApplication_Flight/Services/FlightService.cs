using AutoMapper;
using WebApplication_Flight.Data;
using WebApplication_Flight.Models;
using WebApplication_Flight.Models.DTOs;

namespace WebApplication_Flight.Services;

public interface IFlightService
{
    List<FlightDTO> GetAllFlights();
    FlightDTO? GetFlightById(int id);
    FlightDTO CreateFlight(CreateFlightDTO flight);
    FlightDTO UpdateFlight(int id, CreateFlightDTO flight);
    string? DeleteFlight(int id);


}

public class FlightService : IFlightService
{
    private readonly FlightDbContext context;
    private readonly IMapper _mapper;

    public FlightService(FlightDbContext Context, IMapper mapper)
    {
       context = Context;
        _mapper = mapper;
    }

    public FlightDTO CreateFlight(CreateFlightDTO flightDTO)
    {
        var fligth = _mapper.Map<Flight>(flightDTO);
        context.Flights.Add(fligth);
        context.SaveChanges();
        return _mapper.Map<FlightDTO>(fligth);
    }

    public string? DeleteFlight(int id)
    {
        Flight savedFlight = context.Flights.Find(id);

        if (savedFlight == null)
        {
            return null;
        }

        context.Flights.Remove(savedFlight);
        return $"Successfully delete flight with id {id}";

    }

    public List<FlightDTO> GetAllFlights()
    {
        //return context.Flights.ToList();
        return context.Flights.Select(flight => _mapper.Map<FlightDTO>(flight)).ToList();
    }

    public FlightDTO GetFlightById(int id)
    {
        //    Flight savedFlight = context.Flights.Find(id);
        //    return savedFlight == null ? null : savedFlight;
        var flight = context.Flights.Find(id);
        return flight == null ? null : _mapper.Map<FlightDTO>(flight);
    }

    public FlightDTO UpdateFlight(int id, CreateFlightDTO flightDto)
    {
        var savedFlight = context.Flights.Find(id);
        if (savedFlight == null)
        {
            return null;
        }

        _mapper.Map(flightDto, savedFlight);
        //context.Entry(saved   Flight).CurrentValues.SetValues(flight);
        context.SaveChanges();
        return _mapper.Map<FlightDTO> (savedFlight);
    }
}
