using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;

namespace Demo.PL.MappingProfiles
{
    public class Departmentprofile : Profile
    {
        public Departmentprofile()
        {
            CreateMap<DepartmentViewModel , Department>().ReverseMap();
        }
    }
}
