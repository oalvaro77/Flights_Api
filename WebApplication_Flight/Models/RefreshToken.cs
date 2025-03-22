namespace WebApplication_Flight.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }  // 🔹 Identificador único
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiredDate { get; set; }
        public bool IsRevoked { get; set; } = false;  // 🚀 Permite revocar un token
        public bool IsUsed { get; set; } = false;  // ✅ Evita reutilización
        public int UserId { get; set; }  // 🔗 Relación con usuario
        public User User { get; set; }  // 🔗 Propiedad de navegación
    }
}
