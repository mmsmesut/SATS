namespace SATS.Data.Entities
{
    public class Course : BaseEntity
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }


        //Navigation Property
        public ICollection<StudentCourse> StudentCourses { get; set; }

    }

}
