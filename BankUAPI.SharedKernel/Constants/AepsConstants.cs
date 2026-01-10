using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Constants
{
    public static class AepsConstants
    {
        public const string Login = "login";
        public const string BiometricKycStatus = "biometrickycstatus";
        public const string BiometricKyc = "biometrickyc";
        public const string Txn = "TXN";
        public const string Tup = "TUP";
        public const string TxType = "BIOMETRIC_KYC_STATUS";
        public const string BioMetricTxnType = "BIOMETRIC_KYC";
        public const string LoginTxnType = "LOGIN";
        public const string BAP = "BAP";
        public const string BAPProdKey = "5011";
        public const string BAPProdName = "BALANCE_ENQUIRY";
        public const string SAPProdKey = "5012";
        public const string SAPProdName = "Mini Statement";
    }

}
