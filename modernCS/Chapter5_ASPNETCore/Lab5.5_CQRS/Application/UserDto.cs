using System;

namespace Chapter5.CQRS.Application;

public record UserDto(Guid Id, string Email, string DisplayName);
