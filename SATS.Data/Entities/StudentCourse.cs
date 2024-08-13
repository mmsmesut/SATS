namespace SATS.Data.Entities
{
    public class StudentCourse : BaseEntity
    {
        public int StudentCourseId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public ICollection<Attendance> AttendanceRecords { get; set; }
    }

}
