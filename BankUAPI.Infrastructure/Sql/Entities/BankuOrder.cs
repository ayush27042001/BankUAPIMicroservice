using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BankuOrder
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? ProductPrice { get; set; }

    public string? Total { get; set; }

    public int? Quantity { get; set; }

    public string? Address { get; set; }

    public string? OrderId { get; set; }

    public string? Status { get; set; }

    public DateTime? OrderDate { get; set; }
}
