using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SATS.Data.Entities;
using SATS.Data.Extensions;

namespace SATS.Data
{
    public class SATSAppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public SATSAppDbContext() { }
        public SATSAppDbContext(DbContextOptions<SATSAppDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity için gerekli yapılandırmaları sağlar
            modelBuilder.AddEntityConfiguration();
            modelBuilder.CreateSeedData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Database=localdb;Username=postgres;Password=mms;Search Path=sats");
        }
    }
}

/*
   XOptions<T>
 */


