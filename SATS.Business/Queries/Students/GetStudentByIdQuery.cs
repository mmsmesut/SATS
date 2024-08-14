using MediatR;
using SATS.Data.Entities;
namespace SATS.Business.Queries.Students
{
    public class GetStudentByIdQuery : IRequest<Student>
    {
        public int StudentId { get; set; }
    }
}
