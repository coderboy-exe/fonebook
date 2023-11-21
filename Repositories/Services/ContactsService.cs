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

        public Task<Contact> DeleteContact(Guid contactId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await _dbContext.Contacts.ToListAsync();
            /*throw new NotImplementedException();*/
        }

        public async Task<Contact> GetContactById(Guid contactId)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contactId);

            if (existingContact == null)
            {
                throw new Exception("This contact does not exist");
            }
            return existingContact;
            /*throw new NotImplementedException();*/
        }

        public async Task<List<Contact>> GetAllContactsByUserId(Guid userId)
        {
            return await _dbContext.Contacts.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Contact> UpdateContact(Guid contactId, UpdateContactDto contactToUpdate)
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
                existingContact.UserId = contactToUpdate.UserId;

                await _dbContext.SaveChangesAsync();

                return existingContact;
            }
            /*throw new NotImplementedException();*/
        }
    }
}
