namespace SATS.Data.Entities
{
    public class Student : BaseEntity
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        //Navigation Property
        public ICollection<StudentCourse> StudentCourses { get; set; }

    }

}
