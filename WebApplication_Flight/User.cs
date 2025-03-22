namespace WebApplication_Flight
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; } 
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiredToken { get; set; }
        
    }
}
