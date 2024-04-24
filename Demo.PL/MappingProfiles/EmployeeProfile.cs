using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;

namespace Demo.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();

            // ReverseMap(); : 1->2 and 2->1

            // if name in 1st != name in 2nd->  
            //.ForMember(M => M.Name, o => o.MapFrom(src=> src.Name));
        }
    }
}
