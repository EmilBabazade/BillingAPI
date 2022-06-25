using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.Payment;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.Mediatr.Handlers.PaymentHandlers
{
    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, PaymentDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetPaymentHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<PaymentDTO> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            Payment? payment = await _dataContext.Payments.FindAsync(request.Id);
            if (payment == null)
                throw new NotFoundException("Payment not found");
            return _mapper.Map<PaymentDTO>(payment);
        }
    }
}
