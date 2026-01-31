using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.DMT.InstantPay
{
    public class DmtTransactionResponse
    {
        public string? statuscode {  get; set; }
        public string? actcode {  get; set; }
        public string? status {  get; set; }
        public TxnResponse? data {  get; set; }
        public string? timestamp {  get; set; }
        public string? ipay_uuid {  get; set; }
        public string? orderid {  get; set; }
        public string? environment {  get; set; }
        public string? internalCode {  get; set; }
    }

    public class TxnResponse
    {
        public string? externalRef { get; set; }
        public string? poolReferenceId { get; set; }
        public string? txnValue { get; set; }
        public string? txnReferenceId { get; set; }
        public PoolResponse? pool { get; set; }
        public string? remitterMobile { get; set; }
        public string? beneficiaryAccount { get; set; }
        public string? beneficiaryIfsc { get; set; }
        public string? beneficiaryName { get; set; }
    }

    public class PoolResponse
    {
        public string? account { get; set; }
        public string? openingBal { get; set; }
        public string? mode { get; set; }
        public string? amount { get; set; }
        public string? closingBal { get; set; }
    }
}
