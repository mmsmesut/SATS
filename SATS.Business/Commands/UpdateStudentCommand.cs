using MediatR;

namespace SATS.Business.Commands
{
    public class UpdateStudentCommand : IRequest
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
    }

}
