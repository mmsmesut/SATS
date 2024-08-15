﻿using AutoMapper;
using MediatR;
using SATS.Business.DTOs;
using SATS.Business.Queries.Students;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications.Students;
namespace SATS.Business.Handlers.Students
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentListQuery, List<StudentDto>>
    {

        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<StudentDto>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var spec = new StudentsListSpec();
            var students = await _repository.ListAsync(spec, cancellationToken);
            return _mapper.Map<List<StudentDto>>(students);

        }
    }
}


//Handler 
/*
  handler : IRequestHandler<QueryRequest,Response>
  GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, List<Student>>
 
 */