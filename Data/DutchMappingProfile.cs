using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchMappingProfile : Profile
    {
        public DutchMappingProfile()
        {
            // Compare both and match properties/types - works in both directions with ReverseMap
            CreateMap<Order, OrderViewModel>()
                // Need to specifically map order IDs
                .ForMember(order => order.OrderId, ex => ex.MapFrom(order => order.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap().ForMember(m => m.Product, opt => opt.Ignore());
        }
    }
}
