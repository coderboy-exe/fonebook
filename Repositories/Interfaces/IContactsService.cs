using fonebook.Dto;
using fonebook.Models;

namespace fonebook.Repositories.Interfaces
{
    public interface IContactsService
    {
        Task<Contact> CreateContact(Guid userId, AddContactDto contactToCreateDto);
        Task<List<Contact>> GetAllContacts();
        Task<List<Contact>> GetAllContactsByUserId(Guid userId);
        Task<Contact> GetContactById(Guid? userId, Guid contactId);
        Task<Contact> UpdateContact(Guid userId, Guid contactId, UpdateContactDto contactToUpdateDto);
        Task<Contact> ShareContact(Guid sourceUserId, Guid contactId, Guid destinationUserId);
        Task<Contact> DeleteContact(Guid userId, Guid contactId);
    }
}
