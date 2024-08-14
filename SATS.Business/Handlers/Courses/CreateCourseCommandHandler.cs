using MediatR;
using SATS.Business.Commands.Courses;
using SATS.Business.Repositories.Interfaces;
using SATS.Data.Entities;

namespace SATS.Business.Handlers.Courses
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
    {
        private readonly ICourseRepository _courseRepository;

        public CreateCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                CourseName = request.CourseName,
                CourseDescription = request.CourseDescription
            };

            // Use the repository to add the course
            await _courseRepository.AddAsync(course, cancellationToken);

            return course.CourseId;
        }
    }
}


