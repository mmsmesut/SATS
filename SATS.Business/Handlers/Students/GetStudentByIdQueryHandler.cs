using MediatR;
using SATS.Business.Queries.Students;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications.Students;
using SATS.Data.Entities;

namespace SATS.Business.Handlers.Students
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Student>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByIdQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new StudentByIdSpec(request.StudentId);
            var student = await _studentRepository.GetBySpecAsync(spec, cancellationToken);
            if (student == null)
            {
                //throw new NotFoundException(nameof(Student), request.StudentId);
            }

            return student;
        }
    }
}
