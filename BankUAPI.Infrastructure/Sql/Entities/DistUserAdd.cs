using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class DistUserAdd
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? Number { get; set; }

    public string? Status { get; set; }

    public string? ReqDate { get; set; }
}
