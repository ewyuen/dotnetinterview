using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Chapter5.CQRS.Application.Queries;
using Chapter5.CQRS.Infrastructure;
using Chapter5.CQRS.Application;

namespace Chapter5.CQRS.Application.Handlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _repo;
    public GetUserByIdHandler(IUserRepository repo) => _repo = repo;

    public Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => _repo.GetByIdAsync(request.Id);
}
