using BillingAPI.API.Payments.DTOs;
using MediatR;

namespace BillingAPI.API.Payments
{
    public record GetPaymentsQuery(bool? successfull = null, int? userId = null, string? order = "") : IRequest<IEnumerable<PaymentDTO>>;
    public record GetPaymentQuery(int Id) : IRequest<PaymentDTO>;
}
