using AutoMapper;
using BillingAPI.Data;
using BillingAPI.DTOs.User;
using BillingAPI.Entities;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.UserHandlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UpdateUserHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // check if user exists
            User? user = await _dataContext.Users
                .Include(u => u.Balances)
                .Include(u => u.Orders)
                .Include(u => u.Payments)
                .SingleOrDefaultAsync(u => u.Id == request.UpdateUserDTO.Id);
            if (user == null) throw new NotFoundException("User not found");
            // check if email is unique
            if (await _dataContext.Users.AnyAsync(u => u.Id != request.UpdateUserDTO.Id && u.Email == request.UpdateUserDTO.Email))
                throw new BadRequestException("Another user already exists with given email");
            User newUser = new()
            {
                Email = request.UpdateUserDTO.Email,
                Name = request.UpdateUserDTO.Name,
                Surname = request.UpdateUserDTO.Surname
            };
            _dataContext.Users.Add(newUser);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<UserDTO>(newUser);
        }
    }
}
