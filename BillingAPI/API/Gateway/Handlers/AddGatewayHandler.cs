using AutoMapper;
using BillingAPI.API.Gateway.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Gateway.Handlers
{
    public class AddGatewayHandler : IRequestHandler<AddGatewayCommand, GatewayDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AddGatewayHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<GatewayDTO> Handle(AddGatewayCommand request, CancellationToken cancellationToken)
        {
            await CheckGatewayNoIsUnique(request);
            GatewayEntity newGateway = await AddGateway(request);
            return _mapper.Map<GatewayDTO>(newGateway);
        }

        private async Task<GatewayEntity> AddGateway(AddGatewayCommand request)
        {
            GatewayEntity newGateway = new()
            {
                No = request.AddGatewayDTO.No
            };
            _dataContext.Add(newGateway);
            await _dataContext.SaveChangesAsync();
            return newGateway;
        }

        private async Task CheckGatewayNoIsUnique(AddGatewayCommand request)
        {
            if (await _dataContext.Gateways.AnyAsync(g => g.No.Trim() == request.AddGatewayDTO.No.Trim()))
                throw new BadRequestException("Gateway with the given gateway number already exists!");
        }
    }
}
