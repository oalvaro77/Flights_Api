namespace WebApplication_Flight.Services
{
    public interface IAuthServicie
    {
        Task<User?> RegisterAsync (User user, string password);
        string Login (string username, string password);
        string GenerateToken (User user);
        Task<User?>GetUserByUsernameAsync(string username);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
