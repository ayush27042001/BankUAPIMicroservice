using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Addfund
{
    public int Id { get; set; }

    public int Wlid { get; set; }

    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? OrderId { get; set; }

    public string? TxnId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime ReqDate { get; set; }

    public string? Reqlogs { get; set; }

    public string? ApiResponse { get; set; }

    public decimal? AmountPaid { get; set; }
}
