using AutoMapper;
using BillingAPI.API.Payments.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.API.Payments.Handlers
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
            PaymentEntity? payment = await _dataContext.Payments.FindAsync(request.Id);
            if (payment == null)
                throw new NotFoundException("Payment not found");
            return _mapper.Map<PaymentDTO>(payment);
        }
    }
}
