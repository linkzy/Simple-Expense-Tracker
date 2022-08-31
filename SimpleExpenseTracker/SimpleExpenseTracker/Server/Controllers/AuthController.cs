using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Infra.Helpers;
using SimpleExpenseTracker.Shared;
using SimpleExpenseTracker.Shared.DTO;
using System.Text;

namespace SimpleExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SETContext _context;
        public AuthController(SETContext context)
        {
            this._context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(UserDTO request)
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
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return true;
        }

    }    
}
