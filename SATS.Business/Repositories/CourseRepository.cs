using Ardalis.Specification.EntityFrameworkCore;
using SATS.Business.Repositories.Interfaces;
using SATS.Data;
using SATS.Data.Entities;


namespace SATS.Business.Repositories
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(SATSAppDbContext dbContext) : base(dbContext)
        {
        }
    }

}
