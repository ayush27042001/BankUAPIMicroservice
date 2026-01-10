using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class DmtSenderRegistration
{
    public int Id { get; set; }

    public string? SenderId { get; set; }

    public string? SenderMobileNo { get; set; }

    public string? SenderName { get; set; }

    public string? SenderGender { get; set; }

    public string? ApiResponse { get; set; }

    public string? ApiRequest { get; set; }
}
