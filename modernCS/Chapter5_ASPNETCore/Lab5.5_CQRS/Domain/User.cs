using System;

namespace Chapter5.CQRS.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}
