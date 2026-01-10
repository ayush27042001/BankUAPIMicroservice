using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class LoginDoc
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string SessionKey { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpireAt { get; set; }

    public bool IsActive { get; set; }
}
