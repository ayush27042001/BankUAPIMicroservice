using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class UserAgreement
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? AgreementId { get; set; }

    public string? AgreementType { get; set; }

    public string? FilePath { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? FullName { get; set; }
}
