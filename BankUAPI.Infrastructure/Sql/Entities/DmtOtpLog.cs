using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class DmtOtpLog
{
    public int Id { get; set; }

    public string? SenderId { get; set; }

    public string? SenderMobileNo { get; set; }

    public string? ApiRequest { get; set; }

    public string? ApiResponse { get; set; }

    public DateTime? CreatedAt { get; set; }
}
