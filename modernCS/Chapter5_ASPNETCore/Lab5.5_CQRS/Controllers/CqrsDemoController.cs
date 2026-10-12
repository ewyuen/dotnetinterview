using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Chapter5.CQRS.Application.Commands;
using Chapter5.CQRS.Application.Queries;

namespace Chapter5.CQRS.Controllers;

[ApiController]
[Route("api/cqrs")]
public class CqrsDemoController : ControllerBase
{
	private readonly IMediator _mediator;
	public CqrsDemoController(IMediator mediator) => _mediator = mediator;

	[HttpPost("users")]
	public async Task<IActionResult> Create([FromBody] CreateUserCommand cmd)
	{
		var created = await _mediator.Send(cmd);
		return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
	}

	[HttpGet("users/{id:guid}")]
	public async Task<IActionResult> Get(Guid id)
	{
		var result = await _mediator.Send(new GetUserByIdQuery(id));
		return result is null ? NotFound() : Ok(result);
	}
}
