using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Recharge
{
    public int Id { get; set; }

    public string MobileNo { get; set; } = null!;

    public string? Operatorname { get; set; }

    public decimal? Amount { get; set; }

    public string? Type { get; set; }

    public string? Circle { get; set; }

    public decimal? CurrBalance { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UserId { get; set; }

    public string? Status { get; set; }

    public string? ApiResponse { get; set; }

    public string? OrderId { get; set; }
}
