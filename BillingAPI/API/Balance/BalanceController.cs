﻿using BillingAPI.API.Balance.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.API.Balance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BalanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all balances
        /// </summary>
        /// <param name="userId">Filter by user</param>
        /// <param name="order">"asc" for ascending, "desc" for descending id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BalanceDTO>>> GetBalances(int? userId = null, string? order = "")
        {
            return Ok(await _mediator.Send(new GetBalancesQuery(userId, order)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BalanceDTO>> GetBalance(int id)
        {
            return Ok(await _mediator.Send(new GetBalanceQuery(id)));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<BalanceDTO>> AddBalance(AddBalanceDTO addBalanceDTO)
        {
            return Ok(await _mediator.Send(new AddBalanceCommand(addBalanceDTO)));
        }
    }
}
