using MediatR;

namespace SATS.Business.Commands
{
    public class DeleteStudentCommand : IRequest
    {
        public int StudentId { get; set; }
    }
}
