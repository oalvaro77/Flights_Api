namespace WebApplication_Flight.Services
{
    public interface IAuthServicie
    {
        Task<User?> RegisterAsync (User user, string password);
        Task<object> LoginAsync (string username, string password, HttpResponse response);
        string GenerateToken (User user);
        Task<User?>GetUserByUsernameAsync(string username);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        Task<object> RefreshTokenAsync(string refreshToken, HttpResponse response);
    }
}
