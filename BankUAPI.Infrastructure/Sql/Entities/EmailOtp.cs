using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class EmailOtp
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Otp { get; set; } 
    public DateTime ExpiresAt { get; set; }
    public bool? IsUsed { get; set; }
    public int? Attempts { get; set; }
    public string? DeviceId { get; set; }
    public string? IpAddress { get; set; }
    public DateTime? CreatedAt { get; set; }
}
