using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.API.Gateway.DTOs;
using BillingAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Gateway.Handlers
{
    public class GetGatewaysHandler : IRequestHandler<GetGatewaysQuery, IEnumerable<GatewayDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetGatewaysHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GatewayDTO>> Handle(GetGatewaysQuery request, CancellationToken cancellationToken)
        {
            if (request.Order == "asc")
            {
                return await _dataContext.Gateways.OrderBy(b => b.Id).ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
            }
            if (request.Order == "desc")
            {
                return await _dataContext.Gateways.OrderByDescending(b => b.Id).ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
            }
            return await _dataContext.Gateways.ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
        }
    }
}
