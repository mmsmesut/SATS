using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SATS.Business.Commands.Students;
using SATS.Business.Queries.Students;
using SATS.Presentation.Controllers;
namespace SATS.UnitTest.Controllers
{
    [TestFixture]
    public class StudentControllerTests
    {
        private Mock<ISender> _mediatorMock;
        private StudentController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<ISender>();
            _controller = new StudentController(_mediatorMock.Object);
        }

        [Test]
        public async Task CreateStudent_ReturnsCreatedAtActionResult_WithStudentId()
        {
            // Arrange
            var command = new CreateStudentCommand { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
            var expectedStudentId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateStudentCommand>(), default))
                         .ReturnsAsync(expectedStudentId);

            // Act
            var result = await _controller.CreateStudent(command);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;

            var actualStudentId = createdResult?.Value as int?;
            Assert.That(actualStudentId, Is.EqualTo(expectedStudentId));
        }

        [Test]
        public async Task UpdateStudent_ReturnsNoContent_WhenStudentIsUpdated()
        {
            // Arrange
            var command = new UpdateStudentCommand
            {
                StudentId = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Parse("2000-01-01"),
                Email = "john@example.com",
                City = "City"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateStudentCommand>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(Unit.Value));

            // Act
            var result = await _controller.UpdateStudent(1, command);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }


        [Test]
        public async Task UpdateStudent_ReturnsBadRequest_WhenIdDoesNotMatchStudentId()
        {
            // Arrange
            var command = new UpdateStudentCommand
            {
                StudentId = 2,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Parse("2000-01-01"),
                Email = "john@example.com",
                City = "City"
            };

            // Act
            var result = await _controller.UpdateStudent(1, command);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task DeleteStudent_ReturnsNoContent_WhenStudentIsDeleted()
        {
            // Arrange
            var studentId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateStudentCommand>(), It.IsAny<CancellationToken>()))
             .Returns(Task.FromResult(Unit.Value));

            // Act
            var result = await _controller.DeleteStudent(studentId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetStudentById_ReturnsOkResult_WithStudent()
        {
            // Arrange
            var studentId = 1;
            var student = new Data.Entities.Student
            {
                StudentId = studentId,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Parse("2000-01-01"),
                Email = "john@example.com",
                City = "City"
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), default))
                         .ReturnsAsync(student);

            // Act
            var result = await _controller.GetStudentById(studentId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(student));
        }

        //[Test]
        //public async Task GetStudents_ReturnsOkResult_WithListOfStudents()
        //{
        //    // Arrange
        //    var students = new List<Data.Entities.Student>
        //{
        //    new Data.Entities.Student
        //    {
        //        StudentId = 1,
        //        FirstName = "John",
        //        LastName = "Doe",
        //        BirthDate = DateTime.Parse("2000-01-01"),
        //        Email = "john@example.com",
        //        City = "City"
        //    },
        //    new Data.Entities.Student
        //    {
        //        StudentId = 2,
        //        FirstName = "Jane",
        //        LastName = "Doe",
        //        BirthDate = DateTime.Parse("2000-01-02"),
        //        Email = "jane@example.com",
        //        City = "City"
        //    }
        //       };
        //    _mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentListQuery>(), default))
        //                 .ReturnsAsync(students);

        //    // Act
        //    var result = await _controller.GetStudents();

        //    // Assert
        //    Assert.That(result, Is.InstanceOf<OkObjectResult>());
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult?.Value, Is.EqualTo(students));
        //}
    }
}
