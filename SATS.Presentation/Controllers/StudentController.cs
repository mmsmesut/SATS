using MediatR;
using Microsoft.AspNetCore.Mvc;
using SATS.Business.Commands.Students;
using SATS.Business.Queries.Students;

namespace SATS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : SatsBaseController
    {
        public StudentController(ISender mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentCommand command)
        {
            var studentId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetStudentById), new { id = studentId }, studentId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentCommand command)
        {
            if (id != command.StudentId)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _mediator.Send(new DeleteStudentCommand { StudentId = id });
            return NoContent();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _mediator.Send(new GetStudentByIdQuery { StudentId = id });
            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _mediator.Send(new GetStudentListQuery());
            return Ok(students);
        }

        [HttpGet]
        [Route("StudentsPaginations")]
        public async Task<IActionResult> GetStudents([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetStudentListWithPaginationQuery { PageNumber = 1, PageSize = 10 };
            var students = await _mediator.Send(query);
            return Ok(students);
        }
    }
}
