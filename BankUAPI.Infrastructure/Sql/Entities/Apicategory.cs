using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Apicategory
{
    public int Id { get; set; }

    public string? Category { get; set; }

    public string? Status { get; set; }
}
