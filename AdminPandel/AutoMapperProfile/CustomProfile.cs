using AdminPandel.Models;
using AdminPandel.ViewModels;
using AutoMapper;

namespace AdminPandel.AutoMapperProfile
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<RegisterVm, ApplicationUser>()
                .ForMember(m=>m.FristName, 
                          d => d.MapFrom(s=> s.FirstName))
                  .ForMember(m => m.UserName,
                          d => d.MapFrom(s => s.Email))
                .ReverseMap();

        
        }
    }
}
