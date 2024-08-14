using MediatR;
using SATS.Data.Entities;
namespace SATS.Business.Queries.Courses
{
    public class GetCourseByIdQuery : IRequest<Course>
    {
        public int CourseId { get; set; }
    }
}
