using MediatR;
using SATS.Data.Entities;
namespace SATS.Business.Queries
{
    public class GetStudentByIdQuery : IRequest<Student>
    {
        public int StudentId { get; set; }
    }
}

/*
    RequestQuery : IRequest<Response>
 */
