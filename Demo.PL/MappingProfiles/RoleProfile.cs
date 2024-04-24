using AutoMapper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.MappingProfiles
{
    public class RoleProfile  :Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>()
                .ForMember(M => M.Name, o => o.MapFrom(src => src.RoleName))
                .ReverseMap();
        }
    }
}
