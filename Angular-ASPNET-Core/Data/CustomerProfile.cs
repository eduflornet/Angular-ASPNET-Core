
using Angular_ASPNET_Core.Data.Entities;
using Angular_ASPNET_Core.Models;
using AutoMapper;

namespace Angular_ASPNET_Core.Data
{
    public class CustomerProfile: Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<State, StateModel>().ReverseMap();
        }
    }
}
