using Ardalis.Specification;
using SATS.Data.Entities;

namespace SATS.Business.Specifications
{
    public class StudentsListSpec : Specification<Student>
    {
        public StudentsListSpec()
        {
            //Query.Include(x => x.StudentCourses);
            Query.AsNoTracking();
        }
    }
}
