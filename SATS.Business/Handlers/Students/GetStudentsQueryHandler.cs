using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using SATS.Business.DTOs;
using SATS.Business.Queries.Students;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications.Students;
using System.IdentityModel.Tokens.Jwt;
namespace SATS.Business.Handlers.Students
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentListQuery, List<StudentDto>>
    {

        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor; // Inject IHttpContextAccessor


        public GetStudentsQueryHandler(
            IStudentRepository repository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor // Assign the IHttpContextAccessor
            )
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<StudentDto>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {

            // Access the current HttpContext
            var httpContext = _httpContextAccessor.HttpContext;

            // Check if HttpContext is available
            if (httpContext != null)
            {
                // Retrieve the claims from the current HTTP context
                var userClaims = httpContext.User.Claims;

                // Example: Extract specific claim values
                var userId = userClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                var userEmail = userClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

                // You can use these values as needed in your business logic
            }


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