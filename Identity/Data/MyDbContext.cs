using EnergySuite.SelfService.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnergySuite.SelfService.Identity.Data
{
    public class MyDbContext : IdentityDbContext<MyUser, MyRole, int>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

        }
    }
}
