using Azure;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication_Flight.Data;
using WebApplication_Flight.Models;

namespace WebApplication_Flight.Services
{
    public class AuthServicies : IAuthServicie
    {
        public readonly FlightDbContext _context;
        public readonly IConfiguration _configuration;

        public AuthServicies(FlightDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<object> LoginAsync(string username, string password, HttpResponse response)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            if (!computeHash.SequenceEqual(user.PasswordHash))
            {
                return null;
            }

            var token = GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.ExpiredDate,
                Secure = true,
                SameSite = SameSiteMode.None
            });



            return new {Token  = token, RefreshToken = refreshToken.Token};
        }

        public async Task<User?> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return null;
            }

            using var hmac = new HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            user.Role = user.Role ?? "User";

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;

            }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<object> RefreshTokenAsync(string refreshToken, HttpResponse response)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
                    
            if (user == null)
            {
                return null;
            }

            var validToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

            if (validToken == null || validToken.ExpiredDate < DateTime.UtcNow)
            {
                return null;
            }

            var newToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            await SetRefreshTokenAsync(newRefreshToken, user, response);

            
            return new { Token = newToken, refreshToken = newRefreshToken.Token };
                

        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                ExpiredDate = DateTime.Now.AddDays(7),
                CreatedDate = DateTime.Now
            };
        }

        private async Task SetRefreshTokenAsync(RefreshToken refreshToken, User user, HttpResponse response)
        {
            user.RefreshTokens.RemoveAll(rt => rt.ExpiredDate < DateTime.UtcNow);

            user.RefreshTokens.Add(refreshToken);
           
            await _context.SaveChangesAsync();

            response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true, // Evita accesos desde JavaScript (más seguro)
                Expires = refreshToken.ExpiredDate, // Expira en la fecha establecida
                Secure = true, // Solo funciona en HTTPS
                SameSite = SameSiteMode.None // Permite compartir cookies entre dominios
            });






        }
    }
     
}
