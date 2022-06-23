using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.Gateway;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.GatewayHandlers
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
            Gateway? gateway = await _dataContext.Gateways.FindAsync(request.UpdateGatewayDTO.Id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            if (await _dataContext.Gateways
                    .AnyAsync(g => g.No.Trim() == request.UpdateGatewayDTO.No.Trim() && g.Id != request.UpdateGatewayDTO.Id))
            {
                throw new BadRequestException("Gateway no already exists!");
            }
            gateway.No = request.UpdateGatewayDTO.No;
            _dataContext.Update(gateway);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<GatewayDTO>(gateway);
        }
    }
}
