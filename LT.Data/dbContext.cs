using LT.Model.Models;
using LT.Model.Models.UserCaseModels;
using Microsoft.EntityFrameworkCore;

namespace LT.Data
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleMenuMapping> RoleMenuMapping { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<Constitution> Constitution { get; set; }
        public DbSet<UserCourtCaseDetails> UserCourtCaseDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }
}
