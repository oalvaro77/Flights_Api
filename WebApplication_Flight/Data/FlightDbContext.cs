using Microsoft.EntityFrameworkCore;
using WebApplication_Flight.Models;

namespace WebApplication_Flight.Data;

public class FlightDbContext : DbContext
{
    public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<User> Users { get; set; }
}
