using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Aepslog
{
    public int Id { get; set; }

    public string? TxnType { get; set; }

    public string? OperatorName { get; set; }

    public string? Aadhar { get; set; }

    public string? Mobile { get; set; }

    public string? Bank { get; set; }

    public string? Device { get; set; }

    public DateTime? CreatedAt { get; set; }
}
