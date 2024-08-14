using MediatR;
using SATS.Business.Commands.Courses;
using SATS.Business.Repositories.Interfaces;

namespace SATS.Business.Handlers.Courses
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;

        public UpdateCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the course from the repository
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (course == null)
            {
                //throw new NotFoundException(nameof(Course), request.CourseId);
            }

            // Update the course's properties
            course.CourseName = request.CourseName;
            course.CourseDescription = request.CourseDescription;

            // Save the changes via the repository
            await _courseRepository.UpdateAsync(course, cancellationToken);

            //return Unit.Value;
        }
    }
}


