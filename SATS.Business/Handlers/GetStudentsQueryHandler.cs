using MediatR;
using SATS.Business.Queries;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications;
using SATS.Data.Entities;
namespace SATS.Business.Handlers
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentListQuery, List<Student>>
    {

        private readonly IStudentRepository _repository;

        public GetStudentsQueryHandler(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Student>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var spec = new StudentsListSpec();
            return await _repository.ListAsync(spec, cancellationToken);
        }
    }
}


//Handler 
/*
  handler : IRequestHandler<QueryRequest,Response>
  GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, List<Student>>
 
 */