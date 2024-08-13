using Microsoft.EntityFrameworkCore;
using SATS.Data.Configurations;

namespace SATS.Data.Extensions
{
    public static class EntityConfigurationExtensions
    {

        public static void AddEntityConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new StudentCourseConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceConfiguration());


        }
    }
}
