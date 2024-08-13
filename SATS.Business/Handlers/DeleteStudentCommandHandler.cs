using MediatR;
using SATS.Business.Commands;
using SATS.Business.Repositories.Interfaces;

namespace SATS.Business.Handlers
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
    {
        private readonly IStudentRepository _studentRepository;

        public DeleteStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
            if (student == null)
            {
                //throw new NotFoundException(nameof(Student), request.StudentId);
            }

            await _studentRepository.DeleteAsync(student, cancellationToken);

            //return Unit.Value;
        }
    }
}
