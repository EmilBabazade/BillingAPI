using BillingAPI.API.Order.DTOs;
using MediatR;

namespace BillingAPI.API.Order
{
    public record AddOrderCommand(ProcessOrderDTO ProcessOrderDTO) : IRequest<ReceiptDTO>;
}