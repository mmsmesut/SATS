using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SATS.Data.Entities;

namespace SATS.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses", "sats");

            builder.HasKey(x => x.CourseId);

            builder.Property(x => x.CourseId)
                  .HasColumnName("course_id");

            builder.Property(x => x.CourseName)
                   .HasColumnName("course_name")
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(x => x.CourseDescription)
                   .HasColumnName("course_description")
                  .HasMaxLength(500);

            //Base Entity
            builder.Property(x => x.CreaDate)
                  .HasColumnName("crea_date")
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                   .IsRequired();
        }
    }

}
