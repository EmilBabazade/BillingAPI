using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.API.Gateway.Handlers
{
    public class DeleteGatewayHandler : IRequestHandler<DeleteGatewayCommand, Unit>
    {
        private readonly DataContext _dataContext;

        public DeleteGatewayHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Unit> Handle(DeleteGatewayCommand request, CancellationToken cancellationToken)
        {
            GatewayEntity? gateway = CheckGatewayExists(request);
            await Remove(gateway);
            return Unit.Value;
        }

        private async Task Remove(GatewayEntity gateway)
        {
            _dataContext.Gateways.Remove(gateway);
            await _dataContext.SaveChangesAsync();
        }

        private GatewayEntity CheckGatewayExists(DeleteGatewayCommand request)
        {
            GatewayEntity? gateway = _dataContext.Gateways.Find(request.id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            return gateway;
        }
    }
}
