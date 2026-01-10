using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public string MobileNo { get; set; } = null!;

    public string? Panno { get; set; }

    public string? Mpin { get; set; }

    public string? Email { get; set; }

    public string? AccountType { get; set; }

    public string? BusinessType { get; set; }

    public string? BusinessPan { get; set; }

    public string? BusinessProof { get; set; }

    public string? NatureOfBusiness { get; set; }

    public string? BankAccount { get; set; }

    public string? Ifsc { get; set; }

    public string? AccountHolderType { get; set; }

    public string? AadharNo { get; set; }

    public string? VoterIdcard { get; set; }

    public string? FaceVerificationResult { get; set; }

    public string? RegistrationStatus { get; set; }

    public string? FullName { get; set; }

    public string? UserType { get; set; }

    public string? RegDate { get; set; }

    public string? Status { get; set; }

    public string? Ipaddress { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? GeoVerification { get; set; }

    public string? AddressUser { get; set; }

    public string? Gender { get; set; }

    public string? Dob { get; set; }

    public string? FatherName { get; set; }

    public string? State { get; set; }

    public string? Postal { get; set; }

    public string? BankName { get; set; }

    public string? AccHolder { get; set; }

    public string? CompanyName { get; set; }

    public string? BusinessStartOn { get; set; }

    public string? CompanyAddress { get; set; }

    public string? ProfileImage { get; set; }

    public string? BusinessProofNo { get; set; }

    public string? Gstno { get; set; }

    public string? ApprovedIp { get; set; }

    public string? ApprovCallback { get; set; }

    public string? OutletId { get; set; }
}
