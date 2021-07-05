using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AdeCartAPI.Model;

namespace AdeCartAPI.UserService
{
    public class IdentityDb : IdentityDbContext<User>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
