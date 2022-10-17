using AutoMapper;
using DefaultCrud_AjaxCrud_ImageUpload.Entities;
using DefaultCrud_AjaxCrud_ImageUpload.Models.UserViewModels;

namespace DefaultCrud_AjaxCrud_ImageUpload.MappingProfiles
{
    public class UserProfile:Profile
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
