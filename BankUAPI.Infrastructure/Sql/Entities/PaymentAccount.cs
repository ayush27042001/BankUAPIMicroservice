using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class PaymentAccount
{
    public int Id { get; set; }

    public string? PaymentMethod { get; set; }

    public string? AccountNumber { get; set; }

    public string? BankName { get; set; }

    public string? Ifsc { get; set; }

    public string? BeneficiaryName { get; set; }

    public string? Upiid { get; set; }

    public string? VendorEmail { get; set; }

    public string? VendorId { get; set; }
}
