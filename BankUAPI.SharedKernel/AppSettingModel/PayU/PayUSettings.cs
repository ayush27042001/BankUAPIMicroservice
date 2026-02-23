using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.AppSettingModel.PayU
{
    public class PayUSettings
    {
        public string ClientId { get; set; }
        public string Key { get; set; }
        public string UUID { get; set; }
        public string BaseUrl { get; set; }
        public string VerifyUrl { get; set; }
        public string S2SFlow { get; set; }
        public string SaltKey { get; set; }
        public string Salt { get; set; }
    }
}
