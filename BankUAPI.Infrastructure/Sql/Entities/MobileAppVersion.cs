using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class MobileAppVersion
{
    public int? Id { get; set; }

    public string? AppVer { get; set; }
}
