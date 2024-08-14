using MediatR;
using SATS.Data.Entities;
namespace SATS.Business.Queries.Students
{
    public class GetStudentListQuery : IRequest<List<Student>>
    {
    }
}
