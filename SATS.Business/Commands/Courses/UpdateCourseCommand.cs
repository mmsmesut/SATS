
using MediatR;

namespace SATS.Business.Commands.Courses
{
    public class UpdateCourseCommand : IRequest
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
    }
}
