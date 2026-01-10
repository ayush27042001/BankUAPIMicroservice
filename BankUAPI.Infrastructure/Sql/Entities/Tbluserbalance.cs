using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Tbluserbalance
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public decimal? OldBal { get; set; }

    public decimal? Amount { get; set; }

    public decimal? NewBal { get; set; }

    public string? TxnType { get; set; }

    public string? CrDrType { get; set; }

    public string? Remarks { get; set; }

    public DateTime? TxnDatetime { get; set; }
}
