using MediatR;
using SATS.Business.Queries.Courses;
using SATS.Business.Repositories.Interfaces;
using SATS.Business.Specifications.Courses;
using SATS.Data.Entities;

namespace SATS.Business.Handlers.Courses
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Course>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new CourseByIdSpec(request.CourseId);
            var course = await _courseRepository.GetBySpecAsync(spec, cancellationToken);
            if (course == null)
            {
                //throw new NotFoundException(nameof(Course), request.CourseId);
            }

            return course;
        }
    }
}


