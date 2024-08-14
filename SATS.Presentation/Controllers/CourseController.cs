using MediatR;
using Microsoft.AspNetCore.Mvc;
using SATS.Business.Commands.Courses;
using SATS.Business.Queries.Courses;

namespace SATS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : SatsBaseController
    {
        public CourseController(ISender mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseCommand command)
        {
            var courseId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCourseById), new { id = courseId }, courseId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseCommand command)
        {
            if (id != command.CourseId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _mediator.Send(new UpdateCourseCommand { CourseId = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery { CourseId = id });
            return Ok(course);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _mediator.Send(new GetCourseListQuery());
            return Ok(courses);
        }
    }
}
