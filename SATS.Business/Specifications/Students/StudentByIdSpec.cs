using Ardalis.Specification;
using SATS.Data.Entities;

namespace SATS.Business.Specifications.Students
{
    public class StudentByIdSpec : Specification<Student>, ISingleResultSpecification
    {
        public StudentByIdSpec(int studentId)
        {
            Query.Where(x => x.StudentId == studentId)
                 .Include(x => x.StudentCourses);
        }
    }
}
