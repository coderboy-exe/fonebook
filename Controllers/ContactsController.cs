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

        [HttpGet("MyContacts/{contactId:guid}")]
        public async Task<IActionResult> GetMyContactSingle([FromRoute] Guid contactId)
        {
            try
            {
                var authenticatedUser = User.FindFirstValue("userId");
                Guid parsedUserId = Guid.Parse(authenticatedUser);

                var contact = await _contactsService.GetContactById(userId: parsedUserId, contactId);
                return StatusCode(200, contact);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("MyContacts/{contactId:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid contactId, UpdateContactDto contactToUpdate)
        {
            try
            {
                var authenticatedUser = User.FindFirstValue("userId");
                Guid parsedUserId = Guid.Parse(authenticatedUser);

                var updatedContact = await _contactsService.UpdateContact(userId: parsedUserId, contactId, contactToUpdate);
                return StatusCode(200, updatedContact);

            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [HttpDelete("MyContacts/{contactId:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid contactId)
        {
            try
            {
                var authenticatedUser = User.FindFirstValue("userId");
                Guid parsedUserId = Guid.Parse(authenticatedUser);

                var deletedContact = await _contactsService.DeleteContact(userId: parsedUserId, contactId);
                return StatusCode(200, deletedContact);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("MyContacts/Share")]
        public async Task<IActionResult> ShareContact(ShareContact shareContact)
        {
            try
            {
                var authenticatedUser = User.FindFirstValue("userId");
                Guid parsedUserId = Guid.Parse(authenticatedUser);

                var sharedContact = await _contactsService.ShareContact(sourceUserId: parsedUserId, contactId: shareContact.contactId, destinationUserId: shareContact.destinationUserId);
                return StatusCode(200, sharedContact);

            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
