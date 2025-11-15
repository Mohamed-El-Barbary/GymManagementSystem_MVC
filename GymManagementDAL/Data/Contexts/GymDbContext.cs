using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Contexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(Eb =>
            {

                Eb.Property(x => x.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

                Eb.Property(x => x.LastName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

            });
        }

        #region DbSets

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<HealthRecord> HealthRecords { get; set; } = null!;
        public DbSet<Trainer> Trainers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Plan> Plans { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        #endregion

    }
}
