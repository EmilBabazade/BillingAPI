using AutoMapper;
using BillingAPI.API.Balance;
using BillingAPI.API.Balance.DTOs;
using BillingAPI.API.Gateway;
using BillingAPI.API.Gateway.DTOs;
using BillingAPI.API.Order;
using BillingAPI.API.Order.DTOs;
using BillingAPI.API.Payments;
using BillingAPI.API.Payments.DTOs;
using BillingAPI.API.User;
using BillingAPI.API.User.DTOs;

namespace BillingAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BalanceEntity, BalanceDTO>();
            CreateMap<GatewayEntity, GatewayDTO>();
            CreateMap<OrderEntity, OrderDTO>();
            CreateMap<UserEntity, UserDTO>()
                .ForMember(
                    dest => dest.Balance,
                    opts => opts.MapFrom(
                        src => src.Balances.OrderByDescending(s => s.Id).FirstOrDefault())
                );
            CreateMap<PaymentEntity, PaymentDTO>();
        }
    }
}
