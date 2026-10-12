using MediatR;

namespace Chapter5.CQRS.Application.Commands;

public record CreateUserCommand(string Email, string Password, string DisplayName) : IRequest<Chapter5.CQRS.Application.UserDto>;
