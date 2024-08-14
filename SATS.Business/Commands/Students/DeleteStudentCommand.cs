using MediatR;

namespace SATS.Business.Commands.Students
{
    public class DeleteStudentCommand : IRequest
    {
        public int StudentId { get; set; }
    }
}
