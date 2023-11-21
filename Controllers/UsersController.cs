using fonebook.Data;
using fonebook.Dto;
using fonebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fonebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly FonebookAPIDbContext _dbContext;

        public UsersController(FonebookAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _dbContext.Users.ToListAsync());
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDto userToAdd)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Email == userToAdd.Email
            );

            if (existingUser != null) {
                throw new Exception("A user with this email already exists");
            }

            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = userToAdd.FirstName,
                LastName = userToAdd.LastName,
                Email = userToAdd.Email,
                Password = userToAdd.Password,
                Phone = userToAdd.Phone
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetSingleUser([FromRoute] Guid userId)
        {
            var existingUser = await _dbContext.Users.FindAsync(userId);
            if (existingUser != null)
            {
                return Ok(existingUser);
            }
            return NotFound();
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, AddUserDto userToUpdate)
        {
            var existingUser = await _dbContext.Users.FindAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            else
            {
                existingUser.FirstName = userToUpdate.FirstName;
                existingUser.LastName = userToUpdate.LastName;
                existingUser.Email = userToUpdate.Email;
                existingUser.Password = userToUpdate.Password;
                existingUser.Phone = userToUpdate.Phone;

                await _dbContext.SaveChangesAsync();
                return Ok(existingUser);
            }
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            var existingUser = await _dbContext.Users.FindAsync(userId);
            if (existingUser != null)
            {
                _dbContext.Remove(existingUser);
                _dbContext.SaveChanges();
                return Ok(existingUser);
            }

            return NotFound();
        }
    }
}
