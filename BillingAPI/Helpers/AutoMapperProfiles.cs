using AutoMapper;
using BillingAPI.DTOs.Balance;
using BillingAPI.DTOs.Gateway;
using BillingAPI.DTOs.Order;
using BillingAPI.DTOs.Payment;
using BillingAPI.DTOs.User;
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
            CreateMap<User, UserDTO>()
                .ForMember(
                    dest => dest.Balance,
                    opts => opts.MapFrom(
                        src => src.Balances.OrderByDescending(s => s.Id).FirstOrDefault())
                );
            CreateMap<Payment, PaymentDTO>();
        }
    }
}
