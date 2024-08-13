namespace SATS.Data.Entities
{
    public class Attendance : BaseEntity
    {
        public int AttendanceId { get; set; }
        public int StudentCourseId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }

        public StudentCourse StudentCourse { get; set; }
    }


}
