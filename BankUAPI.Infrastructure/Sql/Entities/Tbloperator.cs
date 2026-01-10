using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Tbloperator
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public string? OperatorName { get; set; }

    public string? OperatorImage { get; set; }

    public string? Status { get; set; }

    public string? Spkey { get; set; }
}
