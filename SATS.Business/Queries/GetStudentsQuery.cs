using MediatR;
using SATS.Data.Entities;
namespace SATS.Business.Queries
{
    public class GetStudentListQuery : IRequest<List<Student>>
    {
        //Query'nin alcağı parametreler buraya yazılacak 
    }
}

/*
    RequestQuery : IRequest<Response>
 */
