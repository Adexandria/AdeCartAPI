using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Profiles
{
    public class OrderCartProfile :Profile
    {
        public OrderCartProfile()
        {
            CreateMap<OrderCartData, OrderCart>();
            CreateMap<OrderCart, OrderCartDTO>().ForMember(s=>s.OrderStatus,map=>map.MapFrom(s=>s.OrderStatus.ToString()));
            CreateMap<OrderCart, OrderCartsDTO>().ForMember(s => s.OrderStatus, map => map.MapFrom(s => s.OrderStatus.ToString()));
            CreateMap<CartUpdate, OrderCart>();
        }
    }
}
