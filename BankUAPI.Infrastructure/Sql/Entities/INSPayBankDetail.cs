using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class INSPayBankDetail
{
    [Key]
    public int Id { get; set; }

    public int? bankId { get; set; }

    public string? name { get; set; } 

    public string? ifscAlias { get; set; } 

    public string? ifscGlobal { get; set; }

    public int? neftEnabled { get; set; }

    public string? neftFailureRate { get; set; }

    public int? impsEnabled { get; set; } 

    public string? impsFailureRate { get; set; }

    public int? upiEnabled { get; set; }

    public string? upiFailureRate { get; set; }
}
