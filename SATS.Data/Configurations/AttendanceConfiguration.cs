using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SATS.Data.Entities;

namespace SATS.Data.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("attendances", "sats"); // Tablo adı snake_case

            builder.HasKey(a => a.AttendanceId);

            builder.Property(a => a.AttendanceId)
                   .HasColumnName("attendance_id");

            builder.Property(a => a.StudentCourseId)
                   .HasColumnName("student_course_id");

            builder.HasOne(a => a.StudentCourse)
                .WithMany(sc => sc.AttendanceRecords)
                .HasForeignKey(a => a.StudentCourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}