using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MyappAPI.Models
{
    public class UserContext : DbContext

    {
        public UserContext(DbContextOptions options) : base(options){ }
        public DbSet<User> Users { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
    }
}
