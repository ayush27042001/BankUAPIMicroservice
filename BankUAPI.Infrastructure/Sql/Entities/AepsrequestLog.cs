using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class AepsrequestLog
{
    public int Personid { get; set; }

    public string? Request { get; set; }

    public string? Responce { get; set; }

    public string? ApiType { get; set; }

    public DateTime? Reqdate { get; set; }
}
