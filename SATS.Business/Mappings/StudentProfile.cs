using AutoMapper;
using SATS.Business.DTOs;
using SATS.Data.Entities;

namespace SATS.Business.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDto>();
        }
    }
}
