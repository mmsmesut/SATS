using Ardalis.Specification;
using SATS.Data.Entities;

namespace SATS.Business.Specifications.Students
{
    internal class StudentsListWithPaginationSpec : Specification<Student>
    {
        public StudentsListWithPaginationSpec(int skip, int take)
        {
            Query.AsNoTracking()
                 .Skip(skip)
                 .Take(take);
        }
    }
}
