using System;
using System.Threading.Tasks;
using Chapter5.CQRS.Application;

namespace Chapter5.CQRS.Infrastructure;

public interface IUserRepository
{
    Task AddAsync(Chapter5.CQRS.Domain.User user);
    Task<UserDto?> GetByIdAsync(Guid id);
}
