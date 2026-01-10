using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class ServiceActivation
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? UserMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ServiceType { get; set; }

    public string? Status { get; set; }
}
