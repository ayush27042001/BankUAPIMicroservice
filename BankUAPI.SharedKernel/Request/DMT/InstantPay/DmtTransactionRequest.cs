using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class DmtTransactionRequest
    {
        public string? remitterMobileNumber { get; set; }   
        public string? accountNumber { get; set; }   
        public string? ifsc { get; set; }   
        public string? transferMode { get; set; }   
        public string? transferAmount { get; set; }   
        public string? latitude { get; set; }   
        public string? longitude { get; set; }   
        public string? referenceKey { get; set; }   
        public string? otp { get; set; }   
        public string? externalRef { get; set; }   
        public string? IP { get; set; }   
        public string? UserId { get; set; }   
    }
}
