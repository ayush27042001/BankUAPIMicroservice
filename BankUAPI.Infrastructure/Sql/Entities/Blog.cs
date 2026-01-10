using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Blog
{
    public int Id { get; set; }

    public string? Picture { get; set; }

    public string? Heading { get; set; }

    public string? Content { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Categories { get; set; }

    public string? Longcontent { get; set; }
}
