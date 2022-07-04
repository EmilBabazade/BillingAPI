using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.API.Payments.DTOs;
using BillingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Payments.Handlers
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
            IQueryable<PaymentEntity>? query = _dataContext.Payments.AsQueryable();
            query = FitlerByUserId(request, query);
            query = GetSuccessfull(request, query);
            query = OrderByAscending(request, query);
            query = OrderByDescending(request, query);
            return await query.ProjectTo<PaymentDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        private static IQueryable<PaymentEntity> OrderByDescending(GetPaymentsQuery request, IQueryable<PaymentEntity> query)
        {
            if (request.order == "desc")
                query = query.OrderByDescending(p => p.Id);
            return query;
        }

        private static IQueryable<PaymentEntity> OrderByAscending(GetPaymentsQuery request, IQueryable<PaymentEntity> query)
        {
            if (request.order == "asc")
                query = query.OrderBy(p => p.Id);
            return query;
        }

        private static IQueryable<PaymentEntity> GetSuccessfull(GetPaymentsQuery request, IQueryable<PaymentEntity> query)
        {
            if (request.successfull != null)
                query = query.Where(p => p.IsSuccessfull == request.successfull);
            return query;
        }

        private static IQueryable<PaymentEntity> FitlerByUserId(GetPaymentsQuery request, IQueryable<PaymentEntity> query)
        {
            if (request.userId != null)
                query = query.Where(p => p.UserId == request.userId);
            return query;
        }
    }
}
