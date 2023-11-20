using fonebook.Models;
using Microsoft.EntityFrameworkCore;

namespace fonebook.Data
{
    public class FonebookAPIDbContext : DbContext
    {
        public FonebookAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
