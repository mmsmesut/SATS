using Ardalis.Specification;
using SATS.Data.Entities;

namespace SATS.Business.Specifications.Courses
{
    public class CourseByIdSpec : Specification<Course>, ISingleResultSpecification
    {
        public CourseByIdSpec(int courseId)
        {
            Query.Where(x => x.CourseId == courseId)
                 .Include(x => x.StudentCourses);
        }
    }
}

