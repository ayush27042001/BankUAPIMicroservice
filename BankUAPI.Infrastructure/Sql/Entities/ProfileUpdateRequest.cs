using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class ProfileUpdateRequest
{
    public int RequestId { get; set; }

    public int? UserId { get; set; }

    public string? DetailType { get; set; }

    public string? NewValue { get; set; }

    public string? Reason { get; set; }

    public string? RequestStatus { get; set; }

    public DateTime? RequestDate { get; set; }
}
