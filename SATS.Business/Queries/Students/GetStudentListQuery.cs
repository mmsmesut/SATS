using MediatR;
using SATS.Business.DTOs;
namespace SATS.Business.Queries.Students
{
    public class GetStudentListQuery : IRequest<List<StudentDto>>
    {
    }
}
