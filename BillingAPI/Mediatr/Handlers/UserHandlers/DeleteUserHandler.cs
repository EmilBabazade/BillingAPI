using BillingAPI.Data;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;

namespace BillingAPI.Mediatr.Handlers.UserHandlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly DataContext _dataContext;

        public DeleteUserHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _dataContext.Users.FindAsync(request.Id);
            if (user == null) throw new NotFoundException("User not found");
            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
