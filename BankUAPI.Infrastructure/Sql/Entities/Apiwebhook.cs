using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Apiwebhook
{
    public int Id { get; set; }

    public string? Link { get; set; }

    public string? ReqPara { get; set; }
}
