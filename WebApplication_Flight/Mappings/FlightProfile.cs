using AutoMapper;
using WebApplication_Flight.Models;
using WebApplication_Flight.Models.DTOs;

namespace WebApplication_Flight.Mappings
{
    public class FlightProfile : Profile
    {
        public FlightProfile() {
            CreateMap<Flight, FlightDTO>();
            CreateMap<CreateFlightDTO, Flight>();
        }
    }
}
