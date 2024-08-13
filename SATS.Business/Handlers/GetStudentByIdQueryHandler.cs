using MediatR;
using SATS.Business.Queries;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications;
using SATS.Data.Entities;

namespace SATS.Business.Handlers
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
