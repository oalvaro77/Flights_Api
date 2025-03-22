using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Flight.Models;
using WebApplication_Flight.Models.DTOs;
using WebApplication_Flight.Services;

namespace WebApplication_Flight.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class FlightController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet, Authorize]
    public ActionResult<List<FlightDTO>> GetAllFlights()
    {
        return Ok(_flightService.GetAllFlights());
    }

    [HttpGet("{id}")]
    
    public ActionResult<FlightDTO> GetFlightById(int id)
    {
        var result = _flightService.GetFlightById(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete]
    public ActionResult<string> DeleteFlight(int id)
    {
        var result = _flightService.DeleteFlight(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<FlightDTO> CreateFlight([FromBody] CreateFlightDTO flight)
    {
        return Ok(_flightService.CreateFlight(flight));
    }

    [HttpPut]
    public ActionResult<FlightDTO> UpdateFlight(int id,[FromBody] CreateFlightDTO flight)
    {
        var result = (_flightService.UpdateFlight(id, flight));

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
