using MediatR;
using SATS.Business.Commands;
using SATS.Business.Repositories.Interfaces;
using SATS.Data.Entities;

namespace SATS.Business.Handlers
{

    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
    {
        private readonly IStudentRepository _studentRepository;

        public CreateStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Email = request.Email,
                City = request.City,
            };

            // Use the repository to add the student
            await _studentRepository.AddAsync(student, cancellationToken);

            return student.StudentId;
        }
    }
}
