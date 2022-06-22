using BillingAPI.Data;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.Mediatr.Handlers.GatewayHandlers
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
            Gateway? gateway = _dataContext.Gateways.Find(request.id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            _dataContext.Gateways.Remove(gateway);
            await _dataContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
