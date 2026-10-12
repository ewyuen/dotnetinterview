using MediatR;
using System;

namespace Chapter5.CQRS.Application.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<Chapter5.CQRS.Application.UserDto?>;
