using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.User;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.UserHandlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User? user = await _dataContext.Users
                .Include(u => u.Balances)
                .Include(u => u.Orders)
                .Include(u => u.Payments)
                .SingleOrDefaultAsync(u => u.Id == request.Id);
            if (user == null) throw new NotFoundException("User not found");
            return _mapper.Map<UserDTO>(user);
        }
    }
}
