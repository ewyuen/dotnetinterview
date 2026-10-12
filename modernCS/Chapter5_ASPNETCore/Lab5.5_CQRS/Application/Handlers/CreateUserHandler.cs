using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Chapter5.CQRS.Application.Commands;
using Chapter5.CQRS.Infrastructure;
using Chapter5.CQRS.Domain;
using Chapter5.CQRS.Application;

namespace Chapter5.CQRS.Application.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _repo;
    public CreateUserHandler(IUserRepository repo) => _repo = repo;

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Id = Guid.NewGuid(), Email = request.Email, DisplayName = request.DisplayName };
        await _repo.AddAsync(user);
        return new UserDto(user.Id, user.Email, user.DisplayName);
    }
}
