using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class DistAcctiveRequest
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Status { get; set; }

    public string? ReqDate { get; set; }

    public string? TeamSize { get; set; }

    public string? ReqId { get; set; }
}
