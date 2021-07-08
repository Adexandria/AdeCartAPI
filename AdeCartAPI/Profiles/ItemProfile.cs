using AdeCartAPI.DTO;
using AdeCartAPI.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Profiles
{
    public class ItemProfile:Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDTO>()
            .ForMember(s => s.Name, opt => opt.MapFrom(s => s.ItemName))
            .ForMember(s => s.Price, opt => opt.MapFrom(s => s.ItemPrice))
            .ForMember(s => s.Description, opt => opt.MapFrom(s => s.ItemDescription));

            CreateMap<ItemCreate, Item>()
           .ForMember(s => s.ItemName, opt => opt.MapFrom(s => s.Name))
           .ForMember(s => s.ItemPrice, opt => opt.MapFrom(s => s.Price))
           .ForMember(s => s.ItemDescription, opt => opt.MapFrom(s => s.Description));

            CreateMap<ItemUpdate, Item>()
            .ForMember(s => s.ItemId, opt => {
                opt.Condition((src, dest, srcMember) => srcMember != 0); 
                opt.MapFrom(s => s.Id);
                
            })
           .ForMember(s => s.ItemName, opt => { 
               opt.Condition((src, dest, srcMember) => srcMember != null);
               opt.MapFrom(s => s.Name);
                
           })
           .ForMember(s => s.ItemPrice, opt =>
           {  
               opt.Condition((src, dest, srcMember) => srcMember != 0);
               opt.MapFrom(s => s.Price);
              
           })
            .ForMember(s => s.ItemDescription, opt => { 
                opt.Condition((src, dest, srcMember) => srcMember != null);
                opt.MapFrom(s => s.Description);
            });

          
        }
    }
}
