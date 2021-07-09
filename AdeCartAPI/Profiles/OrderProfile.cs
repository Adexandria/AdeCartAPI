using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Profiles
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderUpdate, Order>();
            CreateMap<OrderCreate, Order>();
        }
    }
}
