using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BankPayout
{
    public int Id { get; set; }

    public string PayoutTo { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string BeneficiaryName { get; set; } = null!;

    public string Ifsc { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Mode { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? BeneficiaryEmail { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Status { get; set; }

    public string OrderId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? MobileNo { get; set; }

    public string? BankName { get; set; }

    public string? ApiResponse { get; set; }
}
