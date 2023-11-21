using fonebook.Data;
using fonebook.Dto;
using fonebook.Models;
using fonebook.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fonebook.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly FonebookAPIDbContext _dbContext;
        private readonly IContactsService _contactsService;

        public ContactsController(FonebookAPIDbContext dbContext, IContactsService contactsService)
        {
            _dbContext = dbContext;
            _contactsService = contactsService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await _dbContext.Contacts.ToListAsync());
        }

        [HttpGet("MyContacts")]
        public async Task<IActionResult> GetAllMyContacts()
        {
            try
            {
                var authenticatedUser = User.FindFirstValue("userId");
                Guid userId = Guid.Parse(authenticatedUser);

                var contactsList = await _contactsService.GetAllContactsByUserId(userId);
                return StatusCode(200, contactsList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddContact(AddContactDto contactToAdd)
        {
            try
            {
                var authenticatedUser = User.FindFirstValue("userId");
                Guid userId = Guid.Parse(authenticatedUser);

                var newContact = await _contactsService.CreateContact(userId, contactToAdd);
                return StatusCode(201, newContact);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{contactId:guid}")]
        public async Task<IActionResult> GetSingleContact([FromRoute] Guid contactId)
        {
            try
            {
                var contact = await _contactsService.GetContactById(contactId);

                var authenticatedUser = User.FindFirstValue("userId");

                if (contact.UserId == Guid.Parse(authenticatedUser))
                {
                    return StatusCode(200, contact);
                }
                else
                {
                    return Unauthorized("Unauthorized request");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("{contactId:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid contactId, UpdateContactDto contactToUpdate)
        {
            try
            {
                var existingContact = await _contactsService.UpdateContact(contactId, contactToUpdate);
                return StatusCode(200, existingContact);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{contactId:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid contactId)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact != null)
            {
                _dbContext.Remove(existingContact);
                _dbContext.SaveChanges();
                return Ok(existingContact);
            }

            return NotFound();


        }
    }
}
