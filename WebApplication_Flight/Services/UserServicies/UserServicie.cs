using Microsoft.EntityFrameworkCore;
using WebApplication_Flight.Data;

namespace WebApplication_Flight.Services.UserServicies
{
    public class UserServicie : IUserServicie
    {
        private readonly FlightDbContext _context;

        public UserServicie(FlightDbContext context)
        {
            _context = context;
        }

        public User? GetUserByUsername(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.Username == userName);
        }

        public void RegisterUser(User user)
        {
            _context.Users.Add(user);
        }

        public void SavedChanges()
        {
            _context.SaveChanges();
        }
        
    }
}
