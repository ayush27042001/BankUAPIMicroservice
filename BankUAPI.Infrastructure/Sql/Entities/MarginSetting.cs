using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class MarginSetting
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public string? OperatorName { get; set; }

    public decimal? Ipshare { get; set; }

    public decimal? Wlshare { get; set; }

    public string? CommissionType { get; set; }
}
