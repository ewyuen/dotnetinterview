using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Chapter5.CQRS.Application;
using Chapter5.CQRS.Domain;

namespace Chapter5.CQRS.Infrastructure;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _store = new();

    public Task AddAsync(User user)
    {
        _store[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task<UserDto?> GetByIdAsync(Guid id)
    {
        if (_store.TryGetValue(id, out var u)) return Task.FromResult<UserDto?>(new UserDto(u.Id, u.Email, u.DisplayName));
        return Task.FromResult<UserDto?>(null);
    }
}
