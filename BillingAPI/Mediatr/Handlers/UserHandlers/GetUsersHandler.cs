﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.Data;
using BillingAPI.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Mediatr.Handlers.UserHandlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDTO>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetUsersHandler(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            if (request.order == "asc")
            {
                return await _dataContext.Users.OrderBy(b => b.Id).ProjectTo<UserDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
            }
            if (request.order == "desc")
            {
                return await _dataContext.Users.OrderByDescending(b => b.Id).ProjectTo<UserDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
            }
            return await _dataContext.Users.ProjectTo<UserDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
        }
    }
}
