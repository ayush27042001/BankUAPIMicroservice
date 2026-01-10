using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BankUproduct
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDesc { get; set; }

    public string? Amount { get; set; }

    public string? ProductPic { get; set; }

    public string? Status { get; set; }

    public string? Model { get; set; }
}
