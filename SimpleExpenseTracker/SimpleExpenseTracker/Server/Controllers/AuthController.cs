using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Infra.Helpers;
using SimpleExpenseTracker.Shared.DTO.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SETContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SETContext context, IConfiguration configuration)
        {
            this._context = context;
            _configuration = configuration;
        }
            

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(UserRegistrationDTO request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest("User already registered.");

            byte[] password = Encoding.ASCII.GetBytes(request.Password);
            byte[] salt = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());

            User newUser = new User()
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = PasswordHelper.GenerateSaltedHash(password, salt),
                Salt = salt,
                UserAccount = new Account()
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(SignIn(newUser));
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
                return BadRequest("Invalid e-mail or password.");

            byte[] password = Encoding.ASCII.GetBytes(request.Password);
            var passowrdHash = PasswordHelper.GenerateSaltedHash(password, user.Salt);
            if (!PasswordHelper.CompareByteArrays(user.PasswordHash, passowrdHash))
                return BadRequest("Invalid e-mail or password.");

            return Ok(SignIn(user));
        }

        private string SignIn(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("UserAccountId", user.UserAccount.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:ApiKey").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }
    }    
}
