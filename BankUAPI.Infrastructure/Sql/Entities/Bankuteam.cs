using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Bankuteam
{
    public int Id { get; set; }

    public string? UserRoll { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? MobileNo { get; set; }

    public string? EmailId { get; set; }

    public string? FullAddress { get; set; }

    public string? AadharCardNo { get; set; }

    public string? PancardNo { get; set; }

    public DateOnly? Dateofjoining { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Status { get; set; }

    public DateTime? ReqDate { get; set; }

    public string? Password { get; set; }
}
