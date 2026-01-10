using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class TblDispute
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? TransactionId { get; set; }

    public string? DisputeType { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ProofPath { get; set; }
}
