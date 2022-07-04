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
            return request.Order switch
            {
                "asc" => await OrderByAcending(),
                "desc" => await OrderByDescending(),
                _ => await GetGatewaysWithoutOrdering()
            };
        }

        private async Task<List<GatewayDTO>> GetGatewaysWithoutOrdering()
        {
            return await _dataContext.Gateways.ProjectTo<GatewayDTO>(
                                    _mapper.ConfigurationProvider
                                ).ToListAsync();
        }

        private async Task<List<GatewayDTO>> OrderByDescending()
        {
            return await _dataContext.Gateways.OrderByDescending(b => b.Id).ProjectTo<GatewayDTO>(
                                    _mapper.ConfigurationProvider
                                ).ToListAsync();
        }

        private async Task<List<GatewayDTO>> OrderByAcending()
        {
            return await _dataContext.Gateways.OrderBy(b => b.Id).ProjectTo<GatewayDTO>(
                                    _mapper.ConfigurationProvider
                                ).ToListAsync();
        }
    }
}
