
using MediatR;

namespace SATS.Business.Commands.Courses
{
    public class DeleteCourseCommand : IRequest
    {
        public int CourseId { get; set; }
    }
}

