using AutoMapper;
using AutoMapper.QueryableExtensions;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using BillingAPI.Errors;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Data
{
    public class GatewayRepository : IGatewayRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GatewayRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public void Add(Gateway gateway)
        {
            _dataContext.Gateways.Add(gateway);
        }

        public void DeleteById(int id)
        {
            Gateway? gateway = _dataContext.Gateways.Find(id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            _dataContext.Gateways.Remove(gateway);
        }

        public async Task<IEnumerable<GatewayDTO>> GetAll(string order = "")
        {
            if (order == "asc")
            {
                return await _dataContext.Gateways.OrderBy(b => b.Id).ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
            }
            if (order == "desc")
            {
                return await _dataContext.Gateways.OrderByDescending(b => b.Id).ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
            }
            return await _dataContext.Gateways.ProjectTo<GatewayDTO>(
                        _mapper.ConfigurationProvider
                    ).ToListAsync();
        }

        public async Task<Gateway> GetById(int id)
        {
            Gateway? gateway = await _dataContext.Gateways.FindAsync(id);
            if (gateway == null) throw new NotFoundException("Gateway not found");
            return gateway;
        }

        public async Task Update(Gateway gateway)
        {
            if (!await CheckExists(gateway.Id)) throw new NotFoundException("Gateway not found");
            _dataContext.Update(gateway);
        }

        private async Task<bool> CheckExists(int id)
        {
            return await _dataContext.Gateways.AnyAsync(b => b.Id == id);
        }
    }
}
