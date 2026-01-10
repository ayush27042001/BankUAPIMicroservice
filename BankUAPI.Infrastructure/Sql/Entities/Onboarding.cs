using System;
using System.Collections.Generic;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class Onboarding
{
    public int UserId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? UserType { get; set; }

    public string? FullName { get; set; }

    public string? MobileNo { get; set; }

    public string? EmailId { get; set; }

    public string? Password { get; set; }

    public string? SignupStatus { get; set; }

    public string? MobileverifyStatus { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? Address { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public string? PersonalInfoStatus { get; set; }

    public string? PancardNo { get; set; }

    public string? AadharNo { get; set; }

    public string? VoterCardNo { get; set; }

    public string? PanUpload { get; set; }

    public string? AadharUpload { get; set; }

    public string? VoterCardUpload { get; set; }

    public string? KycStatus { get; set; }

    public string? UdyamUpload { get; set; }

    public string? GstUpload { get; set; }

    public string? JioTagPhotoUpload { get; set; }

    public string? PassportSizePhoto { get; set; }

    public string? RegistrationForm { get; set; }

    public string? DocumentStatus { get; set; }

    public string? AgrementStatus { get; set; }

    public string? AgrementCopy { get; set; }

    public DateTime? RegDate { get; set; }

    public string? Status { get; set; }

    public string? ApprovedBy { get; set; }

    public string? AdminRemarks { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? Companyaddress { get; set; }

    public string? UdyamRegNo { get; set; }

    public string? BusinessType { get; set; }

    public DateOnly? BusinessStartOn { get; set; }

    public string? Businessdetailsstatus { get; set; }

    public string? Userposition { get; set; }

    public string? Education { get; set; }

    public string? AccountHolderName { get; set; }

    public string? BankName { get; set; }

    public string? Accountno { get; set; }

    public string? IfscCode { get; set; }

    public string? Accounttype { get; set; }

    public string? Bankaccountstatus { get; set; }

    public string? Aadharbackcopy { get; set; }

    public string? OnboardingStatus { get; set; }

    public string? NotificationType { get; set; }

    public string? RegistrationStatus { get; set; }
}
