using AutoMapper;
using BillingAPI.API.User.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.User.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, UserDTO>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AddUserHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // check if email is unique
            if (await _dataContext.Users.AnyAsync(u => u.Email == request.AddUserDTO.Email))
                throw new BadRequestException("Another user already exists with given email");
            UserEntity newUser = new()
            {
                Email = request.AddUserDTO.Email,
                Name = request.AddUserDTO.Name,
                Surname = request.AddUserDTO.Surname
            };
            _dataContext.Users.Add(newUser);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<UserDTO>(newUser);
        }
    }
}
