﻿using AutoMapper;
using BillingAPI.Data.interfaces;
using BillingAPI.DTOs;
using BillingAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<OrderDTO>> GetAll()
        {
            IEnumerable<Order> orders = await _uow.OrderRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<OrderDTO>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOne(int id)
        {
            Order order = await _uow.OrderRepository.GetById(id);
            return Ok(_mapper.Map<OrderDTO>(order));
        }
    }
}
