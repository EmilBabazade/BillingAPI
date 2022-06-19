using AutoMapper;
using BillingAPI.DTOs;
using BillingAPI.Entities;

namespace BillingAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Balance, BalanceDTO>();
            CreateMap<Gateway, GatewayDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Payment, PaymentDTO>();
        }
    }
}
