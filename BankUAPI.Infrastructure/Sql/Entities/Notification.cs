using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? Content { get; set; }

    public string? Status { get; set; }
}
