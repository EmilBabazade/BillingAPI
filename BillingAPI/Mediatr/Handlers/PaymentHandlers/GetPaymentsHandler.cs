using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.Data;
using BillingAPI.DTOs.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.PaymentHandlers
{
    public class GetPaymentsHandler : IRequestHandler<GetPaymentsQuery, IEnumerable<PaymentDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetPaymentsHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PaymentDTO>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Entities.Payment>? query = _dataContext.Payments.AsQueryable();
            if (request.userId != null)
            {
                query = query.Where(p => p.UserId == request.userId);
            }
            if (request.successfull != null)
            {
                query = query.Where(p => p.IsSuccessfull == request.successfull);
            }
            if (request.order == "asc")
            {
                query = query.OrderBy(p => p.Id);
            }
            if (request.order == "desc")
            {
                query = query.OrderByDescending(p => p.Id);
            }
            return await query.ProjectTo<PaymentDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
