using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Apilog
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? Request { get; set; }

    public string? Responce { get; set; }

    public string? ApiType { get; set; }

    public DateTime? RequestDate { get; set; }
}
