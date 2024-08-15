using MediatR;
using SATS.Business.DTOs;

namespace SATS.Business.Queries.Students
{
    public class GetStudentListWithPaginationQuery : IRequest<List<StudentDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


}
