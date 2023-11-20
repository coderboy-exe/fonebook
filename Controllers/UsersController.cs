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
        private readonly FonebookAPIDbContext dbContext;

        public UsersController(FonebookAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await dbContext.Users.ToListAsync());
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(AddUserDto userToAdd)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u =>
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

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSingleUser([FromRoute] Guid id)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser != null)
            {
                return Ok(existingUser);
            }
            return NotFound();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, AddUserDto userToUpdate)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
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

                await dbContext.SaveChangesAsync();
                return Ok(existingUser);
            }
        }

        [HttpPost("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var existingUser = await dbContext.Users.FindAsync(id);
            if (existingUser != null)
            {
                dbContext.Remove(existingUser);
                dbContext.SaveChanges();
                return Ok(existingUser);
            }

            return NotFound();
        }
    }
}
