using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.BankAccount
{
    public class Step1Response
    {
        public long UserId { get; set; }
        public string Name { get; set; }
    }
    public class MessageResponse
    {
        public string Result { get; set; }
    }
    public class RegistrationStatusResponse
    {
        public string RegistrationStatus { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
    public class MpinLookupResponse
    {
        public string Mobile { get; set; }
        public string Mpin { get; set; }
        public string Name { get; set; }
    }
}
