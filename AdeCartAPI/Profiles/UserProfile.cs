using AdeCartAPI.Model;
using AdeCartAPI.UserModel;
using AutoMapper;

namespace AdeCartAPI.Profiles
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<SignUp, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(s => s.Password));

            CreateMap<Login, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(s => s.Password));

        }
    }
}
