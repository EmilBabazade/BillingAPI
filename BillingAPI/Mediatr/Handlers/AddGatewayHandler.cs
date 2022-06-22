using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers
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
            // check if gateway no is unique
            if (await _dataContext.Gateways.AnyAsync(g => g.No.Trim() == request.AddGatewayDTO.No.Trim()))
                throw new BadRequestException("Gateway with the given gateway number already exists!");
            Gateway newGateway = new()
            {
                No = request.AddGatewayDTO.No
            };
            _dataContext.Add(newGateway);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<GatewayDTO>(newGateway);
        }
    }
}
