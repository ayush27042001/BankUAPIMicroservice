using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class BlogCategory
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public string? CreatedDate { get; set; }

    public string? Status { get; set; }
}
