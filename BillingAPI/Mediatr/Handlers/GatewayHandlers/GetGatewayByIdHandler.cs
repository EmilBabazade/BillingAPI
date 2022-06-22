using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.Data;
using BillingAPI.DTOs;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.GatewayHandlers
{
    public class GetGatewayByIdHandler : IRequestHandler<GetGatewayByIdQuery, GatewayDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetGatewayByIdHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<GatewayDTO> Handle(GetGatewayByIdQuery request, CancellationToken cancellationToken)
        {
            GatewayDTO? gateway = await _dataContext.Gateways.ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).SingleOrDefaultAsync(g => g.Id == request.id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            return gateway;
        }
    }
}
