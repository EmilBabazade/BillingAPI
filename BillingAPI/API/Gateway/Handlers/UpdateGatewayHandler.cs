using AutoMapper;
using BillingAPI.API.Gateway.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Gateway.Handlers
{
    public class UpdateGatewayHandler : IRequestHandler<UpdateGatewayCommand, GatewayDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateGatewayHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<GatewayDTO> Handle(UpdateGatewayCommand request, CancellationToken cancellationToken)
        {
            GatewayEntity? gateway = await GetGateway(request);
            await CheckGatewayNoIsUnique(request);
            await UpdateGateway(request, gateway);
            return _mapper.Map<GatewayDTO>(gateway);
        }

        private async Task UpdateGateway(UpdateGatewayCommand request, GatewayEntity gateway)
        {
            gateway.No = request.UpdateGatewayDTO.No;
            _dataContext.Update(gateway);
            await _dataContext.SaveChangesAsync();
        }

        private async Task CheckGatewayNoIsUnique(UpdateGatewayCommand request)
        {
            if (await _dataContext.Gateways
                                .AnyAsync(g => g.No.Trim() == request.UpdateGatewayDTO.No.Trim() && g.Id != request.UpdateGatewayDTO.Id))
            {
                throw new BadRequestException("Gateway no already exists!");
            }
        }

        private async Task<GatewayEntity> GetGateway(UpdateGatewayCommand request)
        {
            GatewayEntity? gateway = await _dataContext.Gateways.FindAsync(request.UpdateGatewayDTO.Id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            return gateway;
        }
    }
}
