using Ardalis.Specification;
using SATS.Data.Entities;

namespace SATS.Business.Specifications.Courses
{
    public class CoursesListSpec : Specification<Course>
    {
        public CoursesListSpec()
        {
            Query.AsNoTracking();
        }
    }
}
