using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class TxnReport
{
    public int TransId { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? ServiceName { get; set; }

    public string? OperatorId { get; set; }

    public string? OperatorName { get; set; }

    public string? MobileNo { get; set; }

    public string? AccountNo { get; set; }

    public decimal? Amount { get; set; }

    public decimal? OldBal { get; set; }

    public decimal? Commission { get; set; }

    public decimal? Surcharge { get; set; }

    public decimal? TotalCost { get; set; }

    public decimal? NewBal { get; set; }

    public string? Apiname { get; set; }

    public DateTime? TxnDate { get; set; }

    public DateTime? TxnUpdateDate { get; set; }

    public string? Status { get; set; }

    public string? ApiRequest { get; set; }

    public string? ApiResponse { get; set; }

    public string? CallbackResponse { get; set; }

    public string? ApiMsg { get; set; }

    public string? BeneName { get; set; }

    public string? IfscCode { get; set; }

    public string? BankName { get; set; }

    public string? IpAddress { get; set; }

    public string? TransactionId { get; set; }

    public string? Brid { get; set; }
}
