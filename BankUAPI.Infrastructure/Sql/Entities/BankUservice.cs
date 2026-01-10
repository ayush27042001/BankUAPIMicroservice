using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BankUservice
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public string? Status { get; set; }
}
