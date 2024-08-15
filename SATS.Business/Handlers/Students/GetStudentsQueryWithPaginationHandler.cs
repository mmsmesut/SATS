using AutoMapper;
using MediatR;
using SATS.Business.DTOs;
using SATS.Business.Queries.Students;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications.Students;

namespace SATS.Business.Handlers.Students
{
    internal class GetStudentsQueryWithPaginationHandler : IRequestHandler<GetStudentListWithPaginationQuery, List<StudentDto>>
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public GetStudentsQueryWithPaginationHandler(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<StudentDto>> Handle(GetStudentListWithPaginationQuery request, CancellationToken cancellationToken)
        {
            int skip = (request.PageNumber - 1) * request.PageSize;
            var spec = new StudentsListWithPaginationSpec(skip, request.PageSize);
            var students = await _repository.ListAsync(spec, cancellationToken);
            return _mapper.Map<List<StudentDto>>(students);
        }
    }
}
