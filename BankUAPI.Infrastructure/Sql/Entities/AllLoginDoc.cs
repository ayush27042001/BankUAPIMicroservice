using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class AllLoginDoc
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpireAt { get; set; }

    public string? Ipaddress { get; set; }

    public string? UserType { get; set; }
}
