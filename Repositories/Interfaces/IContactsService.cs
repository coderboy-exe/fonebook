using fonebook.Dto;
using fonebook.Models;

namespace fonebook.Repositories.Interfaces
{
    public interface IContactsService
    {
        Task<Contact> CreateContact(Guid userId, AddContactDto contactToCreateDto);
        Task<List<Contact>> GetAllContacts();
        Task<List<Contact>> GetAllContactsByUserId(Guid userId);
        Task<Contact> GetContactById(Guid contactId);
        Task<Contact> UpdateContact(Guid contactId, UpdateContactDto contactToUpdateDto);
        Task<Contact> DeleteContact(Guid contactId);
    }
}
