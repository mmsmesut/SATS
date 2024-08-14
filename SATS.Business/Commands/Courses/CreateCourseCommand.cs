
using MediatR;

namespace SATS.Business.Commands.Courses
{
    public class CreateCourseCommand : IRequest<int>
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
    }
}

