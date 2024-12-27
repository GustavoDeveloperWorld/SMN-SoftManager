using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftManager.Domain.Entities;
using SoftManager.Infrastructure.Persistence.Mappings;

namespace SoftManager.Infrastructure.Persistence.Contexts
{
    public class InfraDbContext : IdentityDbContext<ApplicationUser>
    {
        public InfraDbContext(DbContextOptions<InfraDbContext> options) : base(options) { }

        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserTaskMap());
            builder.ApplyConfiguration(new ApplicationUserMap());
        }
    }
}

