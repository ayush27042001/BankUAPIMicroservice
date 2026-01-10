using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Dmtpay
{
    public int Id { get; set; }

    public string? BeneName { get; set; }

    public string? Ifsccode { get; set; }

    public string? BankTransferMode { get; set; }

    public string? AccountNo { get; set; }

    public string? Amount { get; set; }

    public string? TxnPin { get; set; }

    public string? OrderId { get; set; }

    public string? UserId { get; set; }

    public string? SenderId { get; set; }

    public string? Status { get; set; }

    public string? ApiResponse { get; set; }

    public string? ApiRequest { get; set; }
}
