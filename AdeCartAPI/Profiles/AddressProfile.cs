using AdeCartAPI.Model;
using AdeCartAPI.UserModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Profiles
{
    public class AddressProfile:Profile
    {
        public AddressProfile()
        {
            CreateMap<User, UserAddress>().ForMember(s => s.UserId, dest => dest.MapFrom(s => s.Id));
            CreateMap<UserAddress, AddressCreate>();
            CreateMap<UserAddress, AddressUpdate>();
        }
    }
}
