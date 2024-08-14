using MediatR;
using SATS.Data.Entities;
namespace SATS.Business.Queries.Courses
{
    public class GetCourseListQuery : IRequest<List<Course>>
    {
    }
}
