using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public class Step1Request
    {
        public string Pan { get; set; }
        public string Mpin { get; set; }
        public string ConfirmMpin { get; set; }
        public string Mobile { get; set; }
    }

    public class Step2Request
    {
        public int RegistrationId { get; set; }
        public string AccountType { get; set; }
        public string BusinessType { get; set; }
        public string Email { get; set; }
    }

    public class Step3Request
    {
        public int RegistrationId { get; set; }
        public string BusinessProof { get; set; }
        public string ProofNumber { get; set; }
        public string BusinessName { get; set; }
        public string NatureOfBusiness { get; set; }
        public string BusinessPan { get; set; }
    }

    public class Step4Request
    {
        public int RegistrationId { get; set; }
        public string Aadhaar { get; set; }
        public string Otp { get; set; }
        public string RefId { get; set; }
    }

    public class Step5Request
    {
        public int RegistrationId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IpAddress { get; set; }
        public string RmId { get; set; }
        public int PlanId { get; set; }
    }
    public class AadhaarOtpRequest
    {
        public int RegistrationId { get; set; }
        public string Aadhaar { get; set; }
    }
}
