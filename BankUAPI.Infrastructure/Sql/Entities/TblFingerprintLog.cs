using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class TblFingerprintLog
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? PidXml { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Ipaddress { get; set; }
}
