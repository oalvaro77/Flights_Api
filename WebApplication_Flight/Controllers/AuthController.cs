using Microsoft.AspNetCore.Mvc;
using WebApplication_Flight.Services;

namespace WebApplication_Flight.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {

        private readonly IAuthServicie _authServicie;

        public AuthController(IAuthServicie authServicie)
        {
            _authServicie = authServicie;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDTO request)
        {
            var user = new User { Username = request.Username };
            var result = await _authServicie.RegisterAsync(user, request.Password);

            if (result == null)
            {
                return BadRequest("User already exists");
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserDTO request)
        {
            //var user = await _authServicie.GetUserByUsernameAsync(request.Username);
            

            //if (user == null)
            //{
            //    return BadRequest("User not found");
            //}

            //if (!_authServicie.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            //{
            //    return Unauthorized("Wrong password");
            //}

            //string token = _authServicie.GenerateToken(user);
            //return Ok(token);
            var response = await _authServicie.LoginAsync(request.Username, request.Password, Response);

            if(response == null)
            {
                return Unauthorized("Invalid Username or password");
            }

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            
            var response = await _authServicie.RefreshTokenAsync(refreshToken, Response);

            if (response == null)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            return Ok(response);
        }
    }
}
