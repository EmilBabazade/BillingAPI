﻿using AutoMapper;
using BillingAPI.API.User.DTOs;
using BillingAPI.Data;
using BillingAPI.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.User.Handlers
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
            UserEntity? user = await _dataContext.Users
                .Include(u => u.Balances)
                .Include(u => u.Orders)
                .Include(u => u.Payments)
                .SingleOrDefaultAsync(u => u.Id == request.UpdateUserDTO.Id);
            if (user == null) throw new NotFoundException("User not found");
            // check if email is unique
            if (await _dataContext.Users.AnyAsync(u => u.Id != request.UpdateUserDTO.Id && u.Email == request.UpdateUserDTO.Email))
                throw new BadRequestException("Another user already exists with given email");
            user.Email = request.UpdateUserDTO.Email;
            user.Name = request.UpdateUserDTO.Name;
            user.Surname = request.UpdateUserDTO.Surname;
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }
    }
}
