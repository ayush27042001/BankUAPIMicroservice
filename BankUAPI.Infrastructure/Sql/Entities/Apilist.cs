using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Apilist
{
    public int Id { get; set; }

    public string? ApiName { get; set; }

    public string? ApiDesc { get; set; }

    public decimal? Amount { get; set; }

    public string? ApiIcon { get; set; }

    public string? Status { get; set; }

    public string? Category { get; set; }
}
