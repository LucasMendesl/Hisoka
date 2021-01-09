using HisokaExample.Model;
using Microsoft.EntityFrameworkCore;

namespace HisokaExample.Infrastructure.Context
{
    public class HisokaContext : DbContext
    {
        public HisokaContext(DbContextOptions<HisokaContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }           
    }
}