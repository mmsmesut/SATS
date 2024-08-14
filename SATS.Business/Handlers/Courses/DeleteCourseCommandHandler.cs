using MediatR;
using SATS.Business.Commands.Courses;
using SATS.Business.Repositories.Interfaces;

namespace SATS.Business.Handlers.Courses
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;

        public DeleteCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (course == null)
            {
                //throw new NotFoundException(nameof(Course), request.CourseId);
            }

            await _courseRepository.DeleteAsync(course, cancellationToken);

            //return Unit.Value;
        }
    }
}
