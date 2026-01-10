using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class UserInvoice
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string? FilePath { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? InvoiceType { get; set; }

    public string? InvoiceId { get; set; }
}
