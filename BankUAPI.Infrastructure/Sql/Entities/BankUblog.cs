using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BankUblog
{
    public int Id { get; set; }

    public string? Heading { get; set; }

    public string? Details { get; set; }

    public string? DateTime { get; set; }

    public string? Picture { get; set; }

    public string? Status { get; set; }

    public string? LongDescription { get; set; }

    public string? SecondHeading { get; set; }

    public string? Category { get; set; }
}
