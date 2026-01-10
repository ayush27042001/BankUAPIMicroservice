using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class DmtaddBeneficiary
{
    public int Id { get; set; }

    public string? SenderMobileNo { get; set; }

    public string? BeneficiaryName { get; set; }

    public string? BeneficiaryAccNo { get; set; }

    public string? BankIfscCod { get; set; }

    public string? BeneficiaryBankName { get; set; }

    public string? BeneficiaryMobileNumber { get; set; }

    public string? TrnasferMode { get; set; }

    public string? SenderId { get; set; }

    public string? TransferMode { get; set; }

    public string? BeneficiaryId { get; set; }

    public string? ApiResponse { get; set; }

    public string? ApiRequest { get; set; }
}
