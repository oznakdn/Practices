using AutoMapper;
using JwtAuthentication_JsonResult.Entities;
using JwtAuthentication_JsonResult.Models.UserViewModels;

namespace JwtAuthentication_JsonResult.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserListViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()));

            CreateMap<UserCreateViewModel, User>();
            CreateMap<UserEditViewModel, User>().ReverseMap();
        }
    }
}
