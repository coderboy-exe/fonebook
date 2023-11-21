using fonebook.Data;
using fonebook.Dto;
using fonebook.Models;
using fonebook.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace fonebook.Repositories.Services
{
    public class ContactsService : IContactsService
    {
        private readonly FonebookAPIDbContext _dbContext;

        public ContactsService(FonebookAPIDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public async Task<Contact> CreateContact(Guid userId, AddContactDto contactToCreateDto)
        {
            Contact newContact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = contactToCreateDto.Address,
                FirstName = contactToCreateDto.FirstName,
                LastName = contactToCreateDto.LastName,
                Email = contactToCreateDto.Email,
                Phone = contactToCreateDto.Phone,
                UserId = userId,
            };

            await _dbContext.Contacts.AddAsync(newContact);
            await _dbContext.SaveChangesAsync();

            return newContact;
            /*throw new NotImplementedException();*/
        }

        public async Task<Contact> DeleteContact(Guid userId, Guid contactId)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact == null)
            {
                throw new Exception("Contact not found");
            }

            if (existingContact.UserId != userId)
            {
                throw new Exception("Unauthorized: this contact doen not belong to this user");
            }
            else
            {
                _dbContext.Remove(existingContact);
                _dbContext.SaveChanges();

                return existingContact;
            }

        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await _dbContext.Contacts.ToListAsync();
            /*throw new NotImplementedException();*/
        }

        public async Task<Contact> GetContactById(Guid? userId, Guid contactId)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact == null)
            {
                throw new Exception("This contact does not exist");
            }
            if (userId != null)
            {
                if (existingContact.UserId != userId)
                {
                    throw new Exception("Incorrect source UserId");
                }
            }
            return existingContact;
            /*throw new NotImplementedException();*/
        }

        public async Task<List<Contact>> GetAllContactsByUserId(Guid userId)
        {
            return await _dbContext.Contacts.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Contact> UpdateContact(Guid userId, Guid contactId, UpdateContactDto contactToUpdate)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact == null)
            {
                throw new Exception("Contact does not exist");
            }
            else
            {
                existingContact.FirstName = contactToUpdate.FirstName;
                existingContact.LastName = contactToUpdate.LastName;
                existingContact.Email = contactToUpdate.Email;
                existingContact.Phone = contactToUpdate.Phone;
                existingContact.Address = contactToUpdate.Address;
                if (userId == existingContact.UserId)
                {
                    existingContact.UserId = userId;
                }
                else
                {
                    throw new Exception("Unauthorized: this contact does not belong to the user");
                }

                await _dbContext.SaveChangesAsync();

                return existingContact;
            }
            /*throw new NotImplementedException();*/
        }

        public async Task<Contact> ShareContact(Guid sourceUserId, Guid contactId, Guid destinationUserId)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact == null)
            {
                throw new Exception("Contact does not exist");
            }

            if (existingContact.UserId != sourceUserId)
            {
                throw new Exception("Unauthorized: the source user does not own this contact detail");
            }


            Contact sharedContact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = existingContact.Address,
                FirstName = existingContact.FirstName,
                LastName = existingContact.LastName,
                Email = existingContact.Email,
                Phone = existingContact.Phone,
                UserId = destinationUserId,
            };

            await _dbContext.Contacts.AddAsync(sharedContact);
            await _dbContext.SaveChangesAsync();

            return sharedContact;
            /*throw new NotImplementedException();*/
        }
    }
}
