using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.DMT.InstantPay
{
    public class RemitterProfileResponse
    {
        public string StatusCode { get; set; } = default!;
        public string? ActCode { get; set; }
        public string Status { get; set; } = default!;
        public InstantPayRemitterData? Data { get; set; }
        public string ipay_uuid { get; set; } = default!;
        public DateTime Timestamp { get; set; }
        public string? OrderId { get; set; }
        public string? Environment { get; set; }
        public string? InternalCode { get; set; }
        public bool? success { get; set; }
    }

    public class InstantPayRemitterData
    {
        public string MobileNumber { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Pincode { get; set; } = default!;

        public decimal LimitPerTransaction { get; set; }
        public decimal LimitTotal { get; set; }
        public decimal LimitConsumed { get; set; }
        public decimal LimitAvailable { get; set; }

        public LimitDetails LimitDetails { get; set; } = default!;
        public List<Beneficiary> Beneficiaries { get; set; } = new();

        public bool IsTxnOtpRequired { get; set; }
        public bool IsTxnBioAuthRequired { get; set; }
        public bool IsImpsAllowed { get; set; }
        public bool IsNeftAllowed { get; set; }
        public bool IsFaceAuthAvailable { get; set; }

        public DateTime Validity { get; set; }
        public string ReferenceKey { get; set; } = default!;
        public string PidOptionWadh { get; set; } = default!;
    }

    public class LimitDetails
    {
        public decimal MaximumDailyLimit { get; set; }
        public decimal ConsumedDailyLimit { get; set; }
        public decimal AvailableDailyLimit { get; set; }

        public decimal MaximumMonthlyLimit { get; set; }
        public decimal ConsumedMonthlyLimit { get; set; }
        public decimal AvailableMonthlyLimit { get; set; }
    }

    public class Beneficiary
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Account { get; set; } = default!;
        public string Ifsc { get; set; } = default!;
        public string Bank { get; set; } = default!;
        public string BeneficiaryMobileNumber { get; set; } = default!;
        public DateTime VerificationDt { get; set; }
    }
}
