using MediatR;
using SATS.Business.Commands;
using SATS.Business.Repositories.Interfaces;

namespace SATS.Business.Handlers
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
    {
        private readonly IStudentRepository _studentRepository;

        public UpdateStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the student from the repository
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
            if (student == null)
            {
                //throw new NotFoundException(nameof(Student), request.StudentId);
            }

            // Update the student's properties
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.BirthDate = request.BirthDate;
            student.Email = request.Email;
            student.City = request.City;

            // Save the changes via the repository
            await _studentRepository.UpdateAsync(student, cancellationToken);

            //return Unit.Value;
        }
    }
}
