using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BillPayment
{
    public int Id { get; set; }

    public string? OrderId { get; set; }

    public string? BillType { get; set; }

    public string? AccountNo { get; set; }

    public string? Mobile { get; set; }

    public string? Operator { get; set; }

    public decimal? Amount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ApiResponse { get; set; }

    public string? UserId { get; set; }

    public string? OperatorName { get; set; }
}
