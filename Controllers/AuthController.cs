using fonebook.Data;
using fonebook.Dto;
using fonebook.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fonebook.Controllers
{

    public class AuthController : Controller
    {
        private readonly AuthHelper _authHelper;
        private readonly FonebookAPIDbContext _dbContext;
        public AuthController(IConfiguration config, FonebookAPIDbContext dbContext)
        {
            _dbContext = dbContext;
            _authHelper = new AuthHelper(config);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserToLoginDto userToLogin)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userToLogin.Email);
            string token = "";

            if (existingUser == null)
            {
                return NotFound("Email does not exist");
            }
            else
            {
                if (existingUser.Password == userToLogin.Password) 
                {
                    token = _authHelper.CreateAuthToken(existingUser.Id);
                }
            }

            return Ok(new Dictionary<string, dynamic>
            {
                {"token", token },
                {"user", existingUser }
            });
        }
    }
}
