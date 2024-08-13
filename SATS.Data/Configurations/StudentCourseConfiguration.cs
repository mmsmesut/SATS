using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SATS.Data.Entities;

namespace SATS.Data.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.ToTable("student_courses", "sats"); // Table adı snake_case

            builder.HasKey(sc => sc.StudentCourseId);

            builder.Property(sc => sc.StudentCourseId)
                   .HasColumnName("student_course_id");

            builder.Property(sc => sc.StudentId)
                   .HasColumnName("student_id");

            builder.Property(sc => sc.CourseId)
                   .HasColumnName("course_id");

            builder.HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

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
