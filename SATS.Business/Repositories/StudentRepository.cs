using Ardalis.Specification.EntityFrameworkCore;
using SATS.Business.Repositories.Interfaces;
using SATS.Data;
using SATS.Data.Entities;


namespace SATS.Business.Repositories
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(SATSAppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
