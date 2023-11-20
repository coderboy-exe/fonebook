using fonebook.Data;
using fonebook.Dto;
using fonebook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fonebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly FonebookAPIDbContext dbContext;

        public ContactsController(FonebookAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpPost("")]
        public async Task<IActionResult> AddContact(AddContactDto contactToAdd)
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = contactToAdd.Address,
                FirstName = contactToAdd.FirstName,
                LastName = contactToAdd.LastName,
                Email = contactToAdd.Email,
                Phone = contactToAdd.Phone,
                UserId = contactToAdd.UserId,
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSingleContact([FromRoute] Guid id)
        {
            var existingContact = await dbContext.Contacts.FindAsync(id);

            if (existingContact == null)
            {
                return NotFound();
            }

            return Ok(existingContact);

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactDto contactToUpdate)
        {
            var existingContact = await dbContext.Contacts.FindAsync(id);

            if (existingContact == null) {
                return NotFound();
            }
            else
            {
                existingContact.FirstName = contactToUpdate.FirstName;
                existingContact.LastName = contactToUpdate.LastName;
                existingContact.Email = contactToUpdate.Email;
                existingContact.Phone = contactToUpdate.Phone;
                existingContact.Address = contactToUpdate.Address;
                existingContact.UserId = contactToUpdate.UserId;

                await dbContext.SaveChangesAsync();
                return Ok(existingContact);
            }


        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var existingContact = await dbContext.Contacts.FindAsync(id);

            if (existingContact != null)
            {
                dbContext.Remove(existingContact);
                dbContext.SaveChanges();
                return Ok(existingContact);
            }

            return NotFound();


        }
    }
}
